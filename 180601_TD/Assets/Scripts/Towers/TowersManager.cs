using UnityEngine;
namespace Game.Towers
{
    public class TowersManager : MonoBehaviour
    {
        /// <summary>
        /// Список всех башен с ракетами
        /// </summary>
        [Tooltip("Префабы всех башен с ракетами")]
        public BaseTower[] RocketTowers;
        /// <summary>
        /// Список всех лазерных башен
        /// </summary>
        [Tooltip("Префабы всех лазерных башен")]
        public BaseTower[] LazerTowers;
        /// <summary>
        /// Список всех башен с пулеметами
        /// </summary>
        [Tooltip("Префабы всех башен с пулеметами")]
        public BaseTower[] GunTowers;
        /// <summary>
        /// Префаб строительной площадки
        /// </summary>
        [Tooltip("Префаб строительной площадки")]
        [SerializeField] private BuildBase _towerBuildBase;
        /// <summary>
        /// Transform места строительства
        /// </summary>
        private Transform _buildTransform;

        #region Unity functions
        private void Start()
        {
            _buildTransform = FindObjectOfType<TerrainGeneratorController>().GetComponent<Transform>();
        }
        #endregion

        #region TowersManager Functions
        /// <summary>
        /// Логика апгрейда башни с ракетами
        /// </summary>
        /// <param name="buildBase"></param>
        public void BuildRocketTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney >= RocketTowers[0].Cost)
            {
                var newRocketTower = Instantiate(RocketTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(_buildTransform);
                GameManager.Instance.UpdateMoney(-RocketTowers[0].Cost);
                Destroy(buildBase.gameObject);
            }


        }
        public void BuildLazerTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney >= LazerTowers[0].Cost)
            {
                var newRocketTower = Instantiate(LazerTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(_buildTransform);
                GameManager.Instance.UpdateMoney(-LazerTowers[0].Cost);
                Destroy(buildBase.gameObject);
            }

        }
        public void BuildGunTower(Transform buildBase)
        {
            if (GameManager.Instance.CurrentMoney >= GunTowers[0].Cost)
            {
                var newRocketTower = Instantiate(GunTowers[0], buildBase.position, buildBase.rotation);
                newRocketTower.transform.SetParent(_buildTransform);
                GameManager.Instance.UpdateMoney(-GunTowers[0].Cost);
                Destroy(buildBase.gameObject);

            }

        }
        public void SellTower(Transform buildPosition, int sellValue)
        {
            var newBuildBase = Instantiate(_towerBuildBase, buildPosition.position, buildPosition.rotation);
            newBuildBase.transform.SetParent(_buildTransform);
            GameManager.Instance.UpdateMoney(sellValue);
        }
        #endregion




    }
}

