using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegion : MonoBehaviour
{

    private bool isActive = false;

    [SerializeField]
    protected List<UnitInfo> availableSpawns = new List<UnitInfo>();

    private List<GameObject> activeSpawns = new List<GameObject>();

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int spawnFrequency = 1;

    private float spawnTimer = 1f;
    private float elapsedTime = 0f;

    private int spawnChance = 0;

    [SerializeField]
    private int maxSpawns = 3;

    public int minLevel = 3;
    public int maxLevel = 5;

    public virtual void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
        for (int i =  activeSpawns.Count-1; i >= 0; i--)
        {
            Destroy(activeSpawns[i]);
        }
        activeSpawns.Clear();
    }

    private void Update()
    {
        if (GameManager.instance.gameState != GameManager.GameState.NORMAL)
            return;
        if(isActive && activeSpawns.Count < maxSpawns)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= spawnTimer)
            {
                elapsedTime -= spawnTimer;
                RollForSpawn();
            }
        }
    }

    private void RollForSpawn()
    {
        spawnChance += spawnFrequency;
        int roll = Random.Range(1, 100) * (activeSpawns.Count + 1);
        if (roll <= spawnChance)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int level = Random.Range(minLevel, maxLevel+1);
        spawnChance = 0;
        Collider2D col2d = GetComponent<Collider2D>();
        float x = transform.position.x + Random.Range(-col2d.bounds.size.x * .5f, col2d.bounds.size.x * .5f);
        float y = transform.position.y + Random.Range(-col2d.bounds.size.y * .5f, col2d.bounds.size.y * .5f);
        Vector2 newPosition = new Vector2(x, y);
        GameObject newEnemyObject = Instantiate(enemyPrefab, newPosition, Quaternion.identity) as GameObject;
        UnitInfo enemyInfo = Instantiate(availableSpawns[Random.Range(0, availableSpawns.Count)]);
        enemyInfo.Setup();
        enemyInfo.SetLevel(level);
        newEnemyObject.GetComponent<EnemyMovement>().SetEnemyUnit(enemyInfo);
        activeSpawns.Add(newEnemyObject);
    }
}