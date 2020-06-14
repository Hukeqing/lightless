using UnityEngine;

namespace Weapon
{
    public class PistolWeapon : Weapon
    {
        public int damage;
        public float maxRange;
        public float lineDisableInt;
        public Transform firePoint;
        public LineRenderer attackLine;

        private Ray _ray;
        private float _lineDisableTime;

        private void Start()
        {
            attackLine.gameObject.SetActive(false);
            Init();
        }

        private void Update()
        {
            if (attackLine.enabled && _lineDisableTime <= Time.time)
            {
                attackLine.gameObject.SetActive(false);
            }
        }

        public override void Attack()
        {
        }

        public override void AttackDown()
        {
            if (nextAttack > Time.time) return;

            PlayAudio();

            if (curWeaponCost <= 0.001f) return;

            nextAttack = Time.time + coolDown;
            WeaponCost(weaponCost);
            var firePointPosition = firePoint.position;
            _ray = new Ray(firePointPosition, firePoint.forward);
            attackLine.gameObject.SetActive(true);
            attackLine.SetPosition(0, firePointPosition);
            if (Physics.Raycast(_ray, out var hitInfo, maxRange, hitLayerMask))
            {
                attackLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    var enemyUnit = hitInfo.collider.GetComponent<Enemy.Unit>();
                    enemyUnit.ApplyDamage(damage);
                }
            }
            else
            {
                attackLine.SetPosition(1, firePointPosition + firePoint.forward * maxRange);
            }

            _lineDisableTime = Time.time + lineDisableInt;
        }
    }
}