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
            if (rocketTowers.Length!=0)
            {
                var newRocketTower = Instantiate(rocketTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                Destroy(buildBase.gameObject);
            }
            else
            {
                throw new System.NullReferenceException("Нет префаба для строительства RocktTower");
            }
            
        }
        public void BuildLazerTower(Transform buildBase)
        {
            if (rocketTowers.Length != 0)
            {
                var newRocketTower = Instantiate(lazerTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                Destroy(buildBase.gameObject);
            }
            
            else
            {
                throw new System.NullReferenceException("Нет префаба для строительства LazerTower");
            }
        }
        public void BuildGunTower(Transform buildBase)
        {
            if (rocketTowers.Length != 0)
            {
                var newRocketTower = Instantiate(gunTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(_buildTransform);
                Destroy(buildBase.gameObject);
            }
            else
            {
                throw new System.NullReferenceException("Нет префаба для строительства GunTower");
            }
        }
        public void SellTower(Transform buildPosition, int sellValue)
        {
            var newBuildBase = Instantiate(_towerBuildBase, buildPosition.position, buildPosition.rotation);
            newBuildBase.transform.SetParent(_buildTransform);
        }



    }
}

