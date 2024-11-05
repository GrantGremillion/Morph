using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BigJohnny : MonoBehaviour
{
  public GameObject canvasChild;  // Reference to the child object of the canvas
    private bool isPlayerInRange = false;

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;

    void Start()
    {
        // Ensure the canvas child starts as disabled
        if (canvasChild != null)
            canvasChild.SetActive(false);
    }

    void Update()
    {
        if (canvasChild.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }

        // Check if the player is in range and presses "E"
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Enable the canvas child object
            if (canvasChild != null)
                canvasChild.SetActive(true);

            textComponent.text = string.Empty;
            StartDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider belongs to the player
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // Reset when the player leaves the collider area
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (canvasChild != null)
                canvasChild.SetActive(false);  // Hide the canvas child when out of range
        }
    }

    void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length -1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            canvasChild.SetActive(false);
        }
    }
}
