using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public abstract class BaseAmmunition : MonoBehaviour
    {

        [SerializeField] protected string _hitImpactPath;
        public float Speed;
        public DamageInfo DamageInfo;
        public BaseEnemy Target;
        protected GameObject hitImpact;


        #region unity Functions
        protected virtual void Awake()
        {
            LoadResources();
            
        }
        protected virtual void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }
            MoveToTarget();
        }
        #endregion
        #region BaseAmmunition Functions
        protected virtual void LoadResources()
        {
            if (_hitImpactPath == null)
            {
                Debug.Log("Ammunition " + name + " Cant load resources. Enter resources path");
                return;
            }
            hitImpact = Resources.Load<GameObject>(_hitImpactPath);
        }
        protected virtual void MoveToTarget()
        {
            Vector3 dir = Target.EnemyTransform.position - transform.position;
            float distanceThisFrame = Speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        protected virtual void HitTarget()
        {

            SetDamage(Target);
            Destroy(gameObject);
        }
        protected virtual void SetDamage(ISetDamage obj)
        {
            if (obj != null)
            {
                obj.ApplyDamage(DamageInfo);
            }
        }
        
        #endregion

    }
}