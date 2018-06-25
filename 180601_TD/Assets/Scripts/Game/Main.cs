using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;
using Game.Towers;

namespace Game
{
    public class Main : Singleton<Main>
    {

        [SerializeField] private GameManager _gameManager;
        [SerializeField] private EnemiesController _enemiesController;
        [SerializeField] private TowersManager _towersManager;

        public GameManager GetGameManager { get { return _gameManager; } }
        public EnemiesController GetEnemiesController { get { return _enemiesController; } }
        public TowersManager GetTowersManager { get { return _towersManager; } }
    }
}


