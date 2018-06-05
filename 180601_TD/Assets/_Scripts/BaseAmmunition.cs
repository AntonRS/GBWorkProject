using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeekBrains
{
    public abstract class BaseAmmunition : MonoBehaviour
    {
        public Bot target;
        public float speed;
        public float damage;
        public AttackType attackType;
        
    }
}