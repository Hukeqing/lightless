using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    public class Zombie : Enemy
    {
        private PlayerControl _pc;
        private Animator _zombieAnimator;

        public int damage;
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private static readonly int DiePos = Animator.StringToHash("DiePos");

        private void Start()
        {
            Init();
            _pc = player.GetComponent<PlayerControl>();
            _zombieAnimator = GetComponent<Animator>();
            _zombieAnimator.SetBool(DiePos, Random.Range(0, 1.0f) < 0.5f);
        }

        protected override void Attack()
        {
            _pc.ApplyDamage(damage);
        }

        protected override void Die()
        {
            audioSource.clip = dieClip;
            audioSource.Play();
            StartCoroutine(Dead());
        }

        private IEnumerator Dead()
        {
            _zombieAnimator.SetTrigger(DieTrigger);
            yield return new WaitForSeconds(1.4f);
            Destroy(gameObject);
        }

        public override void ApplyDamage(int getDamage)
        {
            if (curHealth <= 0) return;
            curHealth -= getDamage;
            audioSource.Play();
            if (curHealth > 0) return;
            curHealth = 0;
            Die();
        }
    }
}