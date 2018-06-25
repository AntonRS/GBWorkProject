using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Enemy
{
    public class EnemiesController : Singleton<EnemiesController>
    {
        public List<BaseEnemy> enemies;
        public Transform destination;
        public List<EnemySpawner> spawners;
        

        public void SpawnSimultaneously(int waveIndex)
        {
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.SpawnWave(waveIndex);
            }
        }
        public void SetDetinationInSpawners()
        {
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.destination = this.destination;
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
    }
}

