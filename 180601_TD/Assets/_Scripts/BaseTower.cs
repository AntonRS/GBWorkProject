using System.Collections;
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
        [SerializeField] protected AttackType attackType;
        [SerializeField] protected string ammunitionPath;
        protected BaseAmmunition ammunition;
        protected BaseEnemy target;
        [SerializeField]
        protected DamageInfo damageInfo;


        

        #region Unity Functions
        protected virtual void Awake()
        {
            LoadResources();
            SetDamageInfo();
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
            if (ammunitionPath == null)
            {
                Debug.Log("Tower "+ name + " Cant load resources. Enter resources path");
                return;
            }
            ammunition = Resources.Load<BaseAmmunition>(ammunitionPath);
        }
        protected virtual void UpdateTarget()
        {

            float shortestDistance = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;

            foreach (BaseEnemy enemy in Main.Instance.enemies)
            {
                if ((isAbleToAttackGround && !enemy.IsFlying)||(isAbleToAttackAir && enemy.IsFlying))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
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

        protected virtual void SetDamageInfo()
        {
            damageInfo.Damage = damage;
            damageInfo.AttackType = attackType;
        }
        protected virtual void UpdateTower()
        {

        }
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

