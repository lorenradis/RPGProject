using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SceneInfo : ScriptableObject
{
    public string sceneName;
    public string sceneTitle;
    public Vector2[] entrances;
    public float globalLightIntensity;
    public Color globalLightColor;

    public List<SceneInfo> adjacentScenes = new List<SceneInfo>();
    public List<NPCInfo> occupants = new List<NPCInfo>();

    public void AddOccupant(NPCInfo npc)
    {
        if(!occupants.Contains(npc))
        {
            occupants.Add(npc);
        }
    }

    public void RemoveOccupant(NPCInfo npc)
    {
        if(occupants.Contains(npc))
        {
            occupants.Remove(npc);
        }
    }
}
