using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    public class Wizard : Enemy
    {
        private PlayerControl _pc;
        private Animator _wizardAnimator;

        public ParticleSystem hitEffect;
        public Transform firePos;
        public GameObject attackMagic;

        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private static readonly int OnHit = Animator.StringToHash("OnHit");

        private void Start()
        {
            Init();
            _pc = player.GetComponent<PlayerControl>();
            _wizardAnimator = GetComponent<Animator>();
        }

        protected override void Attack()
        {
            var newMagic = Instantiate(attackMagic, firePos.position, firePos.rotation);
            newMagic.GetComponent<Effect.WizardAttack>().pc = _pc;
        }

        protected override void Die()
        {
            StartCoroutine(Dead());
        }

        private IEnumerator Dead()
        {
            _wizardAnimator.SetTrigger(DieTrigger);
            yield return new WaitForSeconds(4.65f);
            Destroy(gameObject);
        }

        public override void ApplyDamage(int getDamage)
        {
            if (curHealth <= 0) return;
            curHealth -= getDamage;
            _wizardAnimator.SetTrigger(OnHit);
            hitEffect.Stop();
            hitEffect.Play();
            if (curHealth > 0) return;
            curHealth = 0;
            Die();
        }
    }
}