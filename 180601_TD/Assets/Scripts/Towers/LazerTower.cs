using UnityEngine;
using System.Collections.Generic;
using Game.Enemy;
namespace Game.Towers
{
    [RequireComponent(typeof(LineRenderer))]
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
        private RaycastHit _hit;
        private LineRenderer _lazer;
        private int targetMultiplier = 1;
        
        
        float searchingTime = 0;

        private List<BaseEnemy> targets;
        protected override void Start()
        {
            base.Start();
            _lazer = GetComponent<LineRenderer>();
            _lazer.enabled = false;
            targets = new List<BaseEnemy>();
            _maxLvl = GameManager.Instance.GetTowersManager.rocketTowers.Length - 1;

        }

        protected override void Update()
        {
            base.Update();
            FillTargetsList(5);
            LookAtTarget();
            Fire();
        }
        
        public override void Fire()
        {
            
            if (targets.Count > 0)
            {
                _lazer.enabled = true;
                _lazer.positionCount = targets.Count + 1;
                _lazer.SetPosition(0, _firePoint.position);
                for (int i = 0; i < targets.Count; i++)
                {
                    _lazer.SetPosition(i + 1, targets[i].transform.position);
                }
            }
            else
            {
                _lazer.enabled = false;

            }
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
            if (_rotateHead != null&&targets.Count>0)
            {
                var direction = targets[0].transform.position - _rotateHead.position;
                Quaternion lookRotation = Quaternion.Lerp(_rotateHead.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          Time.deltaTime * _turnSpeed);
                _rotateHead.rotation = lookRotation;
            }
        }
        
        private void FillTargetsList(int enemiesCount)
        {
            targets.Clear();
            BaseEnemy nearestEnemy = FindNearestEnemyinRange(transform.position, _attackRange);
            if (nearestEnemy != null)
            {
                targets.Add(nearestEnemy);
                for (int i = 0; i < enemiesCount; i++)
                {
                    BaseEnemy nextEnemy = FindNearestEnemyinRange(targets[i].transform.position, 10);
                    if (nextEnemy != null)
                    {
                        targets.Add(nextEnemy);
                    }
                    else return;
                }
            }
        }

        private BaseEnemy FindNearestEnemyinRange(Vector3 startpoint, float range)
        {
            float shortestDistance = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;
            foreach (BaseEnemy enemy in GameManager.Instance.GetEnemiesController.enemies)
            {
                if (((_isAbleToAttackGround && !enemy.IsFlying) || (_isAbleToAttackAir && enemy.IsFlying)) && !targets.Contains(enemy))
                {
                    float distanceToEnemy = Vector3.Distance(startpoint, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }
            if (nearestEnemy != null && shortestDistance <= range)
            {
                return nearestEnemy;
            }
            else
            {
                return null;
            }
        }



    }
}


