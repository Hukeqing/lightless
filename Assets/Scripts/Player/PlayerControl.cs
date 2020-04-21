using System;
using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed;
        public Camera mainCamera;
        public CameraFollower cf;
        public float costTime;
        public Weapon.Weapon weapon;

        private float _nextCostTime;
        private PackageControl _pc;
        private CameraControl _cc;


        private void Start()
        {
            _nextCostTime = Time.time + costTime;
            _cc = mainCamera.GetComponent<CameraControl>();
            _pc = GetComponent<PackageControl>();
        }

        private void FixedUpdate()
        {
            transform.Translate(Time.deltaTime * moveSpeed * _cc.HealthValue *
                                new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), Space.World);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hitInfo, 1000.0f, 1 << 9)) return;
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
            cf.CameraFollow();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && weapon != null)
            {
                weapon.Attack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _pc.ApplyItem();
            }

            if (!(Time.time >= _nextCostTime)) return;
            _nextCostTime = Time.time + costTime;
            _cc.ApplyDamage(1);
        }

        public void ApplyDamage(int damage)
        {
            _cc.ApplyDamage(damage);
        }

        public void AddHealth(int cure)
        {
            _cc.AddHealth(cure);
        }
    }
}