using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    public Wave [] waves;
    public Transform destination;
    
    private void Start()
    {
        SpawnWave(0);
    }
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-3, 3),
                                             transform.position.y ,
                                             transform.position.z + Random.Range(-3,3));
        return randomPosition;
    }
    
    public void SpawnWave(int waveindex)
    {
        foreach (var enemy in waves[waveindex].enemiesTypeAndCount)
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
