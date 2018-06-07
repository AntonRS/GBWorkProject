using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeekBrains
{
    public abstract class BaseAmmunition : MonoBehaviour
    {
        [SerializeField] protected Bot target;
        [SerializeField] protected float speed;
        [SerializeField] protected float damage;
        protected AttackType attackType;

        #region GetSet
        public Bot Target
        {
            get { return target; }
            set { target = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public AttackType AttackType
        {
            get { return attackType; }
            set { attackType = value; }
        }
        #endregion
        #region unity Functions
        protected virtual void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            MoveToTarget();
        }
        #endregion
        #region BaseAmmunition Functions
        protected virtual void MoveToTarget()
        {
            Vector3 dir = target.botTransform.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        protected virtual void HitTarget()
        {

            SetDamage(target);

            Destroy(gameObject);
        }
        protected virtual void SetDamage(ISetDamage obj)
        {
            if (obj != null)
            {
                obj.ApplyDamage(damage, attackType);
            }
        }
        #endregion

    }
}