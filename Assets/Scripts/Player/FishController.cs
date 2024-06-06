using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FishController : PlayerController
{
    // varibles specific to the fish
    [SerializeField]  private float rotationSpeedInDegrees;
    private Vector2 inputVector = Vector2.zero;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Child Awake");
        rotationSpeedInDegrees = 180f;
        speed = 5f; 
    }

    // Overridding the FixedUpdate from playerControler
    private void FixedUpdate()
    { 
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
        inputVector = new Vector2(horizontalInput, verticalInput);

        if (inputVector.magnitude != 0)
        {
            RotateFish(horizontalInput, verticalInput);
            MoveFish();
        }
    }

    // Used when entering the fish form from another file
    public void morph(GameObject fish)
    {
        fish.SetActive(true);
    }

    // rotates fish in degrees
    void RotateFish(float horizontalInput, float verticalInput) 
    {
        float inputDirectionInDegrees = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, inputDirectionInDegrees);
        // this line rotates the current angle toward that of the input angle at a rate the rotationSpeedInDegrees
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedInDegrees * Time.deltaTime);
    }

    void MoveFish() 
    {

        // calculate the vector from the inputs (since in 2d obj rotates on z)
        float currentAngleInDegrees = transform.eulerAngles.z;
        float radianAngle = currentAngleInDegrees * Mathf.Deg2Rad;
        Vector2 movement = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * speed;
        // Space.World make sure that the obj moves based on teh world coordinates
        transform.Translate(movement * Time.deltaTime, Space.World);

    }
}
