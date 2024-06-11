using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : MonoBehaviour
{  
    // reference the controller (Note: this is referenced from the inspector)
    public GameObject fishController;
    [SerializeField] float cooldownTimeInSeconds = 1f;
    [SerializeField] float gunRotationSpeed = 620f;
    private float cooldownTimer = 0f;
    private float FPS;

    // Start is called before the first frame update
    void Start()
    {
        FPS = 1.0f / Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        adjustGunRotation();
        checkForShoot();
    }

    // uses mouse position to determine the direction of the watergun
    void adjustGunRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(mousePos.x - fishController.transform.position.x, mousePos.y - fishController.transform.position.y);
        float mouseDirectionInDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float fishRotationAngle = adjustAngle(fishController.transform.eulerAngles.z);
        float targetGunAngle = adjustAngle(mouseDirectionInDegrees - fishRotationAngle);

        float gunRotationUpperLimit = 90f;
        float gunRotationLowerLimit = -90f;

        Debug.Log(targetGunAngle);

        if (targetGunAngle > gunRotationUpperLimit)
        {
            targetGunAngle = gunRotationUpperLimit;
        }
        else if (targetGunAngle < gunRotationLowerLimit)
        {
            targetGunAngle = gunRotationLowerLimit;
        }

        float newGunAngle = adjustAngle(fishRotationAngle + targetGunAngle);
        Debug.Log(newGunAngle);
        transform.rotation = Quaternion.Euler(0, 0, newGunAngle);
    }

    // this returns the max angle that the gun is allowed to move relative to the direction of the fish
    float findUpperAngleLimit(float fishAngle)
    {
        if (fishAngle >= -90 && fishAngle < 90)
        {
            return fishAngle + 90;
        }
        else
        {
            float result = adjustAngle(fishAngle - 90);
            return result; 
        }
    }
    // this returns the min angle that the gun is allowed to move relative to the direction of the fish
    float findLowerAngleLimit(float fishAngle)
    {
        if (fishAngle >= -90 && fishAngle < 90)
        {
            return fishAngle - 90;
        }
        else
        {
            float result = adjustAngle(fishAngle + 90);
            return result;
        }
    }

    // this function makes the angles like the same as the ones in the editor 
    float adjustAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)  { angle -= 360; }
        if (angle < -180) { angle += 360; }
        return angle;
    }

    // gets mouse postion if the leftdown event occurs on mouse
    void checkForShoot()
    {
        if (Input.GetMouseButton(0))
        {
            cooldownTimer += (1/FPS);
            if (cooldownTimer > cooldownTimeInSeconds)
            {
                cooldownTimer = 0f;
            }
              
        }
    }
}
