using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyMove : MonoBehaviour
    {
        public Transform player;
        public float moveSpeed;
        public float viewRange;

        private void Update()
        {
            var target = (player.position - transform.position).normalized;
            if (!(Vector3.Dot(target, transform.forward) >= 0.866f)) return;
            var ray = new Ray(transform.position, target);
            if (!Physics.Raycast(ray, out var hitInfo, viewRange)) return;
            if (hitInfo.collider.gameObject.layer != 10) return;
            transform.LookAt(player);
            transform.Translate(target * (moveSpeed * Time.deltaTime), Space.World);
        }
    }
}