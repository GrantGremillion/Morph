using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        numberOfDrops = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Arrow"))
        {
            DropItems();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

        public void DropItems()
    {

        // Drop all enemy items
        for (int i = 0; i < numberOfDrops; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(transform.position);

            GameObject newItem = null;

            newItem = Instantiate(banana, spawnPosition, Quaternion.identity);


            StartCoroutine(MoveItemAway(newItem.transform));
        }
    }
}
