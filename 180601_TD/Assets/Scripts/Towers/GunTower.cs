using UnityEngine;
using Game.CommandUI;
namespace Game.Towers
{
    /// <summary>
    /// Содержит логику работы пулеметной башни.
    /// </summary>
    public class GunTower : BaseTower
    {
        /// <summary>
        /// Уменьшение скорости врага в %
        /// </summary>
        [Tooltip("Уменьшение скорости врага в %")]
        [SerializeField] private int _speedreduction;      
        
        #region UI
        public override void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {

                viaButton.gameObject.SetActive(true);
                _fakeRange = _towersManager.GunTowers[_lvl + 1].AttackRange;

            }
        }
        #endregion
        #region GunTower Functions and overrides
        /// <summary>
        /// Возвращает стоимость апгрейда башни
        /// </summary>
        /// <returns>стоимость апгрейда башни</returns>       
        public override int? GetUpgradeCost()
        {
            if (_lvl < _towersManager.GunTowers.Length)
            {
                return _towersManager.GunTowers[_lvl + 1].Cost;
            }
            else return null;
        }
        /// <summary>
        /// Логика нанесения урона.
        /// </summary>
        /// <param name="damageInfo"></param>
        private void SetDamage(DamageInfo damageInfo)
        {
            damageInfo.Damage *= Time.deltaTime;
            _target.ApplyDamage(damageInfo);
        }
        /// <summary>
        /// Логика ведения огня.
        /// </summary>
        protected override void Fire()
        {
            if (_target)
            {
                SetDamage(_damageInfo);
            }
        }
        /// <summary>
        /// Логика улучшения башни
        /// </summary>
        public override void UpgradeTower()
        {
            if (_lvl < _maxLvl && GameManager.Instance.CurrentMoney >= GetUpgradeCost())
            {

                _lvl += 1;

                var tower = GameManager.Instance.GetTowersManager.GunTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                newTower.transform.SetParent(_terrainGenerator.transform);
                GameManager.Instance.UpdateMoney(-Cost);
                Destroy(gameObject);
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
            }
        }
        /// <summary>
        /// Инициальзация начальных параметров.
        /// </summary>
        protected override void SetAwakeParams()
        {
            base.SetAwakeParams();
            _maxLvl = _towersManager.GunTowers.Length - 1;
            _damageInfo.SpeedReduction = _speedreduction;
        }
        #endregion

    }
}

