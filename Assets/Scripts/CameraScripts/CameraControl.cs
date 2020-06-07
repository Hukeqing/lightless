using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CameraScripts
{
    public enum GameStatus
    {
        Normal,
        ToStop,
        Stop,
        CountDown,
        UnStop
    }

    [ExecuteInEditMode]
    public class CameraControl : MonoBehaviour
    {
        private static readonly int ColorGreyRangeId = Shader.PropertyToID("_ColorGreyRange");
        private static readonly int ColorReRange = Shader.PropertyToID("_ColorReRange");
        private static readonly int ColorStop = Shader.PropertyToID("_ColorStop");
        private static readonly int SampleStrength = Shader.PropertyToID("_SampleStrength");
        private static readonly int SampleDist = Shader.PropertyToID("_SampleDist");

        private Material _material;
        private Material _noiseMaterial;
        private Material _dimMaterial;

        public int maxHealth;
        public float decreaseSpeed;
        [Range(0, 2f)] public float maxCameraValue;

        public GameManager.GameManager gm;
        public Player.PlayerControl pc;

        public Text countDownText;

        public float stopCostTime;
        private GameStatus _gameStatus;
        private float _stopTime;
        private AudioSource _countDownAudio;

        // private AudioSource _beats;

        private int _curHealth;
        private float _showHealth;

        public bool GameOver { get; private set; }

        public float HealthValue => Mathf.Clamp01(_showHealth / maxHealth + 0.3f);

        private void Start()
        {
            _material = new Material(Shader.Find("CameraGrey/CameraGreyShader"))
                {hideFlags = HideFlags.HideAndDontSave};
            _noiseMaterial = new Material(Shader.Find("CameraGrey/Drift"))
                {hideFlags = HideFlags.HideAndDontSave};
            _dimMaterial = new Material(Shader.Find("CameraGrey/Dim"))
                {hideFlags = HideFlags.HideAndDontSave};
            _countDownAudio = countDownText.GetComponent<AudioSource>();
            // _beats = pc.GetComponent<AudioSource>();
            countDownText.text = "";
            _curHealth = maxHealth;
            _showHealth = _curHealth;
            _gameStatus = GameStatus.Normal;
        }

        private void Update()
        {
            _showHealth = Mathf.Lerp(_showHealth, _curHealth, decreaseSpeed);
            // if (HealthValue <= 0.8f)
            // {
            //     _beats.Play();
            //     _beats.pitch = 2 - HealthValue;
            //     _beats.volume = 1.3f - HealthValue;
            // }
            // else
            // {
            //     _beats.Stop();
            // }

            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GameStop();
                    }

                    break;
                case GameStatus.ToStop:
                    if (Time.time - _stopTime > stopCostTime)
                    {
                        _gameStatus = GameStatus.Stop;
                        Time.timeScale = 0;
                    }

                    break;
                case GameStatus.Stop:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GameStop();
                    }

                    break;
                case GameStatus.CountDown:
                    break;
                case GameStatus.UnStop:
                    if (Time.time - _stopTime > stopCostTime)
                    {
                        _gameStatus = GameStatus.Normal;
                        _material.SetFloat(ColorStop, -1);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _material.SetFloat(ColorGreyRangeId, _showHealth / maxHealth * maxCameraValue);
            _material.SetFloat(ColorReRange,
                _gameStatus == GameStatus.ToStop ||
                _gameStatus == GameStatus.Stop ||
                _gameStatus == GameStatus.CountDown
                    ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                    : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);

            _dimMaterial.SetFloat(SampleStrength, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);
            _dimMaterial.SetFloat(SampleDist, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);

            var rt1 = RenderTexture.GetTemporary(src.width, src.height);
            var rt2 = RenderTexture.GetTemporary(src.width, src.height);
            Graphics.Blit(src, rt1, _material);

            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    Graphics.Blit(rt1, rt2);
                    break;
                case GameStatus.ToStop:
                case GameStatus.Stop:
                case GameStatus.CountDown:
                case GameStatus.UnStop:
                    _noiseMaterial.SetFloat(ColorReRange,
                        _gameStatus == GameStatus.ToStop ||
                        _gameStatus == GameStatus.Stop ||
                        _gameStatus == GameStatus.CountDown
                            ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                            : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);
                    Graphics.Blit(rt1, rt2, _noiseMaterial);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Graphics.Blit(rt2, dest, _dimMaterial);
            RenderTexture.ReleaseTemporary(rt2);
            RenderTexture.ReleaseTemporary(rt1);
        }

        public void ApplyDamage(int damage)
        {
            if (GameOver) return;
            _curHealth -= damage;
            if (_curHealth > 0) return;
            GameOver = true;
            _curHealth = 0;
            pc.GameOver();
            gm.GameOver();
        }

        public void AddHealth(int cure)
        {
            _curHealth += cure;
            if (_curHealth <= maxHealth) return;
            _curHealth = maxHealth;
        }

        private void GameStop()
        {
            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    _stopTime = Time.time;
                    _gameStatus = GameStatus.ToStop;
                    _material.SetFloat(ColorStop, -1);
                    break;
                case GameStatus.ToStop:
                    break;
                case GameStatus.Stop:
                    _gameStatus = GameStatus.CountDown;
                    StartCoroutine(CountDownShow());
                    break;
                case GameStatus.CountDown:
                    break;
                case GameStatus.UnStop:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator CountDownShow()
        {
            _countDownAudio.Play();
            for (var i = 3; i > 0; --i)
            {
                yield return new WaitForSecondsRealtime(1);
                countDownText.text = i.ToString();
            }

            yield return new WaitForSecondsRealtime(1);
            countDownText.text = "";
            _gameStatus = GameStatus.UnStop;
            Time.timeScale = 1;
            _stopTime = Time.time;
        }
    }
}