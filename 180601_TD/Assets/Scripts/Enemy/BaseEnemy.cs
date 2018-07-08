using UnityEngine.AI;
using UnityEngine;
using Game.Towers;
namespace Game.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, ISetDamage
    {
        public Transform Destination;
        public float Hp;
        [Range(1, 90)]
        public float LazerArmor;
        [Range(1, 90)]
        public float PhysicalArmor;
        public float Speed;
        public bool IsFlying;
        [HideInInspector] public Transform EnemyTransform;
        public Transform explosionTransform;

        protected NavMeshAgent _agent;
        protected Animator _animator;
        protected Rigidbody rb;

        #region BaseEnemy Functions
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
            rb.isKinematic = true;
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
            rb = GetComponent<Rigidbody>();
            EnemyTransform = GetComponent<Transform>();
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

