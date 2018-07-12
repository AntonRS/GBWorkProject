using UnityEngine;
using System.Collections.Generic;
using Game.Enemy;
namespace Game.Towers
{
    /// <summary>
    /// Класс, отвечающий за работу башни, стреляющей лазером.
    /// Требует наличие компонента LineRenderer.
    /// Наследуется от BaseTower.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    
    public class LazerTower : BaseTower
    {
        /// <summary>
        /// Список целей, которые башня может атаковать "цепным" лазером.
        /// Колличество целей зависит от параметра _maxTargetsCount.
        /// Первый элемент списка - ближний к башне враг, находящийся в радиусе атаки.
        /// Каждый следующий элемент находится на заданном радиусе от предыдущего.
        /// Элементы не повторяются.
        /// </summary>
        
        /// <summary>
        /// Компонент LineRenderer, отвечающий за визуальное отображене лазера
        /// </summary>
        private LineRenderer _lazer;
        /// <summary>
        /// Максимальное количесвто целей "цепного" лазера.
        /// </summary>
        private int _maxTargetsCount;
        /// <summary>
        /// Снижение урона для следующей цели в процентах.
        /// </summary>
        private int _nextTargetDamageReductionInPercent = 10;
        /// <summary>
        /// Максимальное расстояние до следующей цели цепного лазера.
        /// </summary>
        private int _nexTargetRadius = 5;

        private List<BaseEnemy> _chainTargets;

      
        #region LazerTower Functions

        /// <summary>
        /// Заполняет список целей targets. Длинна списка зависит от параметра _maxTargetsCount.
        /// </summary>
        protected override void UpdateTarget()
        {
            base.UpdateTarget();
            FillTargetsList();
        }
        private void FillTargetsList()
        {
            _chainTargets.Clear();
            BaseEnemy nearestEnemy = _target;
            if (nearestEnemy != null)
            {
                _chainTargets.Add(nearestEnemy);

                for (int i = 0; i < _maxTargetsCount; i++)
                {
                    BaseEnemy nextEnemy = FindNearestEnemyinRange(_chainTargets[i].transform.position, _nexTargetRadius);
                    if (nextEnemy != null)
                    {
                        _chainTargets.Add(nextEnemy);
                    }
                    else return;
                }
            }

        }
        /// <summary>
        /// Возвращает ближайший обьект типа BaseEnemy в заданном радиусе, не содердайщийся в списке targets.
        /// </summary>
        /// <param name="startpoint">начальная точка отсчета</param>
        /// <param name="range">радиус</param>
        /// <returns></returns>
        private BaseEnemy FindNearestEnemyinRange(Vector3 startpoint, float range)
        {
            float shortestDistance = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;
            foreach (BaseEnemy enemy in GameManager.Instance.GetEnemiesController.enemies)
            {
                if (enemy != null && _canAttack.Contains(enemy.EnemyType) && !_chainTargets.Contains(enemy))
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
        /// <summary>
        /// Реализация интерфейса нанесения урона.
        /// Урон уменьшается от цели к цели на величину _nextTargetDamageReductionInPercent в процентах.
        /// </summary>
        /// <param name="damageInfo"></param>
        private void SetDamage(DamageInfo damageInfo)
        {
            damageInfo.Damage *= Time.deltaTime;
            foreach (BaseEnemy enemy in _targets)
            {
                enemy.ApplyDamage(damageInfo);
                damageInfo.Damage -= (damageInfo.Damage * _nextTargetDamageReductionInPercent) / 100;
            }
        }
        #endregion
        #region BaseTower Overrides
        /// <summary>
        /// Логика стреьлбы лазерной башни
        /// </summary>
        protected override void Fire()
        {
            if (_chainTargets.Count > 0)
            {
                SetDamage(_damageInfo);
                _lazer.enabled = true;
                _lazer.positionCount = _chainTargets.Count + 1;
                _lazer.SetPosition(0, _firePoint.position);
                for (int i = 0; i < _chainTargets.Count; i++)
                {
                    _lazer.SetPosition(i + 1, _chainTargets[i].transform.position);
                }
            }
            else
            {
                _lazer.enabled = false;
                return;
            }
        }
        /// <summary>
        /// Логика улучшения лазерной башни. 
        /// </summary>
        public override void UpgradeTower()
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
        /// Логика слежения за целью поворотной части лазерной башни.
        /// </summary>
        protected override void LookAtTarget()
        {
            if (_rotateHead != null && _targets.Count > 0)
            {
                var direction = _targets[0].transform.position - _rotateHead.position;
                Quaternion lookRotation = Quaternion.Lerp(_rotateHead.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          Time.deltaTime * _turnSpeed);
                _rotateHead.rotation = lookRotation;
            }
        }
        /// <summary>
        /// Настройка стартовых параметров лазерной башни
        /// </summary>
        protected override void SetAwakeParams()
        {
            base.SetAwakeParams();
            _maxTargetsCount = _lvl;
            _lazer = GetComponent<LineRenderer>();
            _lazer.enabled = false;
            _chainTargets = new List<BaseEnemy>();



        }
        #endregion





    }
}


