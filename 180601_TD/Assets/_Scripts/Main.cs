using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeekBrains;

public class Main : Singleton<Main> {

    public List<BaseEnemy> enemies;

    #region Enemies List
    

    public void AddEnemy(BaseEnemy enemy)
    {
        if (!enemies.Contains(enemy) && enemy != null)
        {
            enemies.Add(enemy);
        }
    }
    public void DeleteEnemy(BaseEnemy enemy)
    {
        if (enemies.Contains(enemy) && enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
    #endregion


}
