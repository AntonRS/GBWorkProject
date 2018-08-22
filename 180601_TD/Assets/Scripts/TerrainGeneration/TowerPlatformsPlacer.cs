using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    public class TowerPlatformsPlacer
    {
        const string TowerPlatformSpawnPointTag = "Tile_TowerPlatformSpawnPoint";

        public GameObject[] BuildTowerPlatforms(GameObject towerPlatformPrefab, List<GameObject> roadTiles, Transform terrainParent, int towerPlatformsCount)
        {

            //находим все возможные точки расположения платформ под башни

            List<Transform> towerPlatformSpawnPoints = new List<Transform>();

            foreach (GameObject tile in roadTiles)
                foreach (Transform child in tile.transform)
                    if (child.tag == TowerPlatformSpawnPointTag)
                        towerPlatformSpawnPoints.Add(child);

            //корректируем заданное число платформ
            if (towerPlatformsCount > towerPlatformSpawnPoints.Count)
                towerPlatformsCount = towerPlatformSpawnPoints.Count;


            GameObject[] towerPlatforms = new GameObject[towerPlatformsCount];

            //создаём заданное количество платформ - пока рандомно

            for (int i = 0; i < towerPlatformsCount; i++)
            {

                int randomIndex = Random.Range(0, towerPlatformSpawnPoints.Count);
                Object.Instantiate(towerPlatformPrefab, towerPlatformSpawnPoints[randomIndex].position, Quaternion.identity, terrainParent);

                towerPlatformSpawnPoints.RemoveAt(randomIndex);
            }


            return towerPlatforms;

        }
    }

}