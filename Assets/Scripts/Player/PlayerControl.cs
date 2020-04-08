﻿using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;
        public Camera mainCamera;
        private CameraControl _cc;
        public float costTime;

        private float _nextCostTime;

        // TODO remove the following code
        public Weapon.Weapon weapon;

        private void Start()
        {
            _nextCostTime = Time.time + costTime;
            _cc = mainCamera.GetComponent<CameraControl>();
        }

        private void Update()
        {
            transform.Translate(Time.deltaTime * moveSpeed * _cc.HealthValue *
                                new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), Space.World);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 1000.0f, 1 << 9))
            {
                var target = hitInfo.point;
                target.y = transform.position.y;
                transform.LookAt(target);
            }

            if (Input.GetMouseButton(0))
            {
                weapon.Attack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // TODO take thing
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