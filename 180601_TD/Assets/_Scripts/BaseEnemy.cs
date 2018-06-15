using UnityEngine.AI;
using UnityEngine;
namespace GeekBrains
{
    public abstract class BaseEnemy : MonoBehaviour, ISetDamage
    {
        public Transform Destination;
        public float Hp;
        [Range(1, 90)]
        public float PhysicalArmor;
        [Range(1, 90)]
        public float MagicArmor;
        public float Speed;
        public bool IsFlying;
        [HideInInspector] public Transform EnemyTransform;

        protected NavMeshAgent _agent;
        protected Animator _animator;

        #region BaseEnemy Functions
        public void ApplyDamage(DamageInfo damageInfo)
        {
            
            if (Hp > 0)
            {
                if (damageInfo.AttackType == AttackType.physical)
                {
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * PhysicalArmor));
                }
                if (damageInfo.AttackType == AttackType.magic)
                {
                    Hp -= (damageInfo.Damage - ((damageInfo.Damage / 100) * MagicArmor));
                }
                if (damageInfo.AttackType == AttackType.pure)
                {
                    Hp -= damageInfo.Damage;
                }

            }
            if (Hp <= 0)
            {
                Hp = 0;
                Dead();
            }
        }
        #endregion
        #region Unity Functions
        protected virtual void Start()
        {
            
            EnemyTransform = GetComponent<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _agent.speed = Speed;
            Main.Instance.AddEnemy(this);
            if (Destination == null)
            {
                Debug.Log("Enemy " + name + " dont have target. Add destination target");
                return;
            }
            _agent.SetDestination(Destination.position);
        }
        protected virtual void Dead()
        {
            _agent.speed = 0;
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
            
            Main.Instance.DeleteEnemy(this);
            Destroy(gameObject,6);
        }
        #endregion
    }
}

