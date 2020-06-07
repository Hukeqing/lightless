using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public enum EnemyState
    {
        Stand,
        Move,
        FreeMove,
        LookRound,
        Attack,
        Death
    }

    public class Enemy : Unit
    {
        public Transform player;
        public float findPlayerMoveSpeed;
        public float freeMoveSpeed;
        public float findPlayerRotateSpeed;
        public float freeRotateSpeed;

        [Range(1, 5)] public float minNextMoveCoolDown;
        [Range(5, 10)] public float maxNextMoveCoolDown;
        public float minMoveDist;

        public float viewRange;
        public float attackRange;
        public float attackCoolDown;
        public LayerMask lookObstacle;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public EnemyState EnemyState { private set; get; }

        private float _nextAttack;
        private float _nextMove;
        private Vector3 _curTarget;
        private float _rotateValue;
        private float _rotateL;
        private float _rotateR;
        private int _rotateDir;

        protected void Init()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("GameManager").GetComponent<Room.RoomManager>().player;
            }

            EnemyState = EnemyState.Stand;
            _nextAttack = 0;
            _nextMove = Time.time + Random.Range(1, 5.0f);
            InitUnit();
        }

        private void Update()
        {
            if (IsDie)
            {
                EnemyState = EnemyState.Death;
                return;
            }

            var myPosition = transform.position;
            var target = (player.position - myPosition).normalized;
            target.y = 0;
            var ray = new Ray(myPosition, target);
            if (Vector3.Dot(target, transform.forward) >= 0.866f &&
                Physics.Raycast(ray, out var hitInfo, viewRange, lookObstacle) &&
                hitInfo.collider.gameObject.layer == 10)
            {
                //  看到玩家
#if UNITY_EDITOR
                {
                    var position = transform.position;
                    var tmp = Vector3.Distance(position, player.position);
                    Debug.DrawRay(position, target * tmp,
                        Color.Lerp(Color.red, Color.green, Mathf.Clamp01((tmp - viewRange) / viewRange)));
                }
#endif
                _curTarget = player.position;

                EnemyState = Vector3.Distance(_curTarget, transform.position) < attackRange
                    ? EnemyState.Attack
                    : EnemyState.Move;
            }
            else if (EnemyState == EnemyState.Attack || EnemyState == EnemyState.Move)
            {
                EnemyState = EnemyState.Stand;
                _nextMove = Time.time + Random.Range(minNextMoveCoolDown, maxNextMoveCoolDown);
            }

            // 没有看到玩家
            Quaternion rotation;
            var dist = _curTarget != null ? Vector3.Distance(_curTarget, transform.position) : 0;
            switch (EnemyState)
            {
                case EnemyState.Stand:
                    if (_nextMove <= Time.time)
                    {
                        for (var i = 0; i < 30; ++i)
                        {
                            var dir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                            dir.Normalize();
                            ray.origin = myPosition;
                            ray.direction = dir;
                            if (!Physics.Raycast(ray, out hitInfo, 1000, lookObstacle)) continue;
                            if (!(Vector3.Distance(hitInfo.point, myPosition) * 0.9f > minMoveDist)) continue;
                            _curTarget = Vector3.Lerp(myPosition + dir * minMoveDist, hitInfo.point,
                                Random.Range(0, 0.9f));
                            EnemyState = EnemyState.FreeMove;
                            break;
                        }

                        Debug.Log("Find Fail");
                        _nextMove = Time.time + Random.Range(minNextMoveCoolDown, maxNextMoveCoolDown);
                    }

                    break;
                case EnemyState.Move:
                    rotation = Quaternion.LookRotation(player.position - transform.position);
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, rotation, findPlayerRotateSpeed * Time.deltaTime);
                    transform.Translate(target * (findPlayerMoveSpeed * Time.deltaTime), Space.World);
                    break;
                case EnemyState.FreeMove:
                    rotation = Quaternion.LookRotation(_curTarget - myPosition);
                    Transform transform1;
                    (transform1 = transform).rotation =
                        Quaternion.Slerp(transform.rotation, rotation, findPlayerRotateSpeed * Time.deltaTime);
#if UNITY_EDITOR
                    Debug.DrawRay(myPosition, _curTarget - myPosition, Color.blue);
#endif
                    transform.Translate(transform1.forward * (Time.deltaTime * freeMoveSpeed), Space.World);
                    if (dist < 0.1)
                    {
                        EnemyState = EnemyState.LookRound;
                        _rotateValue = 0;
                        _rotateL = Random.Range(30, 180);
                        _rotateR = Random.Range(30, 180);
                        _rotateDir = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;
                    }

                    break;
                case EnemyState.LookRound:
                    transform.Rotate(Vector3.up * (freeRotateSpeed * Time.deltaTime *
                                                   (_rotateValue >= _rotateL ? _rotateDir : -_rotateDir)));
                    _rotateValue += freeRotateSpeed * Time.deltaTime;
                    if (_rotateValue >= _rotateL + _rotateR)
                    {
                        _nextMove = Time.time + Random.Range(minNextMoveCoolDown, maxNextMoveCoolDown);
                        EnemyState = EnemyState.Stand;
                    }

                    break;
                case EnemyState.Attack:
                    rotation = Quaternion.LookRotation(player.position - transform.position);
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, rotation, findPlayerRotateSpeed * Time.deltaTime);
                    if (Time.time >= _nextAttack)
                    {
                        _nextAttack = Time.time + attackCoolDown;
                        Attack();
                    }

                    break;
                case EnemyState.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void Attack()
        {
            Debug.Log("Enemy Miss~");
        }
    }
}