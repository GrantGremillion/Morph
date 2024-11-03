using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayingEntrance : MonoBehaviour
{
    [SerializeField]
    public SimpleRandomWalkGenerator simpleRandomWalkGenerator;

    void OnTriggerEnter2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            simpleRandomWalkGenerator.RunProceduralGeneration();
        }
    }
}
