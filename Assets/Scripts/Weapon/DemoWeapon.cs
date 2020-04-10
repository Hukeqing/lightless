using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Weapon
{
    public class DemoWeapon : Weapon
    {
        public int damage;
        public float maxRange;
        public float lineDisableInt;
        public Transform firePoint;

        private LineRenderer _attackLine;
        private Ray _ray;
        private float _lineDisableTime;

        private void Start()
        {
            _attackLine = GetComponent<LineRenderer>();
            _attackLine.enabled = false;
        }

        private void Update()
        {
            if (_attackLine.enabled && _lineDisableTime <= Time.time)
            {
                _attackLine.enabled = false;
            }
        }

        protected override void ToAttack()
        {
            var firePointPosition = firePoint.position;
            _ray = new Ray(firePointPosition, firePoint.forward);
            _attackLine.enabled = true;
            _attackLine.SetPosition(0, firePointPosition);
            if (Physics.Raycast(_ray, out var hitInfo, maxRange, enemyLayerMask))
            {
                _attackLine.SetPosition(1, hitInfo.point);
                var enemyUnit = hitInfo.collider.GetComponent<Enemy.Unit>();
                if (enemyUnit.IsDie) return;
                enemyUnit.ApplyDamage(damage);
            }
            else
            {
                _attackLine.SetPosition(1, firePointPosition + firePoint.forward * maxRange);
            }

            _lineDisableTime = Time.time + lineDisableInt;
        }
    }
}