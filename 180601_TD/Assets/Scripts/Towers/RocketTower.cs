﻿using UnityEngine;
namespace Game.Towers
{
    public class RocketTower : BaseTower
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
        /// <summary>
        /// Countdown to fire
        /// </summary>
        [SerializeField] Missle _ammunition;
        private float fireCountDown = 0f;

        protected override void Start()
        {
            base.Start();

            _maxLvl = GameManager.Instance.GetTowersManager.rocketTowers.Length - 1;
            if (_firePoint == null)
            {
                _firePoint = transform;
            }
        }

        protected override void Update()
        {
            base.Update();
            LookAtTarget();
        }
        public override void Fire()
        {
            if (fireCountDown <= 0f)
            {
                var _tempMissle = Instantiate(_ammunition, _firePoint.position, _firePoint.rotation);
                _tempMissle.Target = _target;
                _tempMissle.Speed = _ammunitionSpeed;
                _tempMissle.DamageInfo = _damageInfo;
                fireCountDown = 1f / _attackPerSecond;
            }
            fireCountDown -= Time.deltaTime;
        }
        public override void UpdateTower()
        {
            if (_lvl < _maxLvl)
            {
                _lvl += 1;
                var tower = GameManager.Instance.GetTowersManager.rocketTowers[_lvl];
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

