using System;
using UnityEngine;

namespace CameraScripts
{
    public class MapCameraFollower : MonoBehaviour
    {
        public Transform player;
        public float baseFlushTime;

        private Player.PlayerControl _pc;
        private RenderTexture _lastTexture;
        private float _nextFlushTime;

        private void Start()
        {
            _pc = player.GetComponent<Player.PlayerControl>();
            _lastTexture = new RenderTexture(GetComponent<Camera>().targetTexture.width,
                GetComponent<Camera>().targetTexture.height, 0);
        }

        private void LateUpdate()
        {
            transform.position = player.position + new Vector3(0, 5, 0);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (Time.time >= _nextFlushTime)
            {
                Graphics.Blit(src, _lastTexture);
                _nextFlushTime = Time.time + baseFlushTime + 1 - _pc.HealthValue;
            }

            Graphics.Blit(_lastTexture, dest);
        }
    }
}