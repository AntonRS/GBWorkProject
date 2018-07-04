using UnityEngine;
namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {

        private Wave[] waves;
        private Transform destination;
        
        private void Start()
        {

            GameManager.Instance.GetEnemiesController.spawners.Add(this);
            waves = GameManager.Instance.GetEnemiesController.waves;
            destination = GameManager.Instance.GetEnemiesController.destination;
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
        public void SpawnRandomWave(int waveIndex, int minenEmiesCount, int maxEnemiesCount, int modPercent)
        {
            var rndEnemiesCount = Random.Range(minenEmiesCount, maxEnemiesCount);
            for (int i = 0; i < rndEnemiesCount; i++)
            {
                var newEnemy = Instantiate(GetRandomEnemy(), GetRandomPosition(), Quaternion.identity);
                newEnemy.Hp += (newEnemy.Hp/100)*(modPercent*waveIndex);
                newEnemy.Destination = destination;
            }
        }
        public BaseEnemy GetRandomEnemy()
        {
            int rnd = Random.Range(0, GameManager.Instance.GetEnemiesController. enemiesTypes.Length-1);
            return GameManager.Instance.GetEnemiesController.enemiesTypes[rnd];
        }

    }
}

