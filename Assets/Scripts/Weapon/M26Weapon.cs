using Effect;
using UnityEngine;

namespace Weapon
{
    public class M26Weapon : Weapon
    {
        public float maxDistance;
        public float distanceUp;
        public GameObject m26;
        public Transform target;

        private float _curDistance;
        private Ray _ray;
        private int _curDir;

        private void Start()
        {
            _curDistance = 0;
            _curDir = 1;
            Init();
        }

        public override void Attack()
        {
            if (curWeaponCost <= 0.001f) return;
            _curDistance += distanceUp * Time.deltaTime * _curDir;

            _ray = new Ray(firePoint.position, firePoint.forward);
            if (!Physics.Raycast(_ray, out var hitInfo, 10000, hitLayerMask)) return;
            var maxBuildDistance = Mathf.Min(Vector3.Distance(hitInfo.point, firePoint.position), maxDistance);

            if (_curDistance > maxBuildDistance || _curDistance <= 0) _curDir = -_curDir;
            target.localPosition = Vector3.up * _curDistance;
        }

        public override void AttackDown()
        {
            PlayAudio();
            if (curWeaponCost <= 0.001f) return;
            target.gameObject.SetActive(true);
        }

        public override void AttackUp()
        {
            if (curWeaponCost <= 0.001f) return;
            var curM26 = Instantiate(m26, firePoint.position, firePoint.rotation);
            curM26.GetComponent<M26>().target = target.position;
            target.gameObject.SetActive(false);
            WeaponCost(weaponCost);
            _curDistance = 0;
        }
    }
}