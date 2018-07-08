using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public abstract class BaseTower : MonoBehaviour
    {
        /// <summary>
        /// Текущий уровень башни.
        /// </summary>
        [SerializeField] protected int _lvl;
        protected int _maxLvl;
        /// <summary>
        /// Стоимость строительства/апдейта башни.
        /// </summary>
        [SerializeField] protected int _cost;
        /// <summary>
        /// Урон. Зависит от типа _attackType.
        /// </summary>
        [SerializeField] protected int _damage;
        /// <summary>
        /// Радиус атаки.
        /// </summary>
        [SerializeField] protected float _attackRange;
        [SerializeField] protected bool _isAbleToAttackGround;
        [SerializeField] protected bool _isAbleToAttackAir;
        /// <summary>
        /// Тип урона.
        /// </summary>
        [SerializeField] protected AttackType _attackType;
        /// <summary>
        /// Transform части башни, которая вращается во время отслеживания цели.
        /// </summary>
        [SerializeField] protected Transform _rotateHead;
        /// <summary>
        /// Скорость поворота элемента _rotateHead
        /// </summary>
        [SerializeField] protected float _turnSpeed;
        /// <summary>
        /// Transform, где появляется снаряд во время стрельбы.
        /// </summary>
        [SerializeField] protected Transform _firePoint;

        /// <summary>
        /// Цель типа BaseEnemy.
        /// </summary>
        [SerializeField] protected BaseEnemy _target;


        /// <summary>
        /// Информация об уроне.
        /// </summary>
        protected DamageInfo _damageInfo;

        /// <summary>
        /// Переодичность обновления списка целей.
        /// </summary>
        private const float searchingTime = 0.5f;



        #region Unity Functions
        protected virtual void Awake()
        { 
            SetAwakeParams();
        }
        protected virtual void Start()
        {
            InvokeRepeating(UpdateTarget, 0, 0.5f);
        }
        protected virtual void Update()
        {
            LookAtTarget();
            Fire();
        }
        #endregion
        #region BaseTower Functions

        protected virtual void UpdateTarget()
        {
            float shortestDistance = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;

            foreach (BaseEnemy enemy in GameManager.Instance.GetEnemiesController.enemies)
            {
                if (((_isAbleToAttackGround && !enemy.IsFlying) || (_isAbleToAttackAir && enemy.IsFlying))&& enemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }

            if (nearestEnemy != null && shortestDistance <= _attackRange)
            {
                _target = nearestEnemy;
            }
            else
            {
                _target = null;
            }
        }

        /// <summary>
        /// Обертка для метода InvokeRepeating.
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="startTime">время первого запуска</param>
        /// <param name="repeatTime">переодичность</param>
        protected virtual void InvokeRepeating(System.Action action, float startTime, float repeatTime)
        {
            InvokeRepeating(action.Method.Name, startTime, repeatTime);
        }
        
        protected abstract void Fire();
        protected abstract void LookAtTarget();
        public abstract void UpgradeTower();
        /// <summary>
        /// Инициализация начальных параметров башни.
        /// </summary>
        protected virtual void SetAwakeParams()
        {
            _damageInfo.Damage = _damage;
            _damageInfo.AttackType = _attackType;
            //_maxLvl = GameManager.Instance.GetTowersManager.rocketTowers.Length - 1;
        }
        #endregion
        #region Editor Functions
        /// <summary>
        /// Отображает радиус атаки во время работы в редакторе.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
        #endregion
        

        


    }
}

