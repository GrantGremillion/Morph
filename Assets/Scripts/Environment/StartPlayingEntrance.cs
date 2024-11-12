using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlayingEntrance : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            SceneManager.LoadScene("Lvl1");
        }
    }
}
