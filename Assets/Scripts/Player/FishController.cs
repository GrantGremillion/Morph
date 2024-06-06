using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FishController : PlayerController
{
    // varibles specific to the fish
    [SerializeField]  private float rotationSpeedInDegrees;
    private float currentDirectionInDegrees;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeedInDegrees = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFishInput();
    }

    // Gets input from user and calls appropriate functions
    void CheckFishInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

        if (inputVector.magnitude != 0)
        {
            RotateFish(horizontalInput, verticalInput);
        }
    }

    // Used when entering the fish form from another file
    public void morph(GameObject fish)
    {
        fish.SetActive(true);
    }

    // rotates fish in degrees
    void RotateFish(float horizontalInput, float verticalInput) {
        float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        // this line rotates the current angle toward that of the input angle at a rate the rotationSpeedInDegrees
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedInDegrees * Time.deltaTime);
    }
}
