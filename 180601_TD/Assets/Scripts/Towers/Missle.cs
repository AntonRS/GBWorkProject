using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    /// <summary>
    /// Содержит логику и параметры самонаводящейся ракеты.
    /// </summary>
    public class Missle : MonoBehaviour
    {
        /// <summary>
        /// Скорость ракеты.
        /// </summary>
        [HideInInspector] public float Speed;
        /// <summary>
        /// Информация об уроне.
        /// </summary>
        [HideInInspector] public DamageInfo DamageInfo;
        /// <summary>
        /// Цель типа BaseEnemy.
        /// </summary>
        [HideInInspector] public BaseEnemy Target;
        /// <summary>
        /// Префаб эффекта взрыва.
        /// </summary>
        [SerializeField] private GameObject _hitImpact;

        
        /// <summary>
        /// Transform, на который выходит ракета для выхода на цель.
        /// </summary>
        [HideInInspector] public Transform attackPosition;
        
        private bool _onAttackPosition = false;
        /// <summary>
        /// скорость движения до _AttackPosition;
        /// </summary>
        private float _moveToAttackPosSpeed = 3;
        private float _explosionRadius = 5;
        private float _explosionFore = 1000;
        [SerializeField] private Transform _explosionTransform;

        protected virtual void Update()
        {
            if (Target)
            {
                MoveToAttackPosition();
                MoveToEnemyAndHit();
            }
            else
            {
                Destroy(gameObject);
                return;
            }            
        }

        /// <summary>
        /// Перемещает ракету на позицию для атаки со скоростью _moveToAttackPosSpeed;
        /// Придостижении позиции изменяет _onAttackPosition на true;
        /// </summary>
        private void MoveToAttackPosition()
        {
            if (!_onAttackPosition)
            {
                Vector3 dir = attackPosition.position - transform.position;
                float distanceThisFrame = _moveToAttackPosSpeed * Time.deltaTime;
                if (dir.magnitude <= distanceThisFrame)
                {
                    _onAttackPosition = true;
                    return;
                }
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            }    
        }
        /// <summary>
        /// Перемещает ракету к цели.
        /// При достижении цели вызывает метод HitTarget.
        /// </summary>
        private void MoveToEnemyAndHit()
        {
            if (_onAttackPosition)
            {
                Vector3 dir = Target.transform.position - transform.position;
                float distanceThisFrame = Speed * Time.deltaTime;
                if (dir.magnitude <= distanceThisFrame)
                {
                    HitTarget();
                    
                    return;
                }
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            }
        }
        /// <summary>
        /// Наносит урон, вызывает эффект попадания, уничтожает эффект попадания, уничтожает ракету.
        /// </summary>
        private void HitTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (Collider nearbyObject in colliders)
            {
                BaseEnemy enemy = nearbyObject.GetComponent<BaseEnemy>();
                SetDamage(enemy);
            }
            Destroy(gameObject);
            if (_hitImpact != null)
            {
                var tempImpact = Instantiate(_hitImpact, transform.position, transform.rotation);
                Destroy(tempImpact, 2f);
            }
        }
        /// <summary>
        /// Передает параметры урона обьекту, реалезующему интерфейс ISetDamage
        /// </summary>
        /// <param name="obj"></param>
        private  void SetDamage(ISetDamage obj)
        {
            if (obj != null)
            {
                obj.ApplyDamage(DamageInfo);
            }
        }
    }
}

