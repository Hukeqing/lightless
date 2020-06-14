using UnityEngine;

namespace Weapon
{
    public class M40A3Weapon : Weapon
    {
        public int damage;
        public float maxRange;
        public float lineDisableInt;
        public Transform firePoint;
        public LineRenderer attackLine;
        public Camera shotCamera;

        private Ray _ray;
        private float _lineDisableTime;
        private Animator _animator;
        private Camera _mainCamera;
        private static readonly int ShotTrigger = Animator.StringToHash("Shot");
        private GameObject _message;

        private void Start()
        {
            attackLine.gameObject.SetActive(false);
            Init();
            _animator = GetComponent<Animator>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _message = GameObject.FindGameObjectWithTag("MouthHitMessage").transform.GetChild(0).gameObject;
        }

        public override void Attack()
        {
        }

        private void Update()
        {
            if (!attackLine.enabled || !(_lineDisableTime <= Time.time)) return;
            attackLine.gameObject.SetActive(false);
        }

        public override void AttackDown()
        {
            if (nextAttack > Time.time) return;


            if (curWeaponCost <= 0.001f)
            {
                PlayAudio();
                return;
            }

            Time.timeScale = 0;
            _animator.SetTrigger(ShotTrigger);
            _mainCamera.enabled = false;
            _message.SetActive(true);
        }

        // ReSharper disable once UnusedMember.Global
        public void Shot()
        {
            _message.SetActive(false);
            attackLine.gameObject.SetActive(true);
            attackLine.SetPosition(0, firePoint.position);

            var ray = shotCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, maxRange, hitLayerMask))
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
                attackLine.SetPosition(1, firePoint.position + firePoint.forward * maxRange);
            }

            PlayAudio();

            nextAttack = Time.time + coolDown;
            WeaponCost(weaponCost);
            _lineDisableTime = Time.time + lineDisableInt;
        }

        // ReSharper disable once UnusedMember.Global
        public void OverAnimation()
        {
            _mainCamera.enabled = true;
            Time.timeScale = 1;
        }
    }
}