using UnityEngine;
using Game.Enemy;
using Game.CommandUI;
namespace Game.Towers
{
    public class GunTower : BaseTower
    {
        [SerializeField] private int _speedreduction;


        public override int? GetUpgradeCost()
        {
            if (_lvl < GameManager.Instance.GetTowersManager.gunTowers.Length)
            {
                return GameManager.Instance.GetTowersManager.gunTowers[_lvl + 1].Cost;
            }
            else return null;
        }



        public override void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {

                viaButton.gameObject.SetActive(true);
                _fakeRange = GameManager.Instance.GetTowersManager.gunTowers[_lvl + 1].AttackRange;
               
            }
        }

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
            
            if (_lvl < _maxLvl && GameManager.Instance.CurrentMoney >= GetUpgradeCost()) 
            {
                
                _lvl += 1;
                
                var tower = GameManager.Instance.GetTowersManager.gunTowers[_lvl];
                var newTower = Instantiate(tower, transform.position, Quaternion.identity);
                newTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                GameManager.Instance.UpdateMoney(-Cost);
                Destroy(gameObject);
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
            _maxLvl = GameManager.Instance.GetTowersManager.gunTowers.Length - 1;
            _damageInfo.SpeedReduction = _speedreduction;
        }
    }
}

