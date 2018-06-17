using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    [Header("Параметры генерации карты - можно с ними поиграться")]

    [Tooltip("Количество платформ под башни")]
    public int TowerPlatformsCount;

    [Tooltip("Минимальное количество тайлов дороги")]
    public int MinRoadLength;

    [Tooltip("Максимальное количество тайлов дороги")]
    public int MaxRoadLength;

    private int _roadLengthCounter;


    [Header("Префабы для тайлов - лучше не трогать")]

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

    private float _distanceSqrMagnitude = 1f;//число для сравнения местоположения. Если (точкаА - точкаБ).sqrMagnitude < _distanceSqrMagnitude, считаем, что эти точки находятся в одном и том же месте.

    private int _maxAttemptsToBuildMap = 10;//предел для количества попыток построить карту. Просто на всякий случай, чтобы не попасть в бесконечный цикл.
    private bool _buildIsSuccessful;

    // Use this for initialization
    void Start () {

        _roadTiles = new List<GameObject>();

        //временно, потом вызов метода перенесу в контроллер
        //не забыть и эти переменные тоже туда перенести и в этом скрипте сделать приватными или внутриметодными
        GenerateTerrain(MinRoadLength, MaxRoadLength, TowerPlatformsCount);

    }

    /// <summary>
    /// Генерирует карту рандомной длины в заданных рамках.
    /// </summary>
    /// <param name="minRoadTiles">Минимальное количество тайлов дороги, не считая стартового и конечного.</param>
    /// <param name="maxRoadTiles">Максимальное количество тайлов дороги, не считая стартового и конечного.</param>
    /// <param name="towerPlatformsCount">Количество платформ под башни.</param>
    public void GenerateTerrain(int minRoadTiles, int maxRoadTiles, int towerPlatformsCount)
    {
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


    /// <summary>
    /// Пробует сгенерировать дорогу. Возвращает истину, если построить дорогу удалось, ложь - если нет.
    /// </summary>
    /// <returns></returns>
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

            //перемешиваем префабы в массиве, чтобы они были под другими индексами.
            RandomizeGameObjectsArray(ref _roadTilesPrefabs);

            //ставим в качестве тайла нулл-геймобжект
            _roadTiles.Add(null);

            //пытаемся его заменить на нормальный тайл
            for (int j = 0; j < _roadTilesPrefabs.Length; j++)
            {
                _roadTiles[i] = TryPlaceNextTile(_roadTilesPrefabs[j], i);

                if (_roadTiles[i] != null)
                    break;
            }

            if (_roadTiles[i] == null)
                return false;
            //если ни один префаб не подошёл, дорогу построить не удалось

        }

        //конечный тайл
        _roadTiles.Add(Instantiate(_roadFinishPrefab, transform));
        _roadTiles[_roadTiles.Count - 1].transform.position = _currentTileCenter;

        Transform connectionPoint = FindConnectionPoint(_roadTiles[_roadTiles.Count - 1].transform);
        
        while ((connectionPoint.position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
            _roadTiles[_roadTiles.Count - 1].transform.Rotate(0, 90f, 0);

        return true;

    }

    /// <summary>
    /// Уничтожает дорогу и вычищает список уже установленных тайлов.
    /// </summary>
    private void DestroyRoad()
    {
        _roadTiles.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
    
    /// <summary>
    /// Пробует установить и вернуть следующий тайл. Возвращает null, если установка тайла не удалась.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="currentTileNumber"></param>
    /// <returns></returns>
    private GameObject TryPlaceNextTile(GameObject prefab, int currentTileNumber)
    {
        Transform tile = InstantiateTileAtCurrentCenter(prefab);

        Transform[] connectionPoints = FindConnectionPoints(tile);

        int randomIndex = Random.Range(0, 2);
                
        while ((connectionPoints[randomIndex].position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
            tile.Rotate(0, 90f, 0);

        bool willBeBlocked = WillRunIntoOtherTiles(connectionPoints[1 - randomIndex].position, currentTileNumber);

        if (willBeBlocked)
        {

            while ((connectionPoints[1 - randomIndex].position - _currentConnectionPointPos).sqrMagnitude > _distanceSqrMagnitude)
                tile.Rotate(0, 90f, 0);

            willBeBlocked = WillRunIntoOtherTiles(connectionPoints[randomIndex].position, currentTileNumber);

            if (!willBeBlocked)
                _currentConnectionPointPos = connectionPoints[randomIndex].position;

        }
        else
            _currentConnectionPointPos = connectionPoints[1 - randomIndex].position;

        //проверили обе точки. Если пересечение осталось, уничтожаем тайл и выходим из метода возвратом нулл.
        if (willBeBlocked)
        {
            Destroy(tile.gameObject);
            return null;
        }
        

        _currentTileCenter = FindNextCenterPoint(_currentConnectionPointPos);

        return tile.gameObject;

    }

    /// <summary>
    /// Возвращает истину, если следующий тайл, который будет установлен после текущего, будет пересекаться с уже установленным куском дороги.
    /// </summary>
    /// <param name="nextConnectionPoint"></param>
    /// <param name="currentTileNumber"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Инстантиейтит и возвращает тайл с центром в текущей позиции для центра тайла. Также поворачивает тайл в рандомное положение.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private Transform InstantiateTileAtCurrentCenter(GameObject prefab)
    {
        Transform tile = Instantiate(prefab, transform).transform;
        tile.position = _currentTileCenter;
        tile.eulerAngles = new Vector3(0, Random.Range(0, 4) * 90f, 0);
        return tile;
    }

    /// <summary>
    /// Возвращает точку соединения дороги у данного тайла. Применяется для тайлов, у которых эта точка одна (начальный и конечный)
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private Transform FindConnectionPoint(Transform obj)
    {
        foreach (Transform child in obj)
            if (child.tag == "Tile_ConnectionPoint")
                return child.transform;

        return null;
    }

    /// <summary>
    /// Возвращает все точки соединения дороги у данного тайла.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Возвращает центр следующего тайла.
    /// </summary>
    /// <param name="connectionPoint"></param>
    /// <returns></returns>
    private Vector3 FindNextCenterPoint(Vector3 connectionPoint)
    {
        Vector3 result = new Vector3();
        result.x = _currentTileCenter.x + (connectionPoint.x - _currentTileCenter.x) * 2;
        result.z = _currentTileCenter.z + (connectionPoint.z - _currentTileCenter.z) * 2;
        result.y = 0;
        return result;
    }
    
    /// <summary>
    /// Перемешивает содержимое массива гейм-обжектов.
    /// </summary>
    /// <param name="array"></param>
    private void RandomizeGameObjectsArray(ref GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(0, i + 1);
            var temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }


    /// <summary>
    /// Ставит платформы под пушки (пока просто рандомно) на спавн-поинты расставленных тайлов дороги.
    /// </summary>
    private void PlaceTowerPlatforms()
    {
        
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Tile_TowerPlatformSpawnPoint");

        if (TowerPlatformsCount > spawnPoints.Length)
            TowerPlatformsCount = spawnPoints.Length;

        GameObject[] towerPlatforms = new GameObject[TowerPlatformsCount];


        RandomizeGameObjectsArray(ref spawnPoints);

        for (int i = 0; i < TowerPlatformsCount; i++)
        {
            towerPlatforms[i] = Instantiate(_towerPlatform, spawnPoints[i].transform);
            towerPlatforms[i].transform.position = spawnPoints[i].transform.position;
            towerPlatforms[i].transform.rotation = Quaternion.identity;
        }

    }

}
