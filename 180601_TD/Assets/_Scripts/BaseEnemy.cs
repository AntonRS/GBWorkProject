using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Game.Towers;
namespace Game.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, ISetDamage
    {
        [SerializeField] protected Transform destination;
        [SerializeField] protected float hp;
        [Range(1, 90)]
        [SerializeField]
        protected float physicalArmor;
        [Range(1, 90)]
        [SerializeField]
        protected float magicArmor;
        [SerializeField] protected float speed;
        [SerializeField] protected bool isFlying;

        protected NavMeshAgent agent;
        protected Transform enemyTransform;

        #region GetSet
        public Transform Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        public float HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public float PhysicalArmor
        {
            get { return physicalArmor; }
            set { physicalArmor = value; }
        }

        public float MagicArmor
        {
            get { return magicArmor; }
            set { magicArmor = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public bool IsFlying
        {
            get { return isFlying; }
            set { isFlying = value; }
        }
        public Transform EnemyTransform
        {
            get { return enemyTransform; }
            set { enemyTransform = value; }
        }

        #endregion

        #region BaseEnemy Functions
        public void ApplyDamage(float damage, AttackType attackType)
        {

            if (hp > 0)
            {
                if (attackType == AttackType.physical)
                {
                    hp -= (damage-((damage / 100) * physicalArmor));
                }
                if (attackType == AttackType.magic)
                {
                    hp -= (damage-((damage / 100) * magicArmor));
                }
                if (attackType == AttackType.pure)
                {
                    hp -= damage;
                }

            }
            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }
        #endregion

        #region Unity Functions
        protected virtual void Start()
        {
            
            enemyTransform = GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
            Main.Instance.AddEnemy(this);
            if (destination == null)
            {
                Debug.Log("Enemy " + name + " dont have target. Add destination target");
                return;
            }
            agent.SetDestination(destination.position);
        }
        protected virtual void Dead()
        {
            Main.Instance.DeleteEnemy(this);
            Destroy(gameObject);
        }
        #endregion
    }
}

