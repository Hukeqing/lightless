using System;
using UnityEngine;

namespace CameraScripts
{
    public class MapCameraFollower : MonoBehaviour
    {
        public Transform player;
        public float baseFlushTime;

        private Player.PlayerControl _pc;
        private float _nextFlushTime;

        private void Start()
        {
            _pc = player.GetComponent<Player.PlayerControl>();
        }

        private void LateUpdate()
        {
            if (!(Time.time > _nextFlushTime)) return;
            transform.position = player.position + new Vector3(0, 5, 0);
            _nextFlushTime = Time.time + baseFlushTime + 1 - _pc.HealthValue;
        }
    }
}