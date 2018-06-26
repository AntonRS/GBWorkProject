using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TerrainGeneration;

public class TerrainGeneratorController : MonoBehaviour
{

    [Tooltip("Количество платформ под башни")]
    public int TowerPlatformsCount;

    [Tooltip("Минимальное количество тайлов дороги")]
    public int MinRoadLength;

    [Tooltip("Максимальное количество тайлов дороги")]
    public int MaxRoadLength;


    private TerrainGenerator _generator;

    // Use this for initialization
    void Start()
    {
        _generator = FindObjectOfType<TerrainGenerator>();
        _generator.Init();
    }

    public void GenerateTerrain()
    {
        if (_generator.transform.childCount == 0)
            _generator.GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);        
    }

    public void DestroyTerrain()
    {
            _generator.DestroyTerrain();
    }

    private void SetBoundsUsingCameraFOV()
    {
        Camera camera = FindObjectOfType<Camera>();
    }

    private void SetBoundsUsingPoints()
    {

    }

}
