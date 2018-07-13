using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Enemy
{
    public class EnemiesController:MonoBehaviour
    {
        
        [HideInInspector] public List<BaseEnemy> enemies;
        [HideInInspector]
        public Transform destination;
        [HideInInspector] public List<EnemySpawner> spawners;
        public BaseEnemy[] enemiesTypes;
        [HideInInspector]
        public Wave[] waves;

        public void SpawnSimultaneously(int waveIndex)
        {
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.SpawnWave(waveIndex);
            }
        }
        
        public void AddEnemy(BaseEnemy enemy)
        {
            if (!enemies.Contains(enemy) && enemy != null)
            {
                enemies.Add(enemy);
            }
        }
        public void DeleteEnemy(BaseEnemy enemy)
        {
            if (enemies.Contains(enemy) && enemy != null)
            {
                enemies.Remove(enemy);
            }
        }
        public void ClearEnemyList()
        {
            
            foreach (BaseEnemy enemy in enemies)
            {
                
                Destroy(enemy.gameObject);
            }
            enemies.Clear();
        }
        public void ClearSpawnerList()
        {
                spawners.Clear();
        }
        public void SetDestination(Transform destination)
        {
            this.destination = destination;
        }
        public void SetSpawner(Transform spawnerPosition)
        {
            spawnerPosition.gameObject.AddComponent<EnemySpawner>();
        }

    }
}

