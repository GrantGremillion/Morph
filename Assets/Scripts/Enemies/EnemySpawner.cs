using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private MushroomEnemy mushroomEnemy;
    [SerializeField]
    private BugEnemy bugEnemy;
    public List<Enemy> enemyList; 

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    private PlayerController player;

    private int NumEnemiesSpawned;


    // Start is called before the first frame update
    void Awake()
    {
        enemyList = new List<Enemy>{bugEnemy, mushroomEnemy};
        SetTimeUntilSpawn();
        player = FindAnyObjectByType<PlayerController>();

        NumEnemiesSpawned = Random.Range(3,8);
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            Instantiate(enemyList[Random.Range(0, enemyList.Count)], transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
            NumEnemiesSpawned -= 1;
            if (NumEnemiesSpawned == 0) Destroy(gameObject);
        }
    }


    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
