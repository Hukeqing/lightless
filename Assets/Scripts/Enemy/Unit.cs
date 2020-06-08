using UnityEngine;

namespace Enemy
{
    public class Unit : MonoBehaviour
    {
        public int maxHealth;

        public bool IsDie => curHealth <= 0;

        protected int curHealth;

        protected void InitUnit()
        {
            curHealth = maxHealth;
        }

        public virtual void ApplyDamage(int getDamage)
        {
            if (curHealth <= 0) return;
            curHealth -= getDamage;
            if (curHealth > 0) return;
            curHealth = 0;
            Die();
        }

        protected virtual void Die()
        {
            Debug.Log("Enemy Miss~");
        }
    }
}