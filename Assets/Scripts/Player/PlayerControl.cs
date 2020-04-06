using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;

        public CameraControl cc;
        public float costTime;

        private float _nextCostTime;

        // TODO remove the following code
        public Weapon.Weapon weapon;

        private void Start()
        {
            _nextCostTime = Time.time + costTime;
        }

        private void Update()
        {
            transform.Translate(Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed * cc.HealthValue *
                                Vector3.forward);
            transform.Rotate(Time.deltaTime * Input.GetAxis("Horizontal") * rotateSpeed * cc.HealthValue * Vector3.up);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                weapon.Attack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // TODO take thing
            }
            if (!(Time.time >= _nextCostTime)) return;
            _nextCostTime = Time.time + costTime;
            cc.ApplyDamage(1);
        }

        public void ApplyDamage(int damage)
        {
            cc.ApplyDamage(damage);
        }

        public void AddHealth(int cure)
        {
            cc.AddHealth(cure);
        }
    }
}