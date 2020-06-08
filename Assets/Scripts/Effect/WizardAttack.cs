using System;
using UnityEngine;

namespace Effect
{
    public class WizardAttack : MonoBehaviour
    {
        [HideInInspector] public Player.PlayerControl pc;
        public int damage;
        public float moveSpeed;
        public ParticleSystem a, b;

        private bool _hit;

        private void Start()
        {
            _hit = false;
        }

        private void Update()
        {
            if (_hit) return;
            Transform transform1;
            (transform1 = transform).LookAt(pc.transform.position + Vector3.up);
            transform.Translate(transform1.forward * (Time.deltaTime * moveSpeed), Space.World);
            if (!(Vector3.Distance(transform1.position, pc.transform.position + Vector3.up) < 0.1f)) return;
            _hit = true;
            pc.ApplyDamage(damage);
            a.Play();
            b.Play();
            Destroy(gameObject, 2);
        }
    }
}