using UnityEngine;

namespace Weapon
{
    public class LaserWeapon : Weapon
    {
        public int damage;
        public float maxRange;
        public Transform firePoint;
        public LineRenderer attackLine;

        private Ray _ray;
        private bool _onAttack;

        private void Start()
        {
            attackLine.gameObject.SetActive(false);
            _onAttack = false;
        }

        public override void Attack()
        {
        }

        public override void AttackDown()
        {
            _onAttack = true;
        }

        public override void AttackUp()
        {
            _onAttack = false;
        }

        private void Update()
        {
            attackLine.gameObject.SetActive(_onAttack);
            if (curWeaponCost <= 0) _onAttack = false;
            if (!_onAttack) return;
            var firePointPosition = firePoint.position;
            _ray = new Ray(firePointPosition, firePoint.forward);
            attackLine.SetPosition(0, firePointPosition);
            if (Physics.Raycast(_ray, out var hitInfo, maxRange, hitLayerMask))
            {
                attackLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    var enemyUnit = hitInfo.collider.GetComponent<Enemy.Unit>();
                    if (enemyUnit.IsDie) return;
                    Debug.Log((int) Mathf.Ceil(damage * Time.deltaTime));
                    enemyUnit.ApplyDamage((int) Mathf.Ceil(damage * Time.deltaTime));
                }
            }
            else
            {
                attackLine.SetPosition(1, firePointPosition + firePoint.forward * maxRange);
            }

            WeaponCost(weaponCost * Time.deltaTime);
        }
    }
}