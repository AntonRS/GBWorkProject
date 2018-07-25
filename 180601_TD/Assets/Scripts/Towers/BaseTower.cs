using UnityEngine;
using System.Collections.Generic;
using System;
using Game.Enemy;
using Game.CommandUI;
namespace Game.Towers
{
    /// <summary>
    /// Содержит логику и параметры абстрактной башни.
    /// </summary>
    public abstract class BaseTower : MonoBehaviour, IRangeMarkerAssignee, ICommandButtonActuator
    {
        /// <summary>
        /// Текущий уровень башни.
        /// </summary>
        [SerializeField] protected int _lvl;
        /// <summary>
        /// Максимальный уровень прокачки башни.
        /// </summary>
        protected int _maxLvl;
        /// <summary>
        /// Стоимость строительства/апдейта башни.
        /// </summary>
        public int Cost;
        /// <summary>
        /// Цена продажи башни.
        /// </summary>
        public int SellCost;
        /// <summary>
        /// Урон. Зависит от типа _attackType.
        /// </summary>
        [SerializeField] protected int _damage;
        /// <summary>
        /// Радиус атаки.
        /// </summary>
        public float AttackRange;
        /// <summary>
        /// Какой тип врагов может атаковать башня.
        /// </summary>
        [SerializeField] protected List <EnemyType> _canAttack;      
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
        [HideInInspector] protected BaseEnemy _target;
        /// <summary>
        /// Информация об уроне.
        /// </summary>
        protected DamageInfo _damageInfo;
        /// <summary>
        /// Переодичность обновления списка целей.
        /// </summary>
        private const float searchingTime = 0.5f;
        /// <summary>
        /// Список врагов в радиусе поражения/
        /// </summary>
        protected List<BaseEnemy> _targetsInRange;
        /// <summary>
        /// Фейковый радиус. используется для отображения радиуса после апгрейда/строительства/
        /// </summary>
        protected Nullable<float> _fakeRange = null;
        /// <summary>
        /// Возвращает стоимость апгрейда.
        /// </summary>
        /// <returns></returns>
        public abstract int? GetUpgradeCost();
        /// <summary>
        /// Делегат Action.
        /// </summary>
        protected delegate void Action();
        /// <summary>
        /// Абстракция ведения огня.
        /// </summary>
        protected abstract void Fire();
        /// <summary>
        /// Абстракция наблюдения за целью.
        /// </summary>
        protected abstract void LookAtTarget();
        /// <summary>
        /// Абстракция апгрейда башни.
        /// </summary>
        public abstract void UpgradeTower();
        /// <summary>
        /// Ссылка на TowersManager
        /// </summary>
        protected TowersManager _towersManager;
        /// <summary>
        /// Ссылка на TerrainGeneratorController
        /// </summary>
        protected TerrainGeneratorController _terrainGenerator;
        /// <summary>
        /// Ссылка на EnemiesController
        /// </summary>
        protected EnemiesController _enemiesController;

        #region UI Interface
        public bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton)
        {
            return _lvl <= _maxLvl;

        }
        public abstract void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton);

        public void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {

            this._fakeRange = null;

        }
        public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {
                _fakeRange = null;
                UpgradeTower();

            }
            if (ofType == CommandType.Sell)
            {
                _towersManager.SellTower(transform, SellCost);
                Destroy(gameObject);
            }
        }
        public float OnRangeRequested()
        {
            return this._fakeRange ?? this.AttackRange;
        }
        #endregion
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

        /// <summary>
        /// Находит врагов в радиусе поражения и заполняет ими список _targetsInRange.
        /// </summary>
        /// <param name="startpoint">Список врагов в радиусе поражения</param>
        /// <param name="range">радиус поражения</param>
        protected void FindEnemiesInRange(Vector3 startpoint, float range)
        {
            
            _targetsInRange.Clear();
            
            foreach (BaseEnemy enemy in _enemiesController.Enemies)
            {
                if (enemy != null && _canAttack.Contains(enemy.EnemyType))
                {
                    float distanceToEnemy = Vector3.Distance(startpoint, enemy.transform.position);
                    if (distanceToEnemy < range)
                    {
                        _targetsInRange.Add(enemy);
                        
                    }
                }
            }
            
        }
        /// <summary>
        /// Присваивает полю _target ближайшего к финишу врага.
        /// </summary>
        protected void FindClosestToDestinationEnemy()
        {            
            if (_targetsInRange.Count > 0)
            {                
                float shortestDistance = Mathf.Infinity;
                BaseEnemy nearestEnemy = null;
                foreach (BaseEnemy enemy in _targetsInRange)
                {
                    {
                        float distanceToDestination = enemy.GetDistance();                        
                        if (distanceToDestination < shortestDistance)
                        {
                            shortestDistance = distanceToDestination;
                            nearestEnemy = enemy;
                            _target = nearestEnemy;                        
                        }
                    }
                }
            }
            else
            {
                _target = null;
            }            
        }
        /// <summary>
        /// Обновляет значение _target.
        /// </summary>
        protected virtual void UpdateTarget()
        {
            FindEnemiesInRange(transform.position, AttackRange);
            FindClosestToDestinationEnemy();
            
        }
        /// <summary>
        /// Обертка для метода InvokeRepeating.
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="startTime">время первого запуска</param>
        /// <param name="repeatTime">переодичность</param>
        protected virtual void InvokeRepeating(Action action, float startTime, float repeatTime)
        {
            InvokeRepeating(action.Method.Name, startTime, repeatTime);
        }        
        /// <summary>
        /// Инициализация начальных параметров башни.
        /// </summary>
        protected virtual void SetAwakeParams()
        {
            _targetsInRange = new List<BaseEnemy>();
            _damageInfo.Damage = _damage;
            _damageInfo.AttackType = _attackType;
            _damageInfo.AttackingTower = this;
            _enemiesController = GameManager.Instance.GetEnemiesController;
            _terrainGenerator = GameManager.Instance.GetTerrainGenerator;
            _towersManager = GameManager.Instance.GetTowersManager;
            
        }
        #endregion
        #region Editor Functions
        /// <summary>
        /// Отображает радиус атаки во время работы в редакторе.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
        #endregion
    }
}

