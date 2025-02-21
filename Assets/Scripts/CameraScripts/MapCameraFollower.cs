﻿using UnityEngine;

namespace CameraScripts
{
    public class MapCameraFollower : MonoBehaviour
    {
        public Transform player;
        public Shader curShader;
        
        private Player.PlayerControl _pc;
        private Material _material;
        private static readonly int Probability = Shader.PropertyToID("_Probability");

        private void Start()
        {
            _material = new Material(curShader) {hideFlags = HideFlags.HideAndDontSave};
            _pc = player.GetComponent<Player.PlayerControl>();
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _material.SetFloat(Probability, 1 - (Time.timeScale <= 0.01f ? 0 : _pc.HealthValue));
            Graphics.Blit(src, dest, _material);
        }
    }
}