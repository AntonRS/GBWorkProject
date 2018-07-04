using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public class LazerTower : BaseTower
    {

        /// <summary>
        /// Transform of object which should look on to an enemy
        /// </summary>
        [SerializeField] private Transform _rotateHead;
        /// <summary>
        /// Transform where Ammunition appears
        /// </summary>
        [SerializeField] private Transform _firePoint;
        /// <summary>
        /// _rotateHead rotation speed
        /// </summary>
        [SerializeField] private float _turnSpeed = 5;
        /// <summary>
        /// Ammuniton speed
        /// </summary>
        [SerializeField] private float _ammunitionSpeed;
        
        private float _lazerStartFire;
        private float _lazerFinishFire = 0.10f;
        private BaseEnemy[] nearEnemies;
        private float fireCountDown = 0f;
        private RaycastHit _hit;
        private LineRenderer _lazer;
        protected override void Start()
        {
            base.Start();
            


            _lazer = GetComponent<LineRenderer>();
            _lazer.enabled = false;
            _maxLvl = GameManager.Instance.GetTowersManager.rocketTowers.Length - 1;
            
        }

        protected override void Update()
        {
            base.Update();
            LookAtTarget();
            
        }
        public override void Fire()
        {
            if (fireCountDown <= 0f )
            {
                _lazer.enabled = true;
                _lazerStartFire = Time.time;
                _lazer.SetPosition(0, _firePoint.position);
                _lazer.SetPosition(1, _target.transform.position);
                fireCountDown = 1f / _attackPerSecond;
                _target.ApplyDamage(_damageInfo);
            }
            if (Time.time>= _lazerStartFire+_lazerFinishFire)
            {
                _lazer.enabled = false;
            }
            fireCountDown -= Time.deltaTime;
        }
        public override void UpdateTower()
        {
            if (_lvl < _maxLvl)
            {
                _lvl += 1;
                var tower = GameManager.Instance.GetTowersManager.lazerTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                newTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
            }
        }
        /// <summary>
        /// _rotateHead GameObject will look on to an enemy
        /// </summary>
        private void LookAtTarget()
        {
            
            if (_target != null && _rotateHead != null)
            {
                var direction = _target.EnemyTransform.position - _rotateHead.position;
                Quaternion lookRotation = Quaternion.Lerp(_rotateHead.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          Time.deltaTime * _turnSpeed);
                _rotateHead.rotation = lookRotation;
            }
        }
        
    }
}

