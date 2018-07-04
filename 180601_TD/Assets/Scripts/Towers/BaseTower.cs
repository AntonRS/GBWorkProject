using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public abstract class BaseTower : MonoBehaviour
    {
        
        [SerializeField] protected int _cost;
        [SerializeField] protected int _updateCost;
        [SerializeField] protected int _damage;
        [SerializeField] protected float _attackRange;
        [SerializeField] protected float _attackPerSecond;
        [SerializeField] protected bool _isAbleToAttackGround;
        [SerializeField] protected bool _isAbleToAttackAir;
        [SerializeField] protected AttackType _attackType;
        [SerializeField] protected string _ammunitionPath;
        
        protected BaseAmmunition _ammunition;
        protected BaseEnemy _target;
        [SerializeField]
        protected DamageInfo _damageInfo;
        protected int _lvl = 0;
        protected int _maxLvl;




        #region Unity Functions
        protected virtual void Awake()
        {
            LoadResources();
            SetDamageInfo();
        }
        protected virtual void Start()
        {
            _damageInfo.AttackType = _attackType;
            _damageInfo.Damage = _damage;
            InvokeRepeating(UpdateTarget, 0f, 0.5f);
        }
        protected virtual void Update()
        {
            if (_target == null)
            {
                return;
            }
            Fire();
        }
        #endregion
        #region BaseTower Functions
        protected virtual void LoadResources()
        {
            if (_ammunitionPath == null)
            {
                Debug.Log("Tower "+ name + " Cant load resources. Enter resources path");
                return;
            }
            _ammunition = Resources.Load<BaseAmmunition>(_ammunitionPath);
        }
        protected virtual void UpdateTarget()
        {

            float shortestDistance = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;

            foreach (BaseEnemy enemy in GameManager.Instance.GetEnemiesController.enemies)
            {
                if ((_isAbleToAttackGround && !enemy.IsFlying)||(_isAbleToAttackAir && enemy.IsFlying))
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
        private void InvokeRepeating(Action action, float startTime, float repeatTime)
        {
            InvokeRepeating(action.Method.Name, startTime, repeatTime);
        }
        delegate void Action();
        public abstract void Fire();
        public abstract void UpdateTower();

        protected virtual void SetDamageInfo()
        {
            _damageInfo.Damage = _damage;
            _damageInfo.AttackType = _attackType;
        }
       
        #endregion
        #region Editor Functions
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
        #endregion
        

        


    }
}

