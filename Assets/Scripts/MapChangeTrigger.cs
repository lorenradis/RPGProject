using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeTrigger : MonoBehaviour
{
    public MapRegion regionToChangeTo;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public Vector2 playerChange;

    private void Start()
    {
        minBounds = regionToChangeTo.GetMinBounds();
        maxBounds = regionToChangeTo.GetMaxBounds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("I done been triggered");
            ChangeMap();
        }
    }

    private void ChangeMap()
    {
        GameManager.instance.ChangeMap(minBounds, maxBounds, playerChange, regionToChangeTo);
    }
}
