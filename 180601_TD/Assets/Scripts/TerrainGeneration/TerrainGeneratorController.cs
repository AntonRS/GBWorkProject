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
    private TerrainNavMeshBuilder _navMesh;

    public GameObject Generator { get { return _generatorTerrainParent; } }
    public List<GameObject> RoadTiles { get { return _roadTiles; } }


    private const float NavMeshBuildDelay = 0.1f;//пауза перед генерацией навмеша, чтобы тайлы успели удалиться перед собственно генерацией

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
    }

    public void DestroyTerrain()
    {
        _generator.DestroyTerrain();

        GenerateNavMesh();

        _roadTiles.Clear();
    }

    private void GenerateNavMesh()
    {
        //если ранее в процессе генерации дороги какие-то её тайлы удалялись с помощью Destroy
        //, они могут не успеть исчезнуть до того как навмеш построится
        //, поэтому делаю отложенный вызов

        Invoke(TempGenNavMesh, NavMeshBuildDelay);
    }

    private void TempGenNavMesh()
    {
        _navMesh.BuildNavMesh();
    }

    private void Invoke(System.Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }



}
