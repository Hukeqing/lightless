using UnityEngine;

namespace Enemy
{
    public class Unit : MonoBehaviour
    {
        public int maxHealth;

        public bool IsDie => _curHealth <= 0;

        private int _curHealth;

        protected void InitUnit()
        {
            _curHealth = maxHealth;
        }

        public void ApplyDamage(int damage)
        {
            _curHealth -= damage;
            if (_curHealth > 0) return;
            _curHealth = 0;
            Die();
        }

        protected virtual void Die()
        {
            Debug.Log("Enemy Miss~");
        }
    }
}