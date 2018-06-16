using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeekBrains
{
    [System.Serializable]
    public struct UpdateInfo
    {
        public int newUpdateCost;
        public int newDamage;
        public float newAttackRange;
        public float newAttackPerSecond;
        public bool newIsAbleToAttackGround;
        public bool newIsAbleToAttackAir;
        public AttackType newAttackType;
        public string newAmunitionPath;
    }
}

