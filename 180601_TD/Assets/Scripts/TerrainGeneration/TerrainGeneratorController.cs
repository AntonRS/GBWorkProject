using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TerrainGeneration;

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


    private TerrainGenerator _generator;
    private List<GameObject> _roadTiles = new List<GameObject>();

    public GameObject Generator { get { return _generator.gameObject; } }
    public List<GameObject> RoadTiles { get { return _roadTiles; } }

    // Use this for initialization
    void Start()
    {
        _generator = FindObjectOfType<TerrainGenerator>();
    }

    public void GenerateTerrain()
    {
        if (_generator.transform.childCount == 0)
            _roadTiles = _generator.GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);        
    }

    public void DestroyTerrain()
    {
        _generator.DestroyTerrain();
        _roadTiles.Clear();
    }

    
    

}
