using System.Collections.Generic;
using UnityEngine;
namespace Game.Enemy
{
    /// <summary>
    /// Содержит логику управления врагами.
    /// </summary>
    public class EnemiesController:MonoBehaviour
    {
        /// <summary>
        /// Список всех врагов на сцене.
        /// </summary>
        [HideInInspector]
        public List<BaseEnemy> Enemies;
        /// <summary>
        /// Точка финиша.
        /// </summary>
        [HideInInspector] public Transform Destination;
        /// <summary>
        /// Список всех спаунеров на сцене.
        /// </summary>
        [HideInInspector] public List<EnemySpawner> Spawners;
        /// <summary>
        /// Список всех типов врагов.
        /// </summary>
        [Tooltip("Список всех типов врагов.")]
        public BaseEnemy[] enemiesTypes;
        /// <summary>
        /// Минимальное значение сдвига позиции спавна врага.
        /// </summary>
        [Tooltip("Минимальное значение сдвига позиции спавна врага.")]
        [Range(-10, 0)]
        public int MinRandomSpawnPosOffset = -3;
        /// <summary>
        /// Минимальное значение сдвига позиции спавна врага.
        /// </summary>
        [Tooltip("Максимальное значение сдвига позиции спавна врага.")]
        [Range(0, 10)]
        public int MaxRandomSpawnPosOffset = 3;


        #region EnemiesController Functions
        /// <summary>
        /// Вызывает SpawnRandomWave одновременно у всех спавнеров а сцене.
        /// </summary>
        /// <param name="waveIndex">Номер волны</param>
        /// <param name="minEnemiesCount">минимальное количество врнагов</param>
        /// <param name="maxEnemiesCount">Максимальное количество врагов</param>
        /// <param name="modPercent">Параметр увеличения в %</param>
        public void SpawnSimultaneously(int waveIndex, int minEnemiesCount, int maxEnemiesCount, int modPercent)
        {
            foreach (EnemySpawner spawner in Spawners)
            {
                spawner.SpawnRandomWave(waveIndex, minEnemiesCount, maxEnemiesCount, modPercent);
            }
        }
        /// <summary>
        /// Добавляет врага в список Enemies.
        /// </summary>
        /// <param name="enemy">Враг</param>
        public void AddEnemy(BaseEnemy enemy)
        {
            if (!Enemies.Contains(enemy) && enemy != null)
            {
                Enemies.Add(enemy);
            }
        }
        /// <summary>
        /// Добавляет врага из списка Enemies.
        /// </summary>
        /// <param name="enemy">Враг</param>
        public void DeleteEnemy(BaseEnemy enemy)
        {
            if (Enemies.Contains(enemy) && enemy != null)
            {
                Enemies.Remove(enemy);
            }
        }
        /// <summary>
        /// Очищает список Enemies и вызвает Destroy у всех содержащихся в нем врагов.
        /// </summary>
        public void ClearEnemyList()
        {
            foreach (BaseEnemy enemy in Enemies)
            {
                Destroy(enemy.gameObject);
            }
            Enemies.Clear();
        }
        /// <summary>
        /// Очищает список Spawners.
        /// </summary>
        public void ClearSpawnerList()
        {
            Spawners.Clear();
        }
        /// <summary>
        /// Добаляет скрипт спавнер на обьект на сцене.
        /// </summary>
        /// <param name="spawner">Transform обьекта, на который нужно добавить скрипт</param>
        public void SetSpawner(Transform spawner)
        {
            spawner.gameObject.AddComponent<EnemySpawner>();
        }
        /// <summary>
        /// Возвращает рандомного врага из списка enemiesTypes
        /// </summary>
        /// <returns></returns>
        public BaseEnemy GetRandomEnemy()
        {
            int rnd = Random.Range(0, enemiesTypes.Length - 1);
            return enemiesTypes[rnd];
        }
        #endregion


    }
}

