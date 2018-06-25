using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;
namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        
        private float countdown;
        public bool isCountingDown = false;
        public int waveIndex = 0;
        public float[] wavesCountDowns;
        private void Start()
        {
            countdown = wavesCountDowns[waveIndex];
        }

        private void FixedUpdate()
        {
            if (isCountingDown)
            {
                Debug.Log((int)countdown+"___"+waveIndex);
                countdown -= Time.fixedDeltaTime;
                if (countdown <= 0)
                {
                    
                    
                    EnemiesController.Instance.SpawnSimultaneously(waveIndex);
                    waveIndex += 1;
                    countdown = wavesCountDowns[waveIndex];

                }
            }
        }
    }
}


