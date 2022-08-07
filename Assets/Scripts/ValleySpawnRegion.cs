using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleySpawnRegion : SpawnRegion
{
    public override void Activate()
    {
        base.Activate();
        for (int i = 0; i < GameManager.instance.valleySpawns.Count; i++)
        {
            availableSpawns.Add(GameManager.instance.valleySpawns[i]);
        }
        minLevel = GameManager.instance.playerInfo.level - 2;
        maxLevel = GameManager.instance.playerInfo.level + 2;
    }
}
