using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public class RocketTower : BaseTower
    {
        /// <summary>
        /// Префаб ракеты.
        /// </summary>
        [SerializeField] private Missle _missle;
        /// <summary>
        /// Скорость ракеты.
        /// </summary>
        [SerializeField] private float _missleSpeed;
        /// <summary>
        /// Колличество выстрелов в секунду.
        /// </summary>
        [SerializeField] private float _attackPerSecond;
        /// <summary>
        /// Счетчик перезарядки.
        /// </summary>
        private float _fireCountDown = 0f;
        /// <summary>
        /// Точка к которой ракета летит перед тем как выйти на цель.
        /// </summary>
        [SerializeField] private Transform _attackPosition;
        /// <summary>
        /// Колличество ракет в залпе. Определяется уровнем прокачки башни.
        /// </summary>
        private int _misslesCountInOneAttack;

        private int _misslesInSecInMultipleAttack = 5;
        






        protected override void Fire()
        {
            if (_fireCountDown <= 0f && _target)
            {
                if (_misslesCountInOneAttack > 0)
                {
                    var _tempMissle = Instantiate(_missle, _firePoint.position, _firePoint.rotation);
                    _tempMissle.Target = _target;
                    _tempMissle.Speed = _missleSpeed;
                    _tempMissle.DamageInfo = _damageInfo;
                    _tempMissle.attackPosition = _attackPosition;
                    _fireCountDown = 1f / _misslesInSecInMultipleAttack;
                    _misslesCountInOneAttack--;
                }
                else
                {
                    _fireCountDown = 1f / _attackPerSecond;
                    _misslesCountInOneAttack = _lvl + 1;

                }
            }
            _fireCountDown -= Time.deltaTime;
        }
        public override void UpgradeTower()
        {
            if (_lvl < _maxLvl)
            {
                _lvl += 1;
                var tower = GameManager.Instance.GetTowersManager.rocketTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                newTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
            }
        }
        
        protected override void LookAtTarget()
        {
            
            if (_target != null && _rotateHead != null)
            {
                //_rotateHead.LookAt(_target.transform.position);
                //_rotateHead.eulerAngles = new Vector3(0, _rotateHead.eulerAngles.y, 0);

                var direction = _target.EnemyTransform.position - _rotateHead.position;

                Quaternion lookRotation = Quaternion.Lerp(_rotateHead.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          Time.deltaTime * _turnSpeed);
                _rotateHead.rotation = lookRotation;
                _rotateHead.eulerAngles = new Vector3(0, _rotateHead.eulerAngles.y, 0);
            }
        }
        protected override void SetAwakeParams()
        {
            base.SetAwakeParams();
            _misslesCountInOneAttack = _lvl+1;

        }
    }
}

