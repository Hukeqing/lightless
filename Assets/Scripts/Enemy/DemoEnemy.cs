using Player;
using UnityEngine;

namespace Enemy
{
    public class DemoEnemy : Enemy
    {
        private PlayerControl _pc;
        public int damage;

        // TODO remove the following code
        public GameObject score;

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
            var transform1 = transform;
            var item = Instantiate(score, transform1.position, transform1.rotation);
            item.transform.parent = transform.parent;
            Destroy(gameObject);
        }
    }
}