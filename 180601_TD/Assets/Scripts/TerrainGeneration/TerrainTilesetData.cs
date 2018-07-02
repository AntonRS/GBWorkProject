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

        [Tooltip("Остальные, промежуточные тайлы дороги")]
        public GameObject[] RoadTilesPrefabs;

    }

}