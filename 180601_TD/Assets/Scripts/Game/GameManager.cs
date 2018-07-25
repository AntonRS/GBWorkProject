using UnityEngine;
using UnityEngine.UI;
using Game.Enemy;
using Game.Towers;
namespace Game
{
    /// <summary>
    /// Содержит логику механики игры.
    /// Является связующим элементом для других модулей.
    /// Наследуется от Singleton.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {       
        [Header("UI pannels")]
        /// <summary>
        /// Ссылка на панель с игровым меню.
        /// </summary>
        public GameObject mainMenuPannel;
        /// <summary>
        /// Ссылка на gameBar.
        /// </summary>
        public GameObject gameBar;

        /// <summary>
        /// Ссылка на текстовое поле, отображающее отсчет времени.
        /// </summary>
        [Header("Textfields")]
        public Text countdownText;
        /// <summary>
        /// Ссылка на текстовое поле, содержащие количество жизней.
        /// </summary>
        public Text livesText;
        /// <summary>
        /// Ссылка на текстовое поле, содержащее количество деньги.
        /// </summary>
        public Text moneytext;
        /// <summary>
        /// Максимальное количство врагов в волне.
        /// </summary>
        [Tooltip("Максимальное количство врагов в волне.")]
        [Header("Game params")]
        public int maxEnemiesCountInWave;
        /// <summary>
        /// Минимальное количство врагов в волне.
        /// </summary>
        [Tooltip("Минимальное количство врагов в волне.")]
        public int minEnemiesCountInWave;
        /// <summary>
        /// Коэфициент увеличения жизней и ценности врага.
        /// </summary>
        [Tooltip("Коэфициент увеличения жизней и ценности врага.")]
        public int ModPercent;
        /// <summary>
        /// Время между волнами.
        /// </summary>
        [Tooltip("Время между волнами.")]
        [SerializeField] private int _timeBetweenWaves;
        /// <summary>
        /// Начальные жизни.
        /// </summary>
        [Header("Players params")]
        [SerializeField]private int _startLives;
        /// <summary>
        /// Начальные деньги.
        /// </summary>
        [SerializeField] private int _startMoney;
        /// <summary>
        /// Текущее время отсчета.
        /// </summary>
        private int _countdown;
        /// <summary>
        /// Текущий номер волны.
        /// </summary>
        private int _waveIndex = 0;
        /// <summary>
        /// Текущие жизни.
        /// </summary>
        private int _currentLives;
        /// <summary>
        /// Текущие деньги.
        /// </summary>
        public int CurrentMoney { get; private set; }
        /// <summary>
        /// Ссылка на TowersManager.
        /// </summary>
        public TowersManager GetTowersManager { get; private set; }
        /// <summary>
        /// Ссылка на EnemiesController.
        /// </summary>
        public EnemiesController GetEnemiesController { get; private set; }
        /// <summary>
        /// Ссылка на TerraiGenerator.
        /// </summary>
        public TerrainGeneratorController GetTerrainGenerator { get; private set; }
        /// <summary>
        /// Делегат Action.
        /// </summary>
        private delegate void Action();

        #region Unity Functions
        private void Awake()
        {
            GetTowersManager = FindObjectOfType<TowersManager>();
            GetEnemiesController = FindObjectOfType<EnemiesController>();
            GetTerrainGenerator = FindObjectOfType<TerrainGeneratorController>();
        }
        #endregion
        #region GameManager Functions
        /// <summary>
        /// Пауза
        /// </summary>
        public void Pause()
        {
            Time.timeScale = 0;
        }
        /// <summary>
        /// Возвращение к игре
        /// </summary>
        public void Resume()
        {
            Time.timeScale = 1;
        }
        /// <summary>
        /// Логика окончания игры.
        /// </summary>
        public void FinishGame()
        {
            GetTerrainGenerator.DestroyTerrain();
            _waveIndex = 0;
            GetEnemiesController.ClearEnemyList();
            GetEnemiesController.ClearSpawnerList();
            CurrentMoney = _startMoney;
            _currentLives = _startLives;
            CancelInvoke(CountingDown);
            gameBar.SetActive(false);
            mainMenuPannel.SetActive(true);
        }
        /// <summary>
        /// Логика начала игры.
        /// </summary>
        public void StartGame()
        {
            GetTerrainGenerator.GenerateTerrain();
            GetEnemiesController.Destination = GetTerrainGenerator.EnemiesDestinationPoint;
            GetEnemiesController.SetSpawner(GetTerrainGenerator.EnemiesSpawnPoint);
            CurrentMoney = _startMoney;
            _currentLives = _startLives;
            _countdown = _timeBetweenWaves;
            livesText.text = _currentLives.ToString();
            moneytext.text = CurrentMoney.ToString();
            countdownText.text += (_countdown).ToString();
            mainMenuPannel.SetActive(false);
            gameBar.SetActive(true);
            InvokeRepeating(CountingDown, 0, 1);
        }
        /// <summary>
        /// Обновляет значение текущих днег и отображает его в соответствующем поле.
        /// </summary>
        /// <param name="value">Новое значение денег</param>
        public void UpdateMoney(int value)
        {
            CurrentMoney += value;
            moneytext.text = CurrentMoney.ToString();
        }
        /// <summary>
        /// Обновляет значение текущих жизней и отображает его в соответствующем поле.
        /// </summary>
        /// <param name="value">Новое значение жизней</param>
        public void UpdateLive(int value)
        {
            _currentLives += value;
            livesText.text = _currentLives.ToString();
        }
        /// <summary>
        /// Обертка InvokeRepeating
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="startTime">время первого срабатывания</param>
        /// <param name="repeatRate">интервал повторения</param>
        private void InvokeRepeating(Action action, float startTime, float repeatRate)
        {
            InvokeRepeating(action.Method.Name, startTime, repeatRate);
        }
        /// <summary>
        /// Отнимает 1 секунду от текщего времени _countdown.
        /// Если время истекло, отдает команду EnemiesController'у спавнить мобов.
        /// </summary>
        private void CountingDown()
        {
            countdownText.text = _countdown.ToString();
            _countdown -= 1;
            if (_countdown < 0)
            {
                GetEnemiesController.SpawnSimultaneously(_waveIndex, minEnemiesCountInWave, maxEnemiesCountInWave, ModPercent);
                _waveIndex += 1;
                _countdown = _timeBetweenWaves;
            }
        }
        /// <summary>
        /// Обертка CancleInvoke.
        /// </summary>
        /// <param name="action"></param>
        private void CancelInvoke(Action action)
        {
            CancelInvoke(action.Method.Name);
        }
        #endregion
    }
}


