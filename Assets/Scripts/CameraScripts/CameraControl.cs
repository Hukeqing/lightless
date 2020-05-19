using UnityEngine;

namespace CameraScripts
{
    [ExecuteInEditMode]
    public class CameraControl : MonoBehaviour
    {
        private static readonly int ColorGreyRangeId = Shader.PropertyToID("_ColorGreyRange");

        [Range(0, 1)] public float colorGreyRange;

        private Shader _curShader;
        private Material _material;

        public int maxHealth;
        public float decreaseSpeed;

        public GameManager.GameManager gm;

        private int _curHealth;
        private float _showHealth;
        private bool _gameOver;

        public float HealthValue => Mathf.Clamp01(_showHealth / maxHealth + 0.3f);

        private void Start()
        {
            _curShader = Shader.Find("CameraGrey/CameraGreyShader");
            _material = new Material(_curShader) {hideFlags = HideFlags.HideAndDontSave};
            _curHealth = maxHealth;
            _showHealth = _curHealth;
            colorGreyRange = _showHealth / maxHealth;
        }

        private void Update()
        {
            _showHealth = Mathf.Lerp(_showHealth, _curHealth, decreaseSpeed);
            colorGreyRange = _showHealth / maxHealth;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _material.SetFloat(ColorGreyRangeId, colorGreyRange);
            Graphics.Blit(src, dest, _material);
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
    }
}