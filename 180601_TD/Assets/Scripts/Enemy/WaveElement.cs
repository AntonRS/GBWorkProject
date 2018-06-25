
using System;
namespace Game.Enemy
{
    [Serializable]
    public class WaveElement
    {
        public BaseEnemy enemy;
        public int count;
        public float speed;
        public float hp;
    }
}

