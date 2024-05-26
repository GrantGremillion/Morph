using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<string> items = new List<string>();

    public PlayerController player;

    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;

    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 5f;
    public float moveSpeed = 1f; // Speed at which items move away from the player
    public float maxDistanceFromPlayer = 10f; // Maximum distance from player before items stop moving

    public void AddItem(string item)
    {
        if (item != null)
        {
            items.Add(item);
            Debug.Log("Item added: " + item + ". Total items: " + items.Count);

        }
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (string item in items)
        {
            if (item == itemName)
            {
                count++;
            }
        }
        return count;
    }


    public void DropItems()
    {

        // Drop two random items 
        for (int i = 0; i < 2; i++)
        {
            // Make sure the player has items
            if (items.Count == 0) return;

            string randomItem = RandomElementSelector.GetRandomElement(items);
            Vector3 spawnPosition = GetRandomSpawnPosition(player.transform.position);
            //print("Randomly selected item: " + randomItem);


            GameObject newItem = null;
            if (randomItem == "banana")
            {
                newItem = Instantiate(banana, spawnPosition, Quaternion.identity);
            }
            else if (randomItem == "blueberry")
            {
                newItem = Instantiate(blueberry, spawnPosition, Quaternion.identity);
            }
            else
            {
                newItem = Instantiate(cherry, spawnPosition, Quaternion.identity);
            }

            // Remove item from player inventory
            items.Remove(randomItem);

            // Start moving the new item away from the player
            if (newItem != null)
            {
                StartCoroutine(MoveItemAway(newItem.transform));
            }
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 playerPosition)
    {
        // Get random distance within the min and max spawn distances
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Get random angle around the player
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate spawn position offset
        Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);

        // Calculate final spawn position
        Vector3 spawnPosition = playerPosition + spawnOffset;

        return spawnPosition;
    }

    private IEnumerator MoveItemAway(Transform itemTransform)
    {
        while (Vector3.Distance(itemTransform.position, player.transform.position) < maxDistanceFromPlayer)
        {
            // Move the item away from the player
            itemTransform.position += (itemTransform.position - player.transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

}
