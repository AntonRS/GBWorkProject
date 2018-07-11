using UnityEngine.AI;
using UnityEngine;
using Game.Towers;
using System.Collections.Generic;
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
        public float PowerShield;
        /// <summary>
        /// Сопротивление физическому урону. рассчитывается в процентах.
        /// </summary>
        [Range(1, 99)]
        public float PhysicalArmor;
        /// <summary>
        /// Скорость врага.
        /// </summary>
        public float Speed;
        /// <summary>
        /// Тип врага
        /// </summary>
        public EnemyType EnemyType;
        /// <summary>
        /// Transform на сцене, к которому движется враг.
        /// </summary>
        [HideInInspector] public Transform Destination;


        [HideInInspector] public List<BaseTower> _attackingTowers;

        [HideInInspector] public NavMeshAgent Agent;
        protected Animator _animator;
        
        protected bool _isAttacked;
        /// <summary>
        /// Время, через которое обьект уничтожается.
        /// </summary>
        protected float _dyingTime = 6;
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
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * PowerShield));
                }
                if (damageInfo.AttackType == AttackType.rocket)
                {
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * PhysicalArmor));
                }
                if (damageInfo.AttackType == AttackType.bullets)
                {
                    CancelInvoke(RestoreSpeed);
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * PhysicalArmor));
                    Agent.speed = (Speed - ((Speed / 100) * damageInfo.SpeedReduction));
                    Invoke(RestoreSpeed, 0.1f); 
                }
            }
            else
            {
                Dead();
            }
            
        }
        protected virtual void Dead()
        {
            Agent.speed = 0;
            Hp = 0;
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
            GameManager.Instance.GetEnemiesController.DeleteEnemy(this);
            Destroy(gameObject, _dyingTime);
        }
        #endregion
        #region Unity Functions
        protected virtual void Start()
        {
            Agent = GetComponent<NavMeshAgent>();            
            _animator = GetComponentInChildren<Animator>();
            Agent.speed = Speed;

            GameManager.Instance.GetEnemiesController.AddEnemy(this);
            if (Destination == null)
            {
                return;
            }
            Agent.SetDestination(Destination.position);
        }
        protected virtual void RestoreSpeed()
        {
            Agent.speed = Speed;
        }
        protected virtual void CancelInvoke(Action action)
        {
            
            CancelInvoke(action.Method.Name);
        }
        protected virtual void Invoke(Action action, float startTime)
        {
            Invoke(action.Method.Name, startTime);
        }
        protected delegate void Action();
        
        #endregion
    }
}

