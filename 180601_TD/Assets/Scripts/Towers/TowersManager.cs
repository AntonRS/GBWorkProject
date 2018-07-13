using UnityEngine;
namespace Game.Towers
{
    public class TowersManager : MonoBehaviour
    {

        public BaseTower[] rocketTowers;
        public BaseTower[] lazerTowers;
        public BaseTower[] gunTowers;


        public void BuildRocketTower(Transform buildBase)
        {
            if (rocketTowers.Length!=0)
            {
                var newRocketTower = Instantiate(rocketTowers[0], buildBase.position, buildBase.rotation);
                //newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
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
                //newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
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
                //newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
                Destroy(buildBase.gameObject);
            }
            else
            {
                throw new System.NullReferenceException("Нет префаба для строительства GunTower");
            }
        }



    }
}

