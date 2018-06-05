using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : Singleton<Main> {

    public List<Bot> enemies;

    #region Enemies List
    

    public void AddEnemy(Bot enemy)
    {
        if (!enemies.Contains(enemy) && enemy != null)
        {
            enemies.Add(enemy);
        }
    }
    public void DeleteEnemy(Bot enemy)
    {
        if (enemies.Contains(enemy) && enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
    #endregion


}
