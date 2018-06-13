using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {


    //TODO: Подправить алгоритм выбора тайлов в TryPlaceNextTile, ибо в данный момент явная тенденция ставить левые повороты.

   [Header("Параметры генерации карты (с ними можно поиграться)")]

    [Tooltip("Количество платформ под башни")]
    public int TowerPlatformsCount;

    [Tooltip("Минимальная длина дороги, в тайлах")]
    public int MinRoadLength;

    [Tooltip("Максимальная длина дороги, в тайлах")]
    public int MaxRoadLength;

    private int _roadLengthCounter;

    
    [Header("Префабы тайлов (лучше не трогать)")]

    [SerializeField]
    private GameObject _towerPlatform;

    [SerializeField]
    private GameObject _roadStartPrefab;

    [SerializeField]
    private GameObject _roadFinishPrefab;

    [SerializeField]
    private GameObject[] _roadTilesPrefabs;
    
    private List<GameObject> _roadTiles;

    private Vector3 _currentConnectionPointPos;
    private Vector3 _currentTileCenter;


    //переменная для сравнения местополоджения двух объектов.
    // если sqrmagnitude вектора между ними меньше этого числа
    //, считаем, что они расположены в одной и той же точке пространства.
    private float _distanceSqrMagnitude = 1f;

    private int _maxAttemptsToBuildMap = 10;
    private bool _buildIsSuccessful;

    // Use this for initialization
    void Start () {

        _roadTiles = new List<GameObject>();
                
        //TODO: вынести это в отдельный метод и написать контроллер, который будет его вызывать
        for (int i = 0; i < _maxAttemptsToBuildMap; i++)
        {
            if (TryGenerateRoad())
            {
                _buildIsSuccessful = true;
                break;
            }

            Debug.Log("Не получилось построить карту. Осталось " + (_maxAttemptsToBuildMap - i - 1) + " попыток.");
                        
            DestroyRoad();
        }

        if (_buildIsSuccessful)
        {
            Debug.Log("Дорога успешно построена.");
            PlaceTowerPlatforms();
        }
        else
            Debug.Log("Не получилось построить карту. Попробуйте уменьшить её максимальную длину.");

    }

    private bool TryGenerateRoad()
    {
        _roadLengthCounter = Random.Range(MinRoadLength + 2, MaxRoadLength + 3);// +2 - потому что мы в тот же массив добавляем помимо остальных ещё два тайла - начальный и конечный.
        
        _currentTileCenter = transform.position;

        //устанавливаем начальный тайл и начальные точки
        _roadTiles.Add(InstantiateTileAtCurrentCenter(_roadStartPrefab).gameObject);
        _currentConnectionPointPos = FindConnectionPoint(_roadTiles[0].transform).position;

        //определяем, с какой стороны ставить следующий тайл, и переносим точку спавна следующего тайла
        _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);
        
        //дорога
        for (int i = 1; i < _roadLengthCounter - 1; i++)
        {

            RandomizePrefabsArray();

            _roadTiles.Add(null);

            for (int j = 0; j < _roadTilesPrefabs.Length; j++)
            {
                _roadTiles[i] = TryPlaceNextTile(_roadTilesPrefabs[j], i);

                if (_roadTiles[i] != null)
                    break;
            }

            //если ни один из тайлов не подошёл, дорогу построить не удалось - выходим, возвращаем фолс
            if (_roadTiles[i] == null)
                return false;

        }

        //конечный тайл
        _roadTiles.Add(Instantiate(_roadFinishPrefab, transform));
        _roadTiles[_roadTiles.Count - 1].transform.position = _currentTileCenter;

        Transform connectionPoint = FindConnectionPoint(_roadTiles[_roadTiles.Count - 1].transform);
        
        while ((connectionPoint.position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
            _roadTiles[_roadTiles.Count - 1].transform.Rotate(0, 90f, 0);

        //дорога готова, возвращаем тру
        return true;

    }

    private void DestroyRoad()
    {
        _roadTiles.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);
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

    //TODO: Проверить и дописать. Пока не используется
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

    //TODO: Написать, пока тут пусто
    private void PlaceTowerPlatforms()
    {

    }

}
