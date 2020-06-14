using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CameraScripts
{
    /// <summary>
    /// 游戏的暂停状态
    /// </summary>
    public enum GameStatus
    {
        Normal,
        ToStop,
        Stop,
        CountDown,
        UnStop
    }

    /// <summary>
    /// 主摄像机控制
    /// </summary>
    [ExecuteInEditMode]
    public class CameraControl : MonoBehaviour
    {
        #region shader的属性值

        private static readonly int ColorGreyRangeId = Shader.PropertyToID("_ColorGreyRange");
        private static readonly int ColorReRange = Shader.PropertyToID("_ColorReRange");
        private static readonly int ColorStop = Shader.PropertyToID("_ColorStop");
        private static readonly int SampleStrength = Shader.PropertyToID("_SampleStrength");
        private static readonly int SampleDist = Shader.PropertyToID("_SampleDist");

        #endregion

        #region 三层主摄像机shader渲染材质

        public Material material;
        public Material noiseMaterial;
        public Material dimMaterial;

        #endregion

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
        private AudioSource _breath;

        public int CurHealth { get; private set; }
        private float _showHealth;

        public bool GameOver { get; private set; }

        public float HealthValue => Mathf.Clamp01(_showHealth / maxHealth + 0.3f);

        #region BGM

        public AudioClip[] terrorAudioClips;
        public AudioSource terrorAudioSource;

        private float _nextAudioTime;

        #endregion

        private void Start()
        {
            _gameStatus = GameStatus.Normal;
            _countDownAudio = countDownText.GetComponent<AudioSource>();
            _breath = GetComponent<AudioSource>();
            countDownText.text = "";
            CurHealth = maxHealth;
            _showHealth = CurHealth;
            _gameStatus = GameStatus.Normal;
            _nextAudioTime = Time.time + Random.Range(10, 30);
        }

        private void Update()
        {
            _showHealth = Mathf.Lerp(_showHealth, CurHealth, decreaseSpeed);

            if (HealthValue <= 0.6f)
            {
                if (!_breath.isPlaying)
                {
                    _breath.Play();
                }

                _breath.volume = 1 - HealthValue;
            }

            if ((HealthValue >= 0.7f || CurHealth <= 0) && _breath.isPlaying)
            {
                _breath.Stop();
            }

            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0.5f)
                    {
                        GameStop();
                    }

                    break;
                case GameStatus.ToStop:
                    if (Time.time - _stopTime > stopCostTime)
                    {
                        _gameStatus = GameStatus.Stop;
                        Time.timeScale = 0;
                        if (terrorAudioSource.isPlaying) terrorAudioSource.Pause();
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
                        material.SetFloat(ColorStop, -1);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!(Time.time >= _nextAudioTime) || terrorAudioSource.isPlaying) return;
            var tmp = Random.Range(0, terrorAudioClips.Length);
            terrorAudioSource.clip = terrorAudioClips[tmp];
            terrorAudioSource.Play();
            _nextAudioTime = Time.time + terrorAudioClips[tmp].length + Random.Range(10, 30);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            material.SetFloat(ColorGreyRangeId, _showHealth / maxHealth * maxCameraValue);
            material.SetFloat(ColorReRange,
                _gameStatus == GameStatus.ToStop ||
                _gameStatus == GameStatus.Stop ||
                _gameStatus == GameStatus.CountDown
                    ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                    : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);

            dimMaterial.SetFloat(SampleStrength, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);
            dimMaterial.SetFloat(SampleDist, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);

            var rt1 = RenderTexture.GetTemporary(src.width, src.height);
            var rt2 = RenderTexture.GetTemporary(src.width, src.height);
            Graphics.Blit(src, rt1, material);

            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    Graphics.Blit(rt1, rt2);
                    break;
                case GameStatus.ToStop:
                case GameStatus.Stop:
                case GameStatus.CountDown:
                case GameStatus.UnStop:
                    noiseMaterial.SetFloat(ColorReRange,
                        _gameStatus == GameStatus.ToStop ||
                        _gameStatus == GameStatus.Stop ||
                        _gameStatus == GameStatus.CountDown
                            ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                            : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);
                    Graphics.Blit(rt1, rt2, noiseMaterial);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Graphics.Blit(rt2, dest, dimMaterial);
            RenderTexture.ReleaseTemporary(rt2);
            RenderTexture.ReleaseTemporary(rt1);
        }

        public void ApplyDamage(int damage)
        {
            if (GameOver) return;
            damage = Mathf.RoundToInt(damage / Mathf.Exp(-0.005f * gm.GameScore));
            CurHealth -= damage;
            if (CurHealth > 0) return;
            GameOver = true;
            CurHealth = 0;
            pc.GameOver();
            gm.GameOver();
        }

        public void AddHealth(int cure)
        {
            if (GameOver) return;
            cure = Mathf.RoundToInt(cure * Mathf.Exp(-0.005f * gm.GameScore));
            CurHealth += cure;
            if (CurHealth <= maxHealth) return;
            CurHealth = maxHealth;
        }

        private void GameStop()
        {
            if (GameOver) return;
            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    _stopTime = Time.time;
                    _gameStatus = GameStatus.ToStop;
                    material.SetFloat(ColorStop, -1);
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
                countDownText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }

            countDownText.text = "";
            _gameStatus = GameStatus.UnStop;
            Time.timeScale = 1;
            terrorAudioSource.UnPause();
            _stopTime = Time.time;
        }
    }
}