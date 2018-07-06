using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;
namespace Game.Towers
{
    [RequireComponent(typeof(LineRenderer))]
    public class Lazer : BaseAmmunition
    {

        private LineRenderer _lazer;
        private float _lazerMultiplyDistance;
        private List<BaseEnemy> nearEnemies;

        private void Awake()
        {
            _lazer = GetComponent<LineRenderer>();
        }
        
        public void SetPoints(Vector3 startPosition, Vector3 target)//, int enemiesCount, List<BaseEnemy> enemies)
        {

             _lazer.SetPosition(1, startPosition);
            _lazer.SetPosition(1, target);
            //_lazer.positionCount = enemiesCount + 1;
            //for (int i = 1; i <= enemiesCount; i++)
            //{
            //    _lazer.SetPosition(i, enemies[i - 1].transform.position);
            //}
        }
    }   
}

