using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed;
        public Camera mainCamera;
        public CameraFollower cf;
        public float damageTime;
        public float damageUpgradeTime = 0.99f;
        public Weapon.Weapon weapon;
        public float HealthValue => _cc.HealthValue;

        private float _curDamageTime;
        private float _nextDamageTime;
        private PackageControl _pc;
        private CameraControl _cc;
        private Animator _playerAnimator;

        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        private void Start()
        {
            _curDamageTime = damageTime;
            _nextDamageTime = Time.time + _curDamageTime;
            _cc = mainCamera.GetComponent<CameraControl>();
            _pc = GetComponent<PackageControl>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            var moveVec3 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveVec3.Normalize();
            transform.Translate(Time.deltaTime * moveSpeed * HealthValue * moveVec3, Space.World);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hitInfo, 1000.0f, 1 << 9)) return;
            var target = hitInfo.point;
            target.y = transform.position.y;
            _playerAnimator.SetFloat(MoveSpeed, moveVec3.magnitude);
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

            if (!(Time.time >= _nextDamageTime)) return;
            _nextDamageTime = Time.time + _curDamageTime;
            _curDamageTime *= damageUpgradeTime;
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

        public void ApplySlowDamage(float value)
        {
            _curDamageTime *= value;
        }
    }
}