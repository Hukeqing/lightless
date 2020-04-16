using Player;
using UnityEngine;

namespace Enemy
{
    public class DemoEnemy : Enemy
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
    }
}