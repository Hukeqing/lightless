using Player;

namespace Enemy
{
    public class Mummy : Enemy
    {
        private PlayerControl _pc;
        public int damage;


        private void Start()
        {
            Init();
            _pc = player.GetComponent<PlayerControl>();
        }

        protected override void Attack()
        {
            _pc.ApplyDamage(damage);
        }

        protected override void Die()
        {
            audioSource.clip = dieClip;
            audioSource.Play();
            Destroy(gameObject, 1);
        }
    }
}