using UnityEngine.AI;
using UnityEngine;
using Game.Towers;
namespace Game.Enemy
{
    /// <summary>
    /// Содержит логику и параметры абстрактного врага.
    /// Требует наличия NavMeshAgent и Rigidbody на обьекте.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseEnemy : MonoBehaviour, ISetDamage
    {        
        /// <summary>
        /// Здоровье врага.
        /// </summary>
        public float Hp;
        /// <summary>
        /// Сопротивление урону от лазера. Рассчитывается в процентах.
        /// </summary>
        [Tooltip("На сколько % снижается входящий урон от ракет и пулемета")]
        [Range(1, 99)]
        public float PowerShield;
        /// <summary>
        /// Сопротивление физическому урону. рассчитывается в процентах.
        /// </summary>
        [Tooltip("На сколько % снижается входящий урон от лазера")]
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
        /// <summary>
        /// Вознаграждение за убийство врага.
        /// </summary>
        [Tooltip("Вознаграждение за убийство врага.")]
        public int Cost;
        /// <summary>
        /// Урон, который враг отнимает, доходя до финиша.
        /// </summary>
        [Tooltip("Урон, который враг отнимает, доходя до финиша.")]
        [SerializeField] protected int _damage;
        /// <summary>
        /// Наносил ли уже враг здоровье?
        /// </summary>
        protected bool _damaged = false;
        /// <summary>
        /// Ссылка на компонент NavMeshAgent.
        /// </summary>
        [HideInInspector] public NavMeshAgent Agent;
        /// <summary>
        /// Ссылка на компонент Animator.
        /// </summary>
        protected Animator _animator;
        /// <summary>
        /// Путь до финиша.
        /// </summary>
        protected NavMeshPath path;
        /// <summary>
        /// Мертв ли враг?
        /// </summary>
        protected bool _dead = false;
        /// <summary>
        /// Время, через которое обьект уничтожается.
        /// </summary>
        protected float _dyingTime = 6;
        /// <summary>
        /// Делегат Action.
        /// </summary>
        protected delegate void Action();

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
        /// <summary>
        /// Логика смерти врага.
        /// </summary>
        protected virtual void Dead()
        {
            if (!_dead)
            {
                CancelInvoke(RestoreSpeed);
                Agent.speed = 0;
                Hp = 0;
                if (_animator != null)
                {
                    _animator.SetTrigger("Dead");
                }
                GameManager.Instance.GetEnemiesController.DeleteEnemy(this);
                GameManager.Instance.UpdateMoney(Cost);
                Destroy(gameObject, _dyingTime);
                _dead = true;
            }
        }
        /// <summary>
        /// Восстанавливает начальную скорость 
        /// </summary>
        protected virtual void RestoreSpeed()
        {
            Agent.speed = Speed;
        }
        /// <summary>
        /// Отменяет Invoke
        /// </summary>
        /// <param name="action">Метод</param>
        protected virtual void CancelInvoke(Action action)
        {
            CancelInvoke(action.Method.Name);
        }
        /// <summary>
        /// Обертка для Invoke
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="startTime">Время до срабатывания</param>
        protected virtual void Invoke(Action action, float startTime)
        {
            Invoke(action.Method.Name, startTime);
        }
        /// <summary>
        /// Возвращает расстояние до финиша.
        /// </summary>
        /// <returns></returns>
        public float GetDistance()
        {
            path = Agent.path;
            float distance = .0f;
            for (var i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return distance;
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
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Finish" && !_damaged)
            {
                GameManager.Instance.UpdateLive(-_damage);
                Destroy(gameObject, 1f);
                _damaged = true;
            }
        }
        #endregion  
    }
}

