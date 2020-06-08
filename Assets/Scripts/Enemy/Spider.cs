using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    public class Spider : Enemy
    {
        private PlayerControl _pc;
        private Animator _spiderAnimator;
        private bool _useLeft;

        public int damage;
        private static readonly int UseLeft = Animator.StringToHash("useLeft");
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private static readonly int OnHit = Animator.StringToHash("OnHit");

        private void Start()
        {
            Init();
            _useLeft = true;
            _pc = player.GetComponent<PlayerControl>();
            _spiderAnimator = GetComponent<Animator>();
        }

        protected override void Attack()
        {
            _pc.ApplyDamage(damage);
            _useLeft = !_useLeft;
            _spiderAnimator.SetBool(UseLeft, _useLeft);
        }

        protected override void Die()
        {
            StartCoroutine(Dead());
        }

        private IEnumerator Dead()
        {
            _spiderAnimator.SetTrigger(DieTrigger);
            yield return new WaitForSeconds(2.25f);
            Destroy(gameObject);
        }

        public override void ApplyDamage(int getDamage)
        {
            if (curHealth <= 0) return;
            curHealth -= getDamage;
            _spiderAnimator.SetTrigger(OnHit);
            if (curHealth > 0) return;
            curHealth = 0;
            Die();
        }
    }
}