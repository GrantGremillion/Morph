using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public string dropType;
    public int numberOfDrops;



    // Item prefabs
    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;


    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 5f;
    public float moveSpeed = 1f; // Speed at which items move away from the player
    public float maxDistanceFromPlayer = 10f; // Maximum distance from player before items stop moving

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    


    public Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        // Get random distance within the min and max spawn distances
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Get random angle around the player
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate spawn position offset
        Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);

        // Calculate final spawn position
        Vector3 spawnPosition = pos + spawnOffset;

        return spawnPosition;
    }

    public IEnumerator MoveItemAway(Transform itemTransform)
    {
        while (Vector3.Distance(itemTransform.position, transform.position) < maxDistanceFromPlayer)
        {
            // Move the item away from the player
            itemTransform.position += (itemTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
