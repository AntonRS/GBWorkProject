﻿using Game.TerrainGeneration;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер генератора карты. В принципе, можно работать и напрямую с TerrainGenerator
/// </summary>
public class TerrainGeneratorController : MonoBehaviour
{

    [Tooltip("Количество платформ под башни")]
    public int TowerPlatformsCount;

    [Tooltip("Минимальное количество тайлов дороги")]
    public int MinRoadLength;

    [Tooltip("Максимальное количество тайлов дороги")]
    public int MaxRoadLength;

    [Tooltip("Объект в сцене, в котором будет создаваться дорога")]
    [SerializeField]
    private GameObject _generatorTerrainParent;

    [SerializeField]
    private TerrainTilesetData _tileset;

    private TerrainGenerator _generator;
    private List<GameObject> _roadTiles = new List<GameObject>();
    private TerrainNavMeshBuilder _navMesh;

    public GameObject Generator { get { return _generatorTerrainParent; } }
    public List<GameObject> RoadTiles { get { return _roadTiles; } }

    private Transform _enemiesSpawnPoint;
    private Transform _enemiesDestinationPoint;

    /// <summary>
    /// Точка спавна противников
    /// </summary>
    public Transform EnemiesSpawnPoint { get { return _enemiesSpawnPoint; } }

    /// <summary>
    /// Точка, до которой противники стараются добраться
    /// </summary>
    public Transform EnemiesDestinationPoint { get { return _enemiesDestinationPoint; } }

    private const float NavMeshBuildDelay = 0.1f;//пауза перед генерацией навмеша, чтобы тайлы успели удалиться перед собственно генерацией
    

    public float MapSouthernEdgeZ { get; private set; }
    public float MapWesternEdgeX { get; private set; }
    public float MapNorthernEdgeZ { get; private set; }
    public float MapEasternEdgeX { get; private set; }

    //ВРЕМЕННО, потом надо сшить через менеджер
    public delegate void TerrainGeneratorEvent();
    public TerrainGeneratorEvent TerrainGenerated;
    public TerrainGeneratorEvent TerrainDestroyed;

    // Use this for initialization
    void Start()
    {
        _generator =  new TerrainGenerator(_generatorTerrainParent);
        _generator.LoadTilePrefabs(_tileset);

        _navMesh = new TerrainNavMeshBuilder(_generatorTerrainParent);
    }

    public void GenerateTerrain()
    {
        if (_generatorTerrainParent.transform.childCount == 0)
            _roadTiles = _generator.GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);

        GenerateNavMesh();

        _enemiesSpawnPoint = _generator.GetEnemiesSpawnPoint();
        _enemiesDestinationPoint = _generator.GetEnemiesDestinationPoint();

        SetMapEdges();

        //временно, после связывания через менеджер убрать
        TerrainGenerated.Invoke();
    }

    public void DestroyTerrain()
    {
        _generator.DestroyTerrain();

        GenerateNavMesh();

        _roadTiles.Clear();

        _enemiesSpawnPoint = null;
        _enemiesDestinationPoint = null;

        SetMapEdges();

        //временно, после связывания через менеджер убрать
        TerrainDestroyed.Invoke();
    }

    private void GenerateNavMesh()
    {
        //если ранее в процессе генерации дороги какие-то её тайлы удалялись с помощью Destroy
        //, они могут не успеть исчезнуть до того как навмеш построится
        //, поэтому делаю отложенный вызов

        Invoke(TempGenNavMesh, NavMeshBuildDelay);
    }

    //System.Action не берётся из других классов, через точку - Unity так не умеет. Поэтому делаем такой дополнительный метод-"прокладку"
    private void TempGenNavMesh()
    {
        _navMesh.BuildNavMesh();
    }

    private void Invoke(System.Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }

    /// <summary>
    /// Устанавливает координаты крайних угловых точек карты и её центра.
    /// </summary>
    private void SetMapEdges()
    {
        float westEdgeX = 0;
        float eastEdgeX = 0;        
        float southEdgeZ = 0;
        float northEdgeZ = 0;

        if (_roadTiles.Count > 0)
        {
            foreach (var tile in _roadTiles)
            {
                Vector3 tilePosition = tile.transform.position;

                if (tilePosition.x < westEdgeX)
                    westEdgeX = tilePosition.x;

                if (tilePosition.x > eastEdgeX)
                    eastEdgeX = tilePosition.x;

                if (tilePosition.z < southEdgeZ)
                    southEdgeZ = tilePosition.z;

                if (tilePosition.z > northEdgeZ)
                    northEdgeZ = tilePosition.z;
            }
        }

        MapSouthernEdgeZ = southEdgeZ;
        MapWesternEdgeX = westEdgeX;
        MapNorthernEdgeZ = northEdgeZ;
        MapEasternEdgeX = eastEdgeX;
    }
}
