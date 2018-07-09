﻿using UnityEngine.AI;
using UnityEngine;
using Game.Towers;
namespace Game.Enemy
{

    /// <summary>
    /// Содержит логику и параметры абстрактного врага.
    /// Требует наличия NavMeshAgent на обьекте.
    /// При появлении на сцене враг вносит себя в общий список врагов, содержащийся в объекте EnemiesController.
    /// При смерти удаляет себя из общего списка врагов.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemy : MonoBehaviour, ISetDamage
    {
        
        /// <summary>
        /// Здоровье врага.
        /// </summary>
        public float Hp;
        /// <summary>
        /// Сопротивление урону от лазера. Рассчитывается в процентах.
        /// </summary>
        [Range(1, 99)]
        public float LazerArmor;
        /// <summary>
        /// Сопротивление физическому урону. рассчитывается в процентах.
        /// </summary>
        [Range(1, 99)]
        public float PhysicalArmor;
        /// <summary>
        /// Скорость врага.
        /// </summary>
        [HideInInspector] public float Speed;
        /// <summary>
        /// Тип врага
        /// </summary>
        public EnemyType EnemyType;
        /// <summary>
        /// Transform на сцене, к которому движется враг.
        /// </summary>
        [HideInInspector] public Transform Destination;



        protected NavMeshAgent _agent;
        protected Animator _animator;

        #region BaseEnemy Functions
        /// <summary>
        /// Реализация интерфейса, отвечающего за получение урона.
        /// </summary>
        /// <param name="damageInfo">Параметры урона</param>
        public void ApplyDamage(DamageInfo damageInfo)
        {
            
            if (Hp > 0)
            {
                if (damageInfo.AttackType == AttackType.lazer)
                {
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * LazerArmor));
                }
                if (damageInfo.AttackType == AttackType.rocket)
                {
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * PhysicalArmor));
                }
                if (damageInfo.AttackType == AttackType.bullets)
                {
                    //
                }
            }
            else
            {
                Dead();
            }
            
        }
        protected virtual void Dead()
        {
            _agent.speed = 0;
            Hp = 0;
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
            GameManager.Instance.GetEnemiesController.DeleteEnemy(this);
            Destroy(gameObject, 6);
        }
        #endregion
        #region Unity Functions
        protected virtual void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _agent.speed = Speed;
            GameManager.Instance.GetEnemiesController.AddEnemy(this);
            if (Destination == null)
            {
                return;
            }
            _agent.SetDestination(Destination.position);
        }
        
        #endregion
    }
}

