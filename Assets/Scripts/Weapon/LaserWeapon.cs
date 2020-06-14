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
        private float _hsvH;
        private float _hsvS;
        private float _hsvV;

        private void Start()
        {
            attackLine.gameObject.SetActive(false);
            Color.RGBToHSV(Color.red, out _hsvH, out _hsvS, out _hsvV);
            Init();
        }

        public override void Attack()
        {
            if (curWeaponCost <= 0) return;

            var firePointPosition = firePoint.position;
            _ray = new Ray(firePointPosition, firePoint.forward);
            attackLine.SetPosition(0, firePointPosition);
            if (Physics.Raycast(_ray, out var hitInfo, maxRange, hitLayerMask))
            {
                attackLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    var enemyUnit = hitInfo.collider.GetComponent<Enemy.Unit>();
                    enemyUnit.ApplyDamage((int) Mathf.Ceil(damage * Time.deltaTime));
                }
            }
            else
            {
                attackLine.SetPosition(1, firePointPosition + firePoint.forward * maxRange);
            }

            _hsvH += Time.deltaTime;
            _hsvH -= (int) _hsvH;
            var tColor = Color.HSVToRGB(_hsvH, _hsvS, _hsvV);
            attackLine.startColor = tColor;
            attackLine.endColor = tColor;
            WeaponCost(weaponCost * Time.deltaTime);
        }

        public override void AttackDown()
        {
            attackLine.gameObject.SetActive(true);
            PlayAudio();
        }

        public override void AttackUp()
        {
            attackLine.gameObject.SetActive(false);
            weaponAudioSource.Stop();
        }
    }
}