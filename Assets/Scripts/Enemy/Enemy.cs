using UnityEngine;

namespace Enemy
{
    public enum EnemyState
    {
        Stand,
        Move,
        Attack,
        Death
    }

    public class Enemy : Unit
    {
        public Transform player;
        public float moveSpeed;
        public float rotateSpeed;
        public float viewRange;
        public float attackRange;
        public float attackCoolDown;
        public LayerMask lookObstacle;
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public EnemyState EnemyState { private set; get; }

        private float _nextAttack;

        protected void Init()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("GameManager").GetComponent<Room.RoomManager>().player;
            } 
            EnemyState = EnemyState.Stand;
            _nextAttack = 0;
            InitUnit();
        }

        private void Update()
        {
            if (IsDie)
            {
                EnemyState = EnemyState.Death;
                return;
            }

            EnemyState = EnemyState.Stand;
            var target = (player.position - transform.position).normalized;
            target.y = 0;
            if (!(Vector3.Dot(target, transform.forward) >= 0.866f)) return;
            var ray = new Ray(transform.position, target);
            if (!Physics.Raycast(ray, out var hitInfo, viewRange, lookObstacle)) return;
            Debug.DrawRay(transform.position, target * 100, Color.blue);
            if (hitInfo.collider.gameObject.layer != 10) return;
            var rotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            if (Vector3.Distance(player.position, transform.position) > attackRange)
            {
                EnemyState = EnemyState.Move;
                transform.Translate(target * (moveSpeed * Time.deltaTime), Space.World);
            }
            else
            {
                EnemyState = EnemyState.Attack;
                if (!(_nextAttack < Time.time)) return;
                _nextAttack = Time.time + attackCoolDown;
                Attack();
            }
        }

        protected virtual void Attack()
        {
            Debug.Log("Enemy Miss~");
        }
    }
}