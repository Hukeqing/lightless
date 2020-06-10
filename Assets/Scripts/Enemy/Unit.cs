using UnityEngine;

namespace Enemy
{
    public class Unit : MonoBehaviour
    {
        public int maxHealth;
        public AudioClip dieClip;

        public bool IsDie => curHealth <= 0;

        protected int curHealth;
        protected AudioSource audioSource;

        protected void InitUnit()
        {
            curHealth = maxHealth;
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void ApplyDamage(int getDamage)
        {
            if (curHealth <= 0) return;
            curHealth -= getDamage;
            audioSource.Play();
            if (curHealth > 0) return;
            curHealth = 0;
            Die();
        }

        protected virtual void Die()
        {
            audioSource.clip = dieClip;
            audioSource.Play();
        }
    }
}