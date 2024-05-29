using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    public PlayerController player;

    public SpriteRenderer sprite;

    [SerializeField] float swapPointOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.y > transform.position.y + swapPointOffset)
        {
            sprite.sortingOrder = 2;
        }
        else sprite.sortingOrder = 0;
    }


}
