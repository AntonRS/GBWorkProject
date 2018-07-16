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
        [SerializeField] protected int _cost;
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
        protected List<BaseEnemy> _targets;



        private Nullable<float> _fakeRange = null;
        public bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton)
        {
            return _lvl < _maxLvl;
            
        }
        public void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {
                
                viaButton.gameObject.SetActive(true);
                _fakeRange = GameManager.Instance.GetTowersManager.rocketTowers[_lvl + 1].AttackRange;
                Debug.Log(_fakeRange);
            }
        }
        public void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            Debug.Log("End");
            this._fakeRange = null;
            //viaButton.Meta = null;
        }
        public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {
                _fakeRange = null;
                UpgradeTower();
                Destroy(gameObject);
            }
        }
        public float OnRangeRequested()
        {
            return this._fakeRange ?? this.AttackRange;
        }


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
         
        protected void FindEnemiesInRange(Vector3 startpoint, float range)
        {
            
            _targets.Clear();
            foreach (BaseEnemy enemy in GameManager.Instance.GetEnemiesController.enemies)
            {
                if (enemy != null && _canAttack.Contains(enemy.EnemyType))
                {
                    float distanceToEnemy = Vector3.Distance(startpoint, enemy.transform.position);
                    if (distanceToEnemy < range)
                    {
                        _targets.Add(enemy);
                        
                    }
                }
            }
            
        }
        protected void FindClosestToDestinationEnemy()
        {
            
            if (_targets.Count > 0)
            {
                float shortestDistance = Mathf.Infinity;
                BaseEnemy nearestEnemy = null;

                foreach (BaseEnemy enemy in _targets)
                {
                    {
                        
                        float distanceToDestination = enemy.Agent.remainingDistance;
                        
                        if (distanceToDestination < shortestDistance)
                        {
                            shortestDistance = distanceToDestination;
                            nearestEnemy = enemy;
                            _target = nearestEnemy;
                        }
                    }
                }
            }
            
        }
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
        protected delegate void Action();
        protected abstract void Fire();
        protected abstract void LookAtTarget();
        public abstract void UpgradeTower();
        /// <summary>
        /// Инициализация начальных параметров башни.
        /// </summary>
        protected virtual void SetAwakeParams()
        {
            _targets = new List<BaseEnemy>();
            _damageInfo.Damage = _damage;
            _damageInfo.AttackType = _attackType;
            _damageInfo.AttackingTower = this;
            _maxLvl = GameManager.Instance.GetTowersManager.rocketTowers.Length - 1;
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

