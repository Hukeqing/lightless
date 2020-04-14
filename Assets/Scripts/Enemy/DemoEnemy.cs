using Player;

namespace Enemy
{
    public class DemoEnemy : Enemy
    {
        private PlayerControl pc;
        public int damage;

        private void Start()
        {
            Init();
            pc = player.GetComponent<PlayerControl>();
        }

        protected override void Attack()
        {
            pc.ApplyDamage(damage);
        }
    }
}