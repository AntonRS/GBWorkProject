using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    [CreateAssetMenu(fileName = "NewRoadTileset", menuName = "Road Tileset")]
    public class TerrainTilesetData : ScriptableObject
    {

        [Tooltip("Платформа для строительства башни на ней")]
        public GameObject TowerPlatform;

        [Tooltip("Тайл, на котором спавнятся мобы")]
        public GameObject RoadStartPrefab;

        [Tooltip("Тайл, куда мобы пытаются добежать")]
        public GameObject RoadFinishPrefab;

        [Tooltip("Прямые тайлы дороги - т.е. если одна точка соединения слева, то другая будет справа")]
        public GameObject[] RoadStraightTilesPrefabs;

        [Tooltip("Поворотные тайлы дороги - т.е. если одна точка соединения слева, то другая будет сверху или снизу")]
        public GameObject[] RoadTurnTilesPrefabs;

        [Tooltip("Перекрёстки - три точки соединения")]
        public GameObject[] RoadCrossroadsWith3ExitsTilesPrefabs;

        [Tooltip("Перекрёстки - четыре точки соединения")]
        public GameObject[] RoadCrossroadsWith4ExitsTilesPrefabs;

    }

}