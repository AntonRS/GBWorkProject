using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    public class GunTower : BaseTower
    {
        [SerializeField] private int _speedreduction;

        private void SetDamage(DamageInfo damageInfo)
        {
            damageInfo.Damage *= Time.deltaTime;
            _target.ApplyDamage(damageInfo);
        }
        protected override void Fire()
        {
            if (_target)
            {
                SetDamage(_damageInfo);
            }
        }
        public override void UpgradeTower()
        {
            if (_lvl < _maxLvl)
            {
                _lvl += 1;
                var tower = GameManager.Instance.GetTowersManager.gunTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                //newTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
            }
        }
        
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

        protected override void SetAwakeParams()
        {
            base.SetAwakeParams();
            _damageInfo.SpeedReduction = _speedreduction;
        }
    }
}

