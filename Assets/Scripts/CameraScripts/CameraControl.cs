using System;
using UnityEngine;

namespace CameraScripts
{
    public enum GameStatus
    {
        Normal,
        ToStop,
        Stop,
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
        [Range(0, 1.5f)]public float maxCameraValue;

        public GameManager.GameManager gm;

        public float stopCostTime;
        private GameStatus _gameStatus;
        private float _stopTime;

        private int _curHealth;
        private float _showHealth;
        private bool _gameOver;

        public float HealthValue => Mathf.Clamp01(_showHealth / maxHealth + 0.3f);

        private void Start()
        {
            _material = new Material(Shader.Find("CameraGrey/CameraGreyShader"))
                {hideFlags = HideFlags.HideAndDontSave};
            _noiseMaterial = new Material(Shader.Find("CameraGrey/Drift"))
                {hideFlags = HideFlags.HideAndDontSave};
            _dimMaterial = new Material(Shader.Find("CameraGrey/Dim"))
                {hideFlags = HideFlags.HideAndDontSave};
            _curHealth = maxHealth;
            _showHealth = _curHealth;
            _gameStatus = GameStatus.Normal;
        }

        private void Update()
        {
            _showHealth = Mathf.Lerp(_showHealth, _curHealth, decreaseSpeed);

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
                _gameStatus == GameStatus.ToStop || _gameStatus == GameStatus.Stop
                    ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                    : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);

            _dimMaterial.SetFloat(SampleStrength, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);
            _dimMaterial.SetFloat(SampleDist, (maxCameraValue - _showHealth / maxHealth * maxCameraValue) * 2);

            var rt1 = RenderTexture.GetTemporary(src.width, src.height);

            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    Graphics.Blit(src, rt1, _material);
                    Graphics.Blit(rt1, dest, _dimMaterial);
                    break;
                case GameStatus.ToStop:
                case GameStatus.Stop:
                case GameStatus.UnStop:
                    _noiseMaterial.SetFloat(ColorReRange,
                        _gameStatus == GameStatus.ToStop || _gameStatus == GameStatus.Stop
                            ? Mathf.Abs(Time.time - _stopTime) / stopCostTime
                            : 1 - Mathf.Abs(Time.time - _stopTime) / stopCostTime);

                    // _noiseMaterial.SetFloat(TwistIntensity, 1.1f - _showHealth / maxHealth * 1.1f);

                    var rt2 = RenderTexture.GetTemporary(src.width, src.height);

                    Graphics.Blit(src, rt1, _material);
                    Graphics.Blit(rt1, rt2, _noiseMaterial);
                    Graphics.Blit(rt2, dest, _dimMaterial);

                    RenderTexture.ReleaseTemporary(rt2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RenderTexture.ReleaseTemporary(rt1);
        }

        public void ApplyDamage(int damage)
        {
            if (_gameOver) return;
            _curHealth -= damage;
            if (_curHealth > 0) return;
            _curHealth = 0;
            _gameOver = true;
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
            _stopTime = Time.time;
            switch (_gameStatus)
            {
                case GameStatus.Normal:
                    _gameStatus = GameStatus.ToStop;
                    _material.SetFloat(ColorStop, -1);
                    break;
                case GameStatus.ToStop:
                    break;
                case GameStatus.Stop:
                    _gameStatus = GameStatus.UnStop;
                    Time.timeScale = 1;
                    break;
                case GameStatus.UnStop:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}