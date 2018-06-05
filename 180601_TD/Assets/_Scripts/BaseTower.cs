﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GeekBrains
{
    public abstract class BaseTower : MonoBehaviour
    {

        [SerializeField] protected int cost;
        [SerializeField] protected int damage;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float attackPerSecond;
        [SerializeField] protected bool isAbleToAttackGround;
        [SerializeField] protected bool isAbleToAttackAir;
        
        [SerializeField] protected string ammunitionPath;
        protected BaseAmmunition ammunition;
        protected Bot target;

        

        #region Unity Functions
        protected virtual void Awake()
        {
            LoadResources();
        }
        protected virtual void Start()
        {
            InvokeRepeating(UpdateTarget, 0f, 0.5f);
        }
        protected virtual void Update()
        {
            if (target == null)
            {
                return;
            }
            Fire();
        }
        #endregion
        #region BaseTower Functions
        protected virtual void LoadResources()
        {
            ammunition = Resources.Load<BaseAmmunition>(ammunitionPath);
        }
        protected virtual void UpdateTarget()
        {
            float shortestDistance = Mathf.Infinity;
            Bot nearestEnemy = null;
            foreach (Bot enemy in Main.Instance.enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null && shortestDistance <= attackRange)
            {
                target = nearestEnemy;
            }
            else
            {
                target = null;
            }
        }
        private void InvokeRepeating(Action action, float startTime, float repeatTime)
        {
            InvokeRepeating(action.Method.Name, startTime, repeatTime);
        }
        delegate void Action();
        public abstract void Fire();
        #endregion
        #region Editor Functions
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion
        

        


    }
}

