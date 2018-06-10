using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {


    //TODO: Подправить алгоритм выбора тайлов в TryPlaceNextTile, ибо в данный момент явная тенденция ставить левые повороты.
    //TODO: Поправить переделку карты при неполучающейся дороге. Возможно, проблема из-за переопределения массива. Попробовать поменять на List?

    public int TowerPlatformsCount;
    public int MinRoadLength;
    public int MaxRoadLength;

    private int _roadLengthCounter;

    [SerializeField]
    private GameObject _towerPlatform;

    [SerializeField]
    private GameObject _roadStartPrefab;

    [SerializeField]
    private GameObject _roadFinishPrefab;

    [SerializeField]
    private GameObject[] _roadTilesPrefabs;
    
    private GameObject[] _roadTiles;

    private Vector3 _currentConnectionPointPos;
    private Vector3 _currentTileCenter;

    private float _distanceSqrMagnitude = 1f;

    private int _maxAttemptsToBuildMap = 10;

    // Use this for initialization
    void Start () {

        GenerateRoad();
        PlaceTowerPlatforms();

    }

    private void GenerateRoad()
    {
        _roadLengthCounter = Random.Range(MinRoadLength + 2, MaxRoadLength + 3);// +2 - потому что мы в тот же массив добавляем помимо остальных ещё два тайла - начальный и конечный.
        _roadTiles = new GameObject[_roadLengthCounter];

        _currentTileCenter = transform.position;

        //устанавливаем начальный тайл и начальные точки
        _roadTiles[0] = InstantiateTileAtCurrentCenter(_roadStartPrefab).gameObject;
        _currentConnectionPointPos = FindConnectionPoint(_roadTiles[0].transform).position;

        //определяем, с какой стороны ставить следующий тайл, и переносим точку спавна следующего тайла
        _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);

        bool roadWasRemade = false;
        //дорога
        for (int i = 1; i < _roadLengthCounter - 1; i++)
        {

            RandomizePrefabsArray();

            for (int j = 0; j < _roadTilesPrefabs.Length; j++)
            {
                _roadTiles[i] = TryPlaceNextTile(_roadTilesPrefabs[j], i);

                if (_roadTiles[i] != null)
                    break;
            }

            if (_roadTiles[i] == null)
            {
                roadWasRemade = true;
                DestroyRoadAndGenerateAnew();
            }

        }

        if (roadWasRemade)
            return;

        //конечный тайл
        _roadTiles[_roadLengthCounter - 1] = Instantiate(_roadFinishPrefab, transform);
        _roadTiles[_roadLengthCounter - 1].transform.position = _currentTileCenter;

        Transform connectionPoint = FindConnectionPoint(_roadTiles[_roadLengthCounter - 1].transform);
        
        while ((connectionPoint.position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
            _roadTiles[_roadLengthCounter - 1].transform.Rotate(0, 90f, 0);
       

    }

    private void DestroyRoadAndGenerateAnew()
    {
        //Debug.Log("количество тайлов - " + _roadTiles.Length);
        //for (int i = 0; i < _roadTiles.Length; i++)
        //{
        //    if (_roadTiles[i] != null)
        //    {
        //        Destroy(_roadTiles[i]);
        //        Debug.Log("уничтожен тайл номер" + i);
        //    }
        //}

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _maxAttemptsToBuildMap--;
        Debug.Log("Не получилось построить карту. Осталось" + _maxAttemptsToBuildMap + " попыток.");
        Debug.Log("Метод удаления недостроенной дороги в данный момент работает некорректно, перезапустите игру");

        if (_maxAttemptsToBuildMap < 0)
            Debug.Log("Не получилось построить карту. Попробуйте уменьшить максимальную длину.");
        else
            GenerateRoad();

    }

    private GameObject TryPlaceNextTile(GameObject prefab, int currentTileNumber)
    {
        Transform tile = InstantiateTileAtCurrentCenter(prefab);

        Transform[] connectionPoints = FindConnectionPoints(tile);
                
        while ((connectionPoints[0].position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
            tile.Rotate(0, 90f, 0);

        bool willBeBlocked = WillRunIntoOtherTiles(connectionPoints[1].position, currentTileNumber);

        if (willBeBlocked)
        {

            while ((connectionPoints[1].position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
                tile.Rotate(0, 90f, 0);

            willBeBlocked = WillRunIntoOtherTiles(connectionPoints[0].position, currentTileNumber);

            if (!willBeBlocked)
                _currentConnectionPointPos = connectionPoints[0].position;

        }
        else
            _currentConnectionPointPos = connectionPoints[1].position;

        //проверили обе точки. Если пересечение осталось, уничтожаем тайл и выходим из метода возвратом нулл.
        if (willBeBlocked)
        {
            Destroy(tile.gameObject);
            return null;
        }
        

        _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);

        return tile.gameObject;

    }

    private Transform InstantiateTileAtCurrentCenter(GameObject prefab)
    {
        Transform tile = Instantiate(prefab, transform).transform;
        tile.position = _currentTileCenter;
        tile.eulerAngles = new Vector3(0, Random.Range(0, 4) * 90f, 0);
        return tile;
    }

    private Transform FindConnectionPoint(Transform obj)
    {
        foreach (Transform child in obj)
            if (child.tag == "Tile_ConnectionPoint")
                return child.transform;

        return null;
    }

    private Transform[] FindConnectionPoints(Transform obj)
    {
        Transform[] result = new Transform[2];
        int i = 0;

        foreach (Transform child in obj)
            if (child.tag == "Tile_ConnectionPoint")
            {
                result[i] = child.transform;
                i++;
            }

        return result;
    }

    //TODO: Проверить и дописать
    private void FlipUntilConnected(GameObject tile, Vector3 connectionPoint)
    {
        while ((connectionPoint - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
           tile.transform.Rotate(0, 90f, 0);
    }

    private Vector3 FindNextCenterPoint(Vector3 connectionPoint)
    {
        Vector3 result = new Vector3();
        result.x = _currentTileCenter.x + (connectionPoint.x - _currentTileCenter.x) * 2;
        result.z = _currentTileCenter.z + (connectionPoint.z - _currentTileCenter.z) * 2;
        result.y = 0;
        return result;
    }

    private bool WillRunIntoOtherTiles(Vector3 nextConnectionPoint, int currentTileNumber)
    {
        bool RoadWillBeBlocked = false;

        for (int i = 0; i < currentTileNumber; i++)
        {
            if ((FindNextCenterPoint(nextConnectionPoint) - _roadTiles[i].transform.position).sqrMagnitude <= _distanceSqrMagnitude)
            {
                RoadWillBeBlocked = true;
                break;
            }
        }

        return RoadWillBeBlocked;
    }
    
    private void RandomizePrefabsArray()
    {
        for (int i = 0; i < _roadTilesPrefabs.Length; i++)
        {
            int j = Random.Range(0, i + 1);
            var temp = _roadTilesPrefabs[j];
            _roadTilesPrefabs[j] = _roadTilesPrefabs[i];
            _roadTilesPrefabs[i] = temp;
        }
    }

    private void PlaceTowerPlatforms()
    {

    }

}
