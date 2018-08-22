using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{
    public class TilePlacer
    {
        //пока делаем прямую дорогу, без перекрестков. Их планирую добавить потом - т.е. отдельный скрипт (или перегрузка этого) будет присоединять ещё дорогу к уже имеющейся.

        private const float TileFlipDegrees = 90f;//на сколько градусов поворачиваем тайл
        private const int TileMaxFlipTimes = 3;//сколько раз максимум поворачиваем тайл
        private const string TileConnectionPointObjTag = "Tile_ConnectionPoint";
        private const float MinDistanceFromCenterToConnectionPoint = 0.2f;
        private const float DistanceSqrMagnitude = 1f;//число для сравнения местоположения. Если (точкаА - точкаБ).sqrMagnitude < _distanceSqrMagnitude, считаем, что эти точки находятся в одном и том же месте.


        //Тестовый метод, заполняющий указанные клетки поданными на вход объектами.
        public void PlaceGameObjectsAtCoordinates(int[][] arrayOfCoordinates, GameObject objectToPlace, Transform terrainParent)
        {
            foreach (var point in arrayOfCoordinates)
                MonoBehaviour.Instantiate(objectToPlace, new Vector3(point[0], 0, point[1]),Quaternion.identity);
        }

        /// <summary>
        /// Расставляет тайлы дороги по указанным координатам. Тайлы выбираются случайным образом из подходящих. Первый элемент массива координат - старт, последний - финиш.
        /// </summary>
        /// <param name="arrayOfCoordinates"></param>
        /// <param name="tileset"></param>
        public List<GameObject> PlaceTiles(int[][] arrayOfCoordinates, TerrainTilesetData tileset, Transform terrainParent)
        {
            //список, в который кладутся тайлы дороги. Первый элемент списка - стартовый тайл, последний - финишный.
            List<GameObject> roadTiles = new List<GameObject>();

            //два служебных гейм-обжекта для соединения тайлов дороги между собой.
            Transform[] connectionCheckPoints = new Transform[] { new GameObject().transform, new GameObject().transform };
            foreach (var point in connectionCheckPoints)
                point.parent = terrainParent;

            //ширина тайла. Скрипт работает только если все тайлы одинакового размера.
            float tileSize = GetTileSize(tileset.RoadStartPrefab.transform);

            //коррекция координат, чтобы карта строилась от центра terrainParent.
            float[] XYcorrection = new float[] { terrainParent.position.x - arrayOfCoordinates[0][0], terrainParent.position.z - arrayOfCoordinates[0][1] };
            

            //сначала ставим стартовый тайл, потом - в цикле - со второго по предпоследний, потом - финишный тайл, отсюда все эти цифры (1, -2 и т.п.).

            roadTiles.Add(PlaceStartOrFinishTile(arrayOfCoordinates[0], arrayOfCoordinates[1], tileset.RoadStartPrefab, terrainParent, XYcorrection, tileSize, connectionCheckPoints));

            for (int i = 1; i < arrayOfCoordinates.Length - 1; i++)
                roadTiles.Add(PlaceRoadTile(arrayOfCoordinates[i], arrayOfCoordinates[i-1], arrayOfCoordinates[i+1],tileset, terrainParent, XYcorrection, tileSize, connectionCheckPoints));

            roadTiles.Add(PlaceStartOrFinishTile(arrayOfCoordinates[arrayOfCoordinates.Length - 1], arrayOfCoordinates[arrayOfCoordinates.Length - 2], tileset.RoadFinishPrefab, terrainParent, XYcorrection, tileSize, connectionCheckPoints));

            //удаляем служеюные гейм-обжекты.
            foreach (var point in connectionCheckPoints)
                MonoBehaviour.Destroy(point.gameObject);

            //возвращаем список тайлов дороги.
            return roadTiles;
        }

        private float GetTileSize(Transform prefab)
        {
            Transform connectionPoint = FindConnectionPoint(prefab);
            return (prefab.position - new Vector3(connectionPoint.position.x, prefab.position.y, connectionPoint.position.z)).magnitude * 2;//ширина тайла равно расстояние от центра до края, помноженное на два
        }

        private GameObject PlaceStartOrFinishTile(int[] currentTileXY, int[] neighbouringTileXY, GameObject prefab, Transform terrainParent, float[] XYcorrection, float tileSize, Transform[] connectionCheckPoints)
        {
            Transform tile = InstantiateTile(prefab, currentTileXY, terrainParent, XYcorrection, tileSize);
            
            Transform connectionPoint = FindConnectionPoint(tile);



            //Сначала устанавливаем координаты целевой точки для точки соединения, в зависимости от положения соседнего тайла, 
            //Потом вращаем тайл, пока его коннекшн-пойнт с этой целевой точкой не совпадёт.

            SetConnectionPointTargetPosition(currentTileXY, neighbouringTileXY, tile, connectionCheckPoints[0], connectionPoint.position.y, tileSize / 2);

            for (int i = 0; i < TileMaxFlipTimes; i++)
            {
                if ((connectionPoint.position - connectionCheckPoints[0].position).sqrMagnitude <= DistanceSqrMagnitude)
                    break;

                tile.Rotate(0, TileFlipDegrees, 0);
            }

            return tile.gameObject;

        }

        private GameObject PlaceRoadTile(int[] currentTileXY, int[] previousTileXY, int[] nextTileXY, TerrainTilesetData tileset, Transform terrainParent, float[] correctionXY, float tileSize, Transform[] connectionCheckPoints)
        {
            Transform tile;

            if (nextTileXY[0] == previousTileXY[0] || nextTileXY[1] == previousTileXY[1]) //это значит, что текущий тайл дороги должен быть "прямой", а не поворотный
            {
                tile = InstantiateTile(tileset.RoadStraightTilesPrefabs[Random.Range(0, tileset.RoadStraightTilesPrefabs.Length)], currentTileXY, terrainParent, correctionXY, tileSize);
                tile.eulerAngles = new Vector3(0, Random.Range(0, TileMaxFlipTimes + 1) * TileFlipDegrees, 0);//повернуть на случай несимметричных тайлов (зигзаги, например)
            }
            else //а здесь поворотный тайл
                tile = InstantiateTile(tileset.RoadTurnTilesPrefabs[Random.Range(0, tileset.RoadTurnTilesPrefabs.Length)], currentTileXY, terrainParent, correctionXY, tileSize);

            Transform[] connectionPoints = FindConnectionPoints(tile);


            //То же самое, что и в PlaceStartOrFinishTile(), только тут две целевые точки и две точки соединения.

            SetConnectionPointTargetPosition(currentTileXY, previousTileXY, tile, connectionCheckPoints[0], connectionPoints[0].position.y, tileSize / 2);
            SetConnectionPointTargetPosition(currentTileXY, nextTileXY, tile, connectionCheckPoints[1], connectionPoints[1].position.y, tileSize / 2);

            for (int i = 0; i < TileMaxFlipTimes; i++)
            {
                if  (
                        (
                            (connectionPoints[0].position - connectionCheckPoints[0].position).sqrMagnitude <= DistanceSqrMagnitude
                            &&
                            (connectionPoints[1].position - connectionCheckPoints[1].position).sqrMagnitude <= DistanceSqrMagnitude
                        )
                        ||
                        (
                            (connectionPoints[0].position - connectionCheckPoints[1].position).sqrMagnitude <= DistanceSqrMagnitude
                            &&
                            (connectionPoints[1].position - connectionCheckPoints[0].position).sqrMagnitude <= DistanceSqrMagnitude
                        )
                    )
                    break;

                tile.Rotate(0, TileFlipDegrees, 0);
            }

            return tile.gameObject;
        }


        /// <summary>
        /// Создаёт гейм-обжект из заданного префаба, а также поворачивает его случайным образом. Пока оставляю рандомный поворот на случай несимметричных тайлов (зигзаги например). И для 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="coordinates"></param>
        /// <param name="terrainParent"></param>
        /// <param name="correctionXY"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        private Transform InstantiateTile(GameObject prefab, int[] coordinates, Transform terrainParent, float[] correctionXY, float tileSize)
        {
            return Object.Instantiate(prefab, new Vector3((coordinates[0] + correctionXY[0]) * tileSize, terrainParent.position.y, (coordinates[1] + correctionXY[1]) * tileSize), Quaternion.identity, terrainParent).transform;
        }

        /// <summary>
        /// Возвращает точку соединения дороги у данного тайла. Применяется для тайлов, у которых эта точка одна (начальный и конечный)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Transform FindConnectionPoint(Transform obj)
        {
            foreach (Transform child in obj)
                if (child.tag == TileConnectionPointObjTag)
                    return child.transform;

            return null;
        }

        /// <summary>
        /// Возвращает все точки соединения дороги у данного тайла. Рассчитан на "не-перекрестки", т.е. дороги с двумя точками коннекта.
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
        /// Устанавливает координаты точки для последующей проверки соединения с соседним тайлом.
        /// </summary>
        /// <param name="currentTileXY"></param>
        /// <param name="neighbouringTileXY"></param>
        /// <param name="currentTile"></param>
        /// <param name="connectionPointTarget"></param>
        /// <param name="connectionPointYValue"></param>
        /// <param name="distanceTileCenterToConnectionPoint"></param>
        private void SetConnectionPointTargetPosition(int[] currentTileXY, int[] neighbouringTileXY, Transform currentTile, Transform connectionPointTarget, float connectionPointYValue, float distanceTileCenterToConnectionPoint)
        {
            if (neighbouringTileXY[0] > currentTileXY[0])
                connectionPointTarget.position = new Vector3(currentTile.position.x + distanceTileCenterToConnectionPoint, connectionPointYValue, currentTile.position.z);
            else if (neighbouringTileXY[0] < currentTileXY[0])
                connectionPointTarget.position = new Vector3(currentTile.position.x - distanceTileCenterToConnectionPoint, connectionPointYValue, currentTile.position.z);
            else if (neighbouringTileXY[1] > currentTileXY[1])
                connectionPointTarget.position = new Vector3(currentTile.position.x, connectionPointYValue, currentTile.position.z + distanceTileCenterToConnectionPoint);
            else if (neighbouringTileXY[1] < currentTileXY[1])
                connectionPointTarget.position = new Vector3(currentTile.position.x, connectionPointYValue, currentTile.position.z - distanceTileCenterToConnectionPoint);
        }


    }

}