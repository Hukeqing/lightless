using System;
using CameraScripts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed;
        public Camera mainCamera;
        public CameraFollower cf;
        public GameManager.GameManager gm;
        public Image weaponCostImage;
        public float damageTime;
        [Range(0.9f, 0.99f)] public float damageUpgradeTime = 0.99f;
        [HideInInspector] public Weapon.Weapon weapon;
        public Animator beHurt;
        public AudioClip playerDie;
        public float HealthValue => _cc.HealthValue;

        private float _curDamageTime;
        private float _nextDamageTime;
        private PackageControl _pc;
        private CameraControl _cc;
        private Animator _playerAnimator;
        private AudioSource _playerHurt;

        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Hurt = Animator.StringToHash("Hurt");

        private void Start()
        {
            _curDamageTime = damageTime;
            _nextDamageTime = Time.time + _curDamageTime;
            _cc = mainCamera.GetComponent<CameraControl>();
            _pc = GetComponent<PackageControl>();
            _playerAnimator = GetComponent<Animator>();
            _playerHurt = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (_cc.GameOver) return;
            var moveVec3 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveVec3.Normalize();
            transform.Translate(Time.fixedDeltaTime * moveSpeed * HealthValue * moveVec3, Space.World);
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
            if (_cc.GameOver) return;
            if (Mathf.Abs(Time.deltaTime) < 0.01f) return;
            if (weapon != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    weapon.AttackDown();
                }

                if (Input.GetMouseButton(0))
                {
                    weapon.Attack();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    weapon.AttackUp();
                }
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
            if (_cc.CurHealth <= 0) return;
            _cc.ApplyDamage(damage);
            if (damage <= 0) return;
            _playerHurt.Play();
            beHurt.SetTrigger(Hurt);
        }

        public void AddHealth(int cure)
        {
            _cc.AddHealth(cure);
        }

        public void ApplySlowDamage(float value)
        {
            value = 1 + (value - 1) * Mathf.Exp(-0.005f * gm.GameScore);
            _curDamageTime *= value;
        }

        public void GameOver()
        {
            _playerAnimator.SetTrigger(Death);
            _playerHurt.clip = playerDie;
            _playerHurt.Play();
        }

        [UsedImplicitly]
        public void RestartLevel()
        {
            gm.ShowScore();
        }
    }
}