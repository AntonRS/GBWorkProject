using Game.TerrainGeneration;
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

    public GameObject Generator { get { return _generatorTerrainParent; } }
    public List<GameObject> RoadTiles { get { return _roadTiles; } }


    private const float NavMeshBuildDelay = 0.1f;//пауза перед генерацией навмеша, чтобы тайлы успели удалиться перед собственно генерацией

    // Use this for initialization
    void Start()
    {
        _generator =  new TerrainGenerator(_generatorTerrainParent);
        _generator.LoadTilePrefabs(_tileset);
    }

    public void GenerateTerrain()
    {
        if (_generatorTerrainParent.transform.childCount == 0)
            _roadTiles = _generator.GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);

        GenerateNavMesh();
    }

    public void DestroyTerrain()
    {
        _generator.DestroyTerrain();

        GenerateNavMesh();

        _roadTiles.Clear();
    }

    private void GenerateNavMesh()
    {
        //если ранее по ходу выполнения скрипта применялся метод DestroyRoad()
        //, тайлы не успеют удалится до того как навмеш построится
        //, поэтому делаю отложенный вызов

        Invoke(TempGenNavMesh, NavMeshBuildDelay);
    }

    private void TempGenNavMesh()
    {
        _generator.GenerateNavMesh();
    }

    private void Invoke(System.Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }



}
