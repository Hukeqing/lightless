﻿using System;
using UnityEngine;

namespace Enemy
{
    public enum EnemyState
    {
        Stand,
        Move,
        Attack
    }

    public class Enemy : Unit
    {
        public Transform player;
        public float moveSpeed;
        public float rotateSpeed;
        public float viewRange;
        public float attackRange;
        public float attackCoolDown;
        public EnemyState EnemyState { private set; get; }

        private float _nextAttack;

        protected void Init()
        {
            EnemyState = EnemyState.Stand;
            _nextAttack = 0;
            InitUnit();
        }

        private void Update()
        {
            EnemyState = EnemyState.Stand;
            var target = (player.position - transform.position).normalized;
            if (!(Vector3.Dot(target, transform.forward) >= 0.866f)) return;
            var ray = new Ray(transform.position, target);
            if (!Physics.Raycast(ray, out var hitInfo, viewRange)) return;
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
                if (_nextAttack < Time.time)
                {
                    _nextAttack = Time.time + attackCoolDown;
                    Attack();
                }
            }
        }

        protected virtual void Attack()
        {
            Debug.Log("Enemy Miss~");
        }
    }
}