using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float sortingOrderOffset = .2f;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Adjust the sorting order based on the y-position of the object
        spriteRenderer.sortingOrder = (int)(sortingOrderOffset - transform.position.y);
    }
}
