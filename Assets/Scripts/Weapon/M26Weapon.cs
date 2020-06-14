using Effect;
using UnityEngine;

namespace Weapon
{
    public class M26Weapon : Weapon
    {
        public float maxDistance;
        public float distanceUp;
        public Transform firePoint;
        public GameObject m26;
        public Transform target;

        private float _curDistance;
        private Ray _ray;

        private void Start()
        {
            _curDistance = 0;
            Init();
        }

        public override void Attack()
        {
            if (curWeaponCost <= 0.001f) return;
            _curDistance += distanceUp * Time.deltaTime;
            if (_curDistance > maxDistance) _curDistance = 0;

            _ray = new Ray(firePoint.position, firePoint.forward);
            if (!Physics.Raycast(_ray, out var hitInfo, 10000, hitLayerMask)) return;
            var maxBuildDistance = Vector3.Distance(hitInfo.point, firePoint.position);
            target.localPosition = Vector3.up * Mathf.Clamp(_curDistance, 0, maxBuildDistance);
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
            WeaponCost(1);
        }
    }
}