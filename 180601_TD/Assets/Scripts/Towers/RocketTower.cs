using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    /// <summary>
    /// Определяет логику и параметры работы башни с ракетами.
    /// </summary>
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
        /// Задержка между залпами.
        /// </summary>
        [SerializeField] private float _timeBetweenAttack;
        /// <summary>
        /// Счетчик перезарядки.
        /// </summary>
        private float _fireCountDown = 0f;
        /// <summary>
        /// Точка, к которой ракета летит перед тем как выйти на цель.
        /// </summary>
        [SerializeField] private Transform _attackPosition;
        /// <summary>
        /// Количество ракет в залпе.
        /// </summary>
        private int _misslesCountInOneAttack;
        /// <summary>
        /// Время до создания следующей ракеты.
        /// </summary>
        private const float _timeBetweenMisslesSpawn = 0.2f;
        /// <summary>
        /// Логика стрельбы башни с ракетами. 
        /// Башня выпускает несколько ракет в зависимости от уровня прокачки.
        /// </summary>

        #region BaseTower overrides
        protected override void Fire()
        {
            if (_fireCountDown <= 0 && _target)
            {
                if (_misslesCountInOneAttack > 0)
                {
                    CreateMissle();

                    _fireCountDown = _timeBetweenMisslesSpawn;
                    _misslesCountInOneAttack--;
                }
                else
                {
                    _fireCountDown = _timeBetweenAttack;
                    _misslesCountInOneAttack = _lvl + 1;
                }
            }
            _fireCountDown -= Time.deltaTime;
        }
        /// <summary>
        /// Логика появления ракеты на игровой сцене.
        /// </summary>
        private void CreateMissle()
        {
            var _tempMissle = Instantiate(_missle, _firePoint.position, _firePoint.rotation);
            _tempMissle.Target = _target;
            _tempMissle.Speed = _missleSpeed;
            _tempMissle.DamageInfo = _damageInfo;
            _tempMissle.attackPosition = _attackPosition;
        }
        /// <summary>
        /// Логика прокачки башни с ракетами.
        /// </summary>
        public override void UpgradeTower()
        {
            if (_lvl < _maxLvl)
            {
                _lvl += 1;
                var tower = GameManager.Instance.GetTowersManager.rocketTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                //newTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
            }
        }
        /// <summary>
        /// Логика слежения за целью.
        /// </summary>
        protected override void LookAtTarget()
        {
            if (_target != null && _rotateHead != null)
            {
                var direction = _target.transform.position - _rotateHead.position;
                Quaternion lookRotation = Quaternion.Lerp(_rotateHead.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          Time.deltaTime * _turnSpeed);
                _rotateHead.rotation = lookRotation;
                _rotateHead.eulerAngles = new Vector3(0, _rotateHead.eulerAngles.y, 0);
            }
        }
        /// <summary>
        /// Инициализация начальных параметров.
        /// </summary>
        protected override void SetAwakeParams()
        {
            base.SetAwakeParams();
            _misslesCountInOneAttack = _lvl + 1;
        }
        #endregion

    }
}

