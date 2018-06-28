using UnityEngine;
namespace Game.Towers
{
    public class TowersManager : MonoBehaviour
    {

        public RocketTower[] RocketTowers; 

        public void BuildRocketTower(Transform buildBase)
        {
            var newRocketTower = Instantiate(RocketTowers[0], buildBase.position, buildBase.rotation);
            newRocketTower.transform.SetParent(GameManager.Instance.GetTerrainGenerator.transform);
            Destroy(buildBase.gameObject);
        }
        


    }
}

