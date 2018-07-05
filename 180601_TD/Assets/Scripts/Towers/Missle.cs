using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public class Missle : BaseAmmunition
    {
        [SerializeField] private string _hitImpactPath;
        public float Speed;
        public DamageInfo DamageInfo;
        
        protected GameObject hitImpact;

        private void Awake()
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
        private void LoadResources()
        {
            if (_hitImpactPath == null)
            {
                Debug.Log("Ammunition " + name + " Cant load resources. Enter resources path");
                return;
            }
            hitImpact = Resources.Load<GameObject>(_hitImpactPath);
        }
        private void MoveToTarget()
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
        private  void HitTarget()
        {

            SetDamage(Target);
            Destroy(gameObject);
            if (_hitImpactPath == null || hitImpact == null)
            {
                Debug.Log("Ammunition " + name + " cant find impact GameObject");
                return;
            }
            var tempImpact = Instantiate(hitImpact, transform.position, transform.rotation);
            Destroy(tempImpact, 2f);
        }
        private  void SetDamage(ISetDamage obj)
        {
            if (obj != null)
            {
                obj.ApplyDamage(DamageInfo);
            }
        }
    }
}

