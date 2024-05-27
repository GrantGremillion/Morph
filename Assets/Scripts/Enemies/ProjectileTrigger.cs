using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    private Enemy enemy;
    private Collider2D enemyCollider;
    public float disableDuration = 2.0f; // Duration to disable the collider

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider == null)
            {
                Debug.LogError("No Collider2D component found on the Enemy.");
            }
        }
        else
        {
            Debug.LogError("No Enemy component found in parent.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enemy == null || enemyCollider == null)
        {
            Debug.LogError("Enemy or its Collider2D component is not set properly.");
            return;
        }

        if (collider.CompareTag("Arrow") && enemy.currentState == Enemy.State.Idle)
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    private IEnumerator DisableColliderTemporarily()
    {
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(disableDuration);
        enemyCollider.enabled = true;
    }
}
