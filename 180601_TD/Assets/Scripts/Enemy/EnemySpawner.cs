using UnityEngine;
namespace Game.Enemy
{
    /// <summary>
    /// Содержит логику спавнера врагов.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        /// <summary>
        /// Transform финиша.
        /// </summary>
        private Transform _destination;
        /// <summary>
        /// Минимальное значение сдвига позиции спавна врага.
        /// </summary>
        private int _minRandomSpawnPosOffset;
        /// <summary>
        /// Минимальное значение сдвига позиции спавна врага.
        /// </summary>
        private int _maxRandomSpawnPosOffset;
        /// <summary>
        /// Ссылка на EnemiesController.
        /// </summary>
        private EnemiesController _enemiesController;

        #region Unity Functions
        private void Start()
        {
            _enemiesController = GameManager.Instance.GetEnemiesController;
            _enemiesController.Spawners.Add(this);
            _maxRandomSpawnPosOffset = _enemiesController.MaxRandomSpawnPosOffset;
            _minRandomSpawnPosOffset = _enemiesController.MinRandomSpawnPosOffset;
            _destination = _enemiesController.Destination;

        }
        #endregion
        #region Spawner functions
        /// <summary>
        /// Возвращает рандомную позицию со сдвигом по X и Y от _minRandomSpawnPosOffset до _maxRandomSpawnPosOffset.
        /// </summary>
        /// <returns></returns>
        Vector3 GetRandomPosition()
        {
            return new Vector3(transform.position.x + Random.Range(_minRandomSpawnPosOffset, _maxRandomSpawnPosOffset),
                                                 transform.position.y,
                                                 transform.position.z + Random.Range(_minRandomSpawnPosOffset, _maxRandomSpawnPosOffset));

        }
        /// <summary>
        /// Спавнит волну из рандомных врагов.
        /// </summary>
        /// <param name="waveIndex">номер волны</param>
        /// <param name="minEnemiesCount">минимальное количество врагов</param>
        /// <param name="maxEnemiesCount">максимальное количество</param>
        /// <param name="modPercent">параметр увеличения в %</param>
        public void SpawnRandomWave(int waveIndex, int minEnemiesCount, int maxEnemiesCount, int modPercent)
        {
            var rndEnemiesCount = Random.Range(minEnemiesCount, maxEnemiesCount);
            for (int i = 0; i < rndEnemiesCount; i++)
            {
                var newEnemy = Instantiate(_enemiesController.GetRandomEnemy(), GetRandomPosition(), Quaternion.identity);
                newEnemy.Hp += (newEnemy.Hp / 100) * (modPercent * waveIndex);
                newEnemy.Cost += (newEnemy.Cost / 100) * (modPercent * waveIndex);
                newEnemy.Destination = _destination;
            }
        }
        #endregion




    }
}

