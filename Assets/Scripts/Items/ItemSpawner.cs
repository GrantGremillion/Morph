using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Helpers;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{

    public static ItemSpawner Instance { get; private set; }
    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;

    public float minSpawnDistance = .1f;
    public float maxSpawnDistance = .15f;
    public Tilemap spawnableTiles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnEnemyDrops(string dropType, Vector3 spawnPosition)
    {
        GameObject newItem = null;
        switch (dropType)
            {
                case "banana":
                    newItem = Instantiate(banana, spawnPosition, Quaternion.identity);
                    break;
                case "blueberry":
                    newItem = Instantiate(blueberry, spawnPosition, Quaternion.identity);
                    break;
                case "cherry":
                    newItem = Instantiate(cherry, spawnPosition, Quaternion.identity);
                    break;
            }
    }

    public Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        Vector3 spawnPosition = pos;
        bool positionIsValid = false;

        while (!positionIsValid)
        {
            float randomDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
            float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);
            spawnPosition = pos + spawnOffset;

            Vector3Int cellPosition = spawnableTiles.WorldToCell(spawnPosition);

            if (spawnableTiles.HasTile(cellPosition))
            {
                positionIsValid = true;
            }
        }

        return spawnPosition;
    }

}
