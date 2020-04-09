using UnityEngine;

namespace Enemy
{
    public class Unit : MonoBehaviour
    {
        public int maxHealth;
        public bool IsDie { get; private set; }

        private int _curHealth;

        // TODO remove the following code
        public GameObject score;

        private void Start()
        {
            _curHealth = maxHealth;
            IsDie = false;
        }

        public void ApplyDamage(int damage)
        {
            _curHealth -= damage;
            if (_curHealth > 0) return;
            _curHealth = 0;
            SendMessage("Die");
        }

        private void Die()
        {
            // TODO remove the following code
            var transform1 = transform;
            Instantiate(score, transform1.position, transform1.rotation);
            Destroy(gameObject);
        }
    }
}