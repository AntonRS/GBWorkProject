using UnityEngine;
namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {

        private Wave[] waves;
        private Transform destination;
        
        private void Start()
        {

            EnemiesController.Instance.spawners.Add(this);
            waves = EnemiesController.Instance.waves;
            destination = EnemiesController.Instance.destination;
        }
        Vector3 GetRandomPosition()
        {
            Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-3, 3),
                                                 transform.position.y,
                                                 transform.position.z + Random.Range(-3, 3));
            return randomPosition;
        }

        public void SpawnWave(int waveIndex)
        {
            foreach (var enemy in waves[waveIndex].enemiesTypeAndCount)
            {
                for (int i = 0; i < enemy.count; i++)
                {
                    var tempEnemy = Instantiate(enemy.enemy, GetRandomPosition(), transform.rotation);
                    tempEnemy.Speed = enemy.speed + Random.Range(-0.5f, 0.5f);
                    tempEnemy.Hp = enemy.hp;
                    tempEnemy.Destination = destination;
                }

            }
            
        }


    }
}

