using UnityEngine;
using Game.TerrainGeneration;
namespace Game.Towers
{
    public class TowersManager : MonoBehaviour
    {

        public BaseTower[] rocketTowers;
        public BaseTower[] lazerTowers;
        public BaseTower[] gunTowers;
        [SerializeField] private BuildBase _towerBuildBase;

        private Transform _buildTransform;

        private void Start()
        {
            _buildTransform = FindObjectOfType<TerrainGeneratorController>().GetComponent<Transform>();
        }

        public void BuildRocketTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney>= rocketTowers[0].Cost)
            {
                var newRocketTower = Instantiate(rocketTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                GameManager.Instance.UpdateMoney(-rocketTowers[0].Cost);
                Destroy(buildBase.gameObject);
            }
            
            
        }
        public void BuildLazerTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney >= lazerTowers[0].Cost)
            {
                var newRocketTower = Instantiate(lazerTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                GameManager.Instance.UpdateMoney(-lazerTowers[0].Cost);
                Destroy(buildBase.gameObject);
            }
            
        }
        public void BuildGunTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney >= gunTowers[0].Cost)
            {
                var newRocketTower = Instantiate(gunTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(_buildTransform);
                GameManager.Instance.UpdateMoney(-gunTowers[0].Cost);
                Destroy(buildBase.gameObject);
                
            }
            
        }
        public void SellTower(Transform buildPosition, int sellValue)
        {
            var newBuildBase = Instantiate(_towerBuildBase, buildPosition.position, buildPosition.rotation);
            newBuildBase.transform.SetParent(_buildTransform);
            GameManager.Instance.UpdateMoney(sellValue);
        }



    }
}

