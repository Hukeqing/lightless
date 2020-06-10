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
        public float maxDistance = 3;

        private Vector3 _target;
        private bool _hit;

        private void Start()
        {
            _hit = false;
            _target = pc.transform.position + Vector3.up;
            transform.LookAt(_target);
        }

        private void Update()
        {
            if (_hit) return;

            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            if (Vector3.Distance(_target, transform.position) > 0.1f) return;

            _hit = true;

            damage = Mathf.CeilToInt(Mathf.Clamp(
                damage * (1 - Vector3.Distance(_target, pc.transform.position + Vector3.up) /
                    maxDistance), 0, damage));

            pc.ApplyDamage(damage);
            a.Play();
            b.Play();
            Destroy(gameObject, 2);
        }
    }
}