using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRegion : MonoBehaviour
{
    private BoxCollider2D col2d;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    [SerializeField]
    private AudioClip regionBGM;

    [SerializeField]
    private List<SpawnRegion> spawnRegions = new List<SpawnRegion>();
    [SerializeField]
    private List<StoryTrigger> storyTriggers = new List<StoryTrigger>();

    private void Awake()
    {
        col2d = GetComponent<BoxCollider2D>();
        minBounds = transform.position;
        maxBounds = transform.position + (col2d.bounds.size);
        SpawnRegion[] regions = GetComponentsInChildren<SpawnRegion>();
        for (int i = 0; i < regions.Length; i++)
        {
            spawnRegions.Add(regions[i]);
        }
    }

    public Vector2 GetMinBounds()
    {
        return minBounds;
    }

    public Vector2 GetMaxBounds()
    {
        return maxBounds;
    }

    public void OnActivate()
    {
 //       Debug.Log("I done been activated");
        for (int i = 0; i < spawnRegions.Count; i++)
        {
            spawnRegions[i].Activate();
        }
        for (int i = 0; i < storyTriggers.Count; i++)
        {
            storyTriggers[i].Activate();
        }
        GameManager.instance.audioManager.SetBGM(regionBGM);
    }

    public void OnDeactivate()
    {
//        Debug.Log("I done been deactivated!");
        for (int i = 0; i < spawnRegions.Count; i++)
        {
            spawnRegions[i].Deactivate();
        }
        for (int i = 0; i < storyTriggers.Count; i++)
        {
            storyTriggers[i].Deactivate();
        }
    }
}
