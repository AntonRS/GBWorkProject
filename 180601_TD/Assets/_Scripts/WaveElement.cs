using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;
using System;

[Serializable]
public class WaveElement
{
    public BaseEnemy enemy;
    public int count;
    public float speed;
    public float hp;
}
