using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Enemy;
using Game.Towers;
using Game.TerrainGeneration;
namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("UI pannels")]
        public GameObject mainMenuPannel;
        public GameObject gameBar;
        [Header("Textfields")]
        public Text countdownText;
        public Text livesText;
        public Text moneytext;
        [Header("Game params")]
        public int minEnemiesCountInWave;
        public int maxEnemiesCountInWave;
        public int hpModPercent;
        public float timeBetweenWaves;
        [Header("Players parars")]
        public int startLives;
        public int startMoney;

        private float _countdown;
        private int _waveIndex = 0;
        private bool _isCountingDown = false;
        private int _currentLives;
        private int _currentMoney;

        public TowersManager GetTowersManager { get; private set; }
        public EnemiesController GetEnemiesController { get; private set; }
        public TerrainGenerator GetTerrainGenerator { get; private set; }

        private void Awake()
        {
            GetTowersManager = FindObjectOfType<TowersManager>();
            GetEnemiesController = FindObjectOfType<EnemiesController>();
            GetTerrainGenerator = FindObjectOfType<TerrainGenerator>();
        }
        

        private void Update()
        {
            if (_isCountingDown)
            {
                countdownText.text = ((int)_countdown).ToString();
                _countdown -= Time.deltaTime;
                if (_countdown <= 0 )
                {
                    foreach (var spawner in GetEnemiesController.spawners)
                    {
                        spawner.SpawnRandomWave(_waveIndex, minEnemiesCountInWave, maxEnemiesCountInWave, hpModPercent);
                        _waveIndex += 1;
                        _countdown = timeBetweenWaves;

                    }
                }
            }
        }
        public void Pause()
        {
            Time.timeScale = 0;
           
        }
        public void Resume()
        {
            Time.timeScale = 1;
            
        }
        public void FinishGame()
        {
            TerrainGeneratorController.Instance.DestroyTerrain();
            _waveIndex = 0;
            GetEnemiesController.ClearEnemyList();
            GetEnemiesController.ClearSpawnerList();
            _currentMoney = startMoney;
            _currentLives = startLives;
            _isCountingDown = false;
            gameBar.SetActive(false);
            mainMenuPannel.SetActive(true);
        }
        public void StartGame()
        {
            TerrainGeneratorController.Instance.GenerateTerrain();
            _currentMoney = startMoney;
            _currentLives = startLives;
            _countdown = timeBetweenWaves;
            countdownText.text += ((int)_countdown).ToString();
            livesText.text = _currentLives.ToString();
            moneytext.text = _currentMoney.ToString();
            _isCountingDown = true;
            mainMenuPannel.SetActive(false);
            gameBar.SetActive(true);

        }
    }
}


