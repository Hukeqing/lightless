using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        public float coolDown;
        public LayerMask enemyLayerMask;
        private float _nextAttack;

        protected void Init()
        {
            _nextAttack = 0;
        }

        public void Attack()
        {
            if (_nextAttack > Time.time) return;
            _nextAttack = Time.time + coolDown;
            ToAttack();
        }

        protected virtual void ToAttack()
        {
            Debug.Log("No Weapon~");
        }
    }
}