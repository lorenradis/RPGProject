using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    public List<UnitInfo> allEnemies = new List<UnitInfo>();

    public void Start()
    {
        List<UnitInfo> enemies = new List<UnitInfo>();
        for (int i = 0; i < allEnemies.Count; i++)
        {
            enemies.Add(Instantiate(allEnemies[i]));
        }

        allEnemies.Clear();

        allEnemies = enemies;
    }

    public void UpdateKillCounts(UnitInfo enemy)
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i].unitName == enemy.unitName)
            {
                allEnemies[i].numberKilled++;
                return;
            }
        }
    }
}
