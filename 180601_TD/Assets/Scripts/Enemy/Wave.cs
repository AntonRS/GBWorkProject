using System;
namespace Game.Enemy
{
    [Serializable]
    public class Wave
    {
        public WaveElement[] enemiesTypeAndCount;
        public float waveTime;
    }
}

