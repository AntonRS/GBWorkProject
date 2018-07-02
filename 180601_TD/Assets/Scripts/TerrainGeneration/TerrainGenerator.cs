﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.TerrainGeneration
{

    public class TerrainGenerator
    {

        private GameObject _terrainParent;

        public GameObject TerrainParent
        {
            get
            {
                return _terrainParent;
            }
        }

        private GameObject _towerPlatform;
        private GameObject _roadStartPrefab;
        private GameObject _roadFinishPrefab;
                
        private GameObject[] _roadTilesPrefabs;

        private List<GameObject> _roadTiles = new List<GameObject>();

        private int _roadLengthCounter;

        private Vector3 _currentConnectionPointPos;
        private Vector3 _currentTileCenter;

        private const float DistanceSqrMagnitude = 1f;//число для сравнения местоположения. Если (точкаА - точкаБ).sqrMagnitude < _distanceSqrMagnitude, считаем, что эти точки находятся в одном и том же месте.
        private const int MaxAttemptsToBuildMap = 25;//предел для количества попыток построить карту. Просто на всякий случай, чтобы не попасть в бесконечный цикл. Если оказывается так, что и на последний раз карту построить не удаётся, просто финальный тайл ставится на месте того, который вызывал проблемы в последний раз. Таким образом в крайне редких случаях карта может не соответствовать параметрам (быть значительно короче).
        private const float TileFlipDegrees = 90f;//на сколько градусов поворачиваем тайл
        private const int TileMaxFlipTimes = 3;//сколько раз максимум поворачиваем тайл
        private const float NavMeshBuildDelay = 0.1f;//пауза перед генерацией навмеша, чтобы тайлы успели удалиться перед собственно генерацией

        private const string TileConnectionPointObjTag = "Tile_ConnectionPoint";
        private const string TileTowerPlatformSpawnPointObjTag = "Tile_TowerPlatformSpawnPoint";

        
        private float _bordersOffset; // отступ от границ. может, и не пригодится

        private NavMeshSurface _navMeshSurface;

        public TerrainGenerator(GameObject terrainParent)
        {
            _terrainParent = terrainParent;

            _navMeshSurface = _terrainParent.GetComponent<NavMeshSurface>();
            if (_navMeshSurface == null)
                _navMeshSurface = terrainParent.AddComponent<NavMeshSurface>();            
        }

        public void LoadTilePrefabs (TerrainTilesetData tileset)
        {
            _towerPlatform = tileset.TowerPlatform;
            _roadStartPrefab = tileset.RoadStartPrefab;
            _roadFinishPrefab = tileset.RoadFinishPrefab;
            _roadTilesPrefabs = tileset.RoadTilesPrefabs;
        }

        /// <summary>
        /// Генерирует карту рандомной длины в заданных рамках. Возвращает List<GameObject>, содержащий все тайлы дороги. Из них первый (с индексом 0) - стартовая зона, а последний (с индексом length - 1) - финишная зона.
        /// </summary>
        /// <param name="minRoadTiles">Минимальное количество тайлов дороги, не считая стартового и конечного.</param>
        /// <param name="maxRoadTiles">Максимальное количество тайлов дороги, не считая стартового и конечного.</param>
        /// <param name="towerPlatformsCount">Количество платформ под башни.</param>
        public List<GameObject> GenerateTerrain(int minRoadTiles, int maxRoadTiles, int towerPlatformsCount)
        {

            for (int i = 0; i < MaxAttemptsToBuildMap; i++)
            {
                if (!TryGenerateRoad(minRoadTiles, maxRoadTiles, i))
                {                    
                    DestroyRoad();
                }
                else
                    break;
            }

            PlaceTowerPlatforms(towerPlatformsCount);
            //GenerateNavMesh();
            return _roadTiles;

        }

        /// <summary>
        /// Уничтожает созданную карту и прилагающиеся к ней объекты и компоненты (например, навмеш)
        /// </summary>
        public void DestroyTerrain()
        {
            DestroyRoad();
            //GenerateNavMesh();
        }

        /// <summary>
        /// Уничтожает дорогу и вычищает список уже установленных тайлов.
        /// </summary>
        private void DestroyRoad()
        {
            _roadTiles.Clear();

            foreach (Transform child in _terrainParent.transform)
                Object.Destroy(child.gameObject);                        
        }

        /// <summary>
        /// Пробует сгенерировать дорогу. Возвращает истину, если построить дорогу удалось, ложь - если нет.
        /// </summary>
        /// <returns></returns>
        private bool TryGenerateRoad(int minRoadTiles, int maxRoadTiles, int attemptIndex)
        {
            _currentTileCenter = _terrainParent.transform.position;

            //устанавливаем начальный тайл и начальные точки
            _roadTiles.Add(InstantiateTileAtCurrentCenter(_roadStartPrefab).gameObject);
            _currentConnectionPointPos = FindConnectionPoint(_roadTiles[0].transform).position;

            //определяем, с какой стороны ставить следующий тайл, и переносим точку спавна следующего тайла
            _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);

            //дорога
            if (
                    !TryPlaceRoadTiles(Random.Range(minRoadTiles, maxRoadTiles + 1))
                    && (attemptIndex < MaxAttemptsToBuildMap - 1) //если так и не получается построить дорогу, то на последней попытке просто ставим конечный тайл на место последнего, который не смогли установить
               )
                return false;


            //конечный тайл
            _roadTiles.Add(Object.Instantiate(_roadFinishPrefab, _terrainParent.transform));
            _roadTiles[_roadTiles.Count - 1].transform.position = _currentTileCenter;
            FlipUntilConnected(_roadTiles[_roadTiles.Count - 1].transform, FindConnectionPoint(_roadTiles[_roadTiles.Count - 1].transform));

            return true;

        }

        private bool TryPlaceRoadTiles(int roadLength)
        {
            for (int i = 1; i < roadLength + 1; i++)// 1 и +1 потому что первый элемент списка уже есть
            {

                //перемешиваем префабы в массиве, чтобы они были под другими индексами.
                RandomizeGameObjectsArray(ref _roadTilesPrefabs);

                //ставим в качестве тайла нулл-геймобжект
                _roadTiles.Add(null);

                //пытаемся его заменить на нормальный тайл
                for (int j = 0; j < _roadTilesPrefabs.Length; j++)
                {
                    _roadTiles[i] = TryPlaceNextTile(_roadTilesPrefabs[j], i);

                    if (_roadTiles[i] != null)
                        break;
                }

                if (_roadTiles[i] == null)
                    return false;
                //если ни один префаб не подошёл, дорогу построить не удалось

            }

            return true;
        }


        /// <summary>
        /// Пробует установить и вернуть следующий тайл. Возвращает null, если установка тайла не удалась.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="currentTileNumber"></param>
        /// <returns></returns>
        private GameObject TryPlaceNextTile(GameObject prefab, int currentTileNumber)
        {
            Transform tile = InstantiateTileAtCurrentCenter(prefab);

            Transform[] connectionPoints = FindConnectionPoints(tile);

            int randomIndex = Random.Range(0, 2);

            FlipUntilConnected(tile, connectionPoints[randomIndex]);

            bool willBeBlocked = WillRunIntoOtherTiles(connectionPoints[1 - randomIndex].position, currentTileNumber);

            if (willBeBlocked)
            {
                FlipUntilConnected(tile, connectionPoints[1 - randomIndex]);

                willBeBlocked = WillRunIntoOtherTiles(connectionPoints[randomIndex].position, currentTileNumber);

                if (!willBeBlocked)
                    _currentConnectionPointPos = connectionPoints[randomIndex].position;
            }
            else
                _currentConnectionPointPos = connectionPoints[1 - randomIndex].position;

            //проверили обе точки. Если пересечение осталось, уничтожаем тайл и выходим из метода возвратом нулл.
            if (willBeBlocked)
            {
                Object.Destroy(tile.gameObject);
                return null;
            }

            _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);

            return tile.gameObject;

        }

        /// <summary>
        /// Возвращает истину, если следующий тайл, который будет установлен после текущего, будет пересекаться с уже установленным куском дороги.
        /// </summary>
        /// <param name="nextConnectionPoint"></param>
        /// <param name="currentTileNumber"></param>
        /// <returns></returns>
        private bool WillRunIntoOtherTiles(Vector3 nextConnectionPoint, int currentTileNumber)
        {
            bool RoadWillBeBlocked = false;

            for (int i = 0; i < currentTileNumber; i++)
            {
                if ((FindNextCenterPoint(nextConnectionPoint) - _roadTiles[i].transform.position).sqrMagnitude <= DistanceSqrMagnitude)
                {
                    RoadWillBeBlocked = true;
                    break;
                }
            }

            return RoadWillBeBlocked;
        }

        /// <summary>
        /// Инстантиейтит и возвращает тайл с центром в текущей позиции для центра тайла. Также поворачивает тайл в рандомное положение.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        private Transform InstantiateTileAtCurrentCenter(GameObject prefab)
        {
            Transform tile = Object.Instantiate(prefab, _terrainParent.transform).transform;
            tile.position = _currentTileCenter;
            tile.eulerAngles = new Vector3(0, Random.Range(0, TileMaxFlipTimes + 1) * TileFlipDegrees, 0);
            return tile;
        }

        /// <summary>
        /// Возвращает точку соединения дороги у данного тайла. Применяется для тайлов, у которых эта точка одна (начальный и конечный)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Transform FindConnectionPoint(Transform obj)
        {
            foreach (Transform child in obj)
                if (child.tag == TileConnectionPointObjTag)//TODO вынести в константу "Tile_ConnectionPoint"
                    return child.transform;

            return null;
        }

        /// <summary>
        /// Возвращает все точки соединения дороги у данного тайла.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Transform[] FindConnectionPoints(Transform obj)
        {
            Transform[] result = new Transform[2];
            int i = 0;

            foreach (Transform child in obj)
                if (child.tag == TileConnectionPointObjTag)
                    result[i++] = child.transform;

            return result;
        }

        /// <summary>
        /// Поворачивает тайл, пока точки соединения не совпадут. У connectionPoint передаётся Transform, а не Vector3, потому что передача Vector3 напрямую приводит к глюкам.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="connectionPoint"></param>
        private void FlipUntilConnected(Transform tile, Transform connectionPoint)
        {
            for (int i = 0; i <= TileMaxFlipTimes; i++)
            {
                if ((connectionPoint.position - _currentConnectionPointPos).sqrMagnitude <= DistanceSqrMagnitude)
                    break;

                tile.Rotate(0, TileFlipDegrees, 0);
            }
        }

        /// <summary>
        /// Возвращает центр следующего тайла.
        /// </summary>
        /// <param name="connectionPoint"></param>
        /// <returns></returns>
        private Vector3 FindNextCenterPoint(Vector3 connectionPoint)
        {
            return new Vector3()
            {
                x = _currentTileCenter.x + (connectionPoint.x - _currentTileCenter.x) * 2,
                z = _currentTileCenter.z + (connectionPoint.z - _currentTileCenter.z) * 2,
                y = 0
            };
        }

        /// <summary>
        /// Перемешивает содержимое массива гейм-обжектов.
        /// </summary>
        /// <param name="array"></param>
        private void RandomizeGameObjectsArray(ref GameObject[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = Random.Range(0, i + 1);
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }


        /// <summary>
        /// Ставит платформы под пушки (пока просто рандомно) на спавн-поинты расставленных тайлов дороги.
        /// </summary>
        private void PlaceTowerPlatforms(int towerPlatformsCount)
        {

            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(TileTowerPlatformSpawnPointObjTag);

            if (towerPlatformsCount > spawnPoints.Length)
                towerPlatformsCount = spawnPoints.Length;

            GameObject[] towerPlatforms = new GameObject[towerPlatformsCount];


            RandomizeGameObjectsArray(ref spawnPoints);

            for (int i = 0; i < towerPlatformsCount; i++)
            {
                towerPlatforms[i] = Object.Instantiate(_towerPlatform, spawnPoints[i].transform);
                towerPlatforms[i].transform.position = spawnPoints[i].transform.position;
                towerPlatforms[i].transform.rotation = Quaternion.identity;
            }

        }

        public void GenerateNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }

        //private void GenerateNavMesh()
        //{
        //    //если ранее по ходу выполнения скрипта применялся метод DestroyRoad()
        //    //, тайлы не успеют удалится до того как навмеш построится
        //    //, поэтому делаю отложенный вызов

        //    Invoke(TempGenNavMesh, NavMeshBuildDelay);
        //}

        //private void TempGenNavMesh()
        //{
        //    _navMeshSurface.BuildNavMesh();
        //}

        //private void Invoke(System.Action method, float time)
        //{
        //    Invoke(method.Method.Name, time);
        //}
    }

}
