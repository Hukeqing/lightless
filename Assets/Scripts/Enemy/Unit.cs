using UnityEngine;

namespace Enemy
{
    public class Unit : MonoBehaviour
    {
        public int maxHealth;

        public bool IsDie => _curHealth <= 0;

        private int _curHealth;

        // TODO remove the following code
        public GameObject score;

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
            var transform1 = transform;
            Instantiate(score, transform1.position, transform1.rotation);
            Destroy(gameObject);
        }
    }
}