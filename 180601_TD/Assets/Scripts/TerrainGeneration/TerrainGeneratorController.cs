using Game.TerrainGeneration;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер генератора карты. В принципе, можно работать и напрямую с TerrainGenerator
/// </summary>
public class TerrainGeneratorController : MonoBehaviour
{
    //ДЛЯ ТЕСТА! УБРАТЬ!!!
    public GameObject cube;
    public GameObject sphere;




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

    // Use this for initialization
    void Start()
    {
        _generator =  new TerrainGenerator(_generatorTerrainParent);
        _generator.LoadTilePrefabs(_tileset);

        _navMesh = new TerrainNavMeshBuilder(_generatorTerrainParent);
    }

    //ВРЕМЕННО ДЛЯ ТЕСТА! ВЕРНУТЬ!!!
    //public void GenerateTerrain()
    //{
    //    if (_generatorTerrainParent.transform.childCount == 0)
    //        _roadTiles = _generator.GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);

    //    GenerateNavMesh();

    //    _enemiesSpawnPoint = _generator.GetEnemiesSpawnPoint();
    //    _enemiesDestinationPoint = _generator.GetEnemiesDestinationPoint();        
    //}

    //ВРЕМЕННО ДЛЯ ТЕСТА! УБРАТЬ!!!!!!
    public void GenerateTerrain()
    {
        LabyrinthGenerator labyrinth = new LabyrinthGenerator();
        LabyrinthPathBuilder pathBuilder = new LabyrinthPathBuilder();
        TilePlacer tilePlacer = new TilePlacer();
        LabyrinthPotentialStartFinishFinder endSquaresFinder = new LabyrinthPotentialStartFinishFinder();

        int[][] a;
        int[][] b;

        a = labyrinth.GenerateLabyrinth(20, 3, 15, 5, 4, new int[] { 0, 0 });
        tilePlacer.PlaceTiles(a, cube, _generatorTerrainParent.transform);

        b = endSquaresFinder.GetPotentialStartAndFinissSquares(a);

        tilePlacer.PlaceTiles(pathBuilder.BuildPathInLabyrinth(20, a, b), sphere, _generatorTerrainParent.transform);
    }

    public void DestroyTerrain()
    {
        _generator.DestroyTerrain();

        GenerateNavMesh();

        _roadTiles.Clear();

        _enemiesSpawnPoint = null;
        _enemiesDestinationPoint = null;        
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



}
