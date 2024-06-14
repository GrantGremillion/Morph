using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : MonoBehaviour
{
    // parent reference
    public GameObject fishController;

    // Gun variables
    [SerializeField] float gunRotationSpeed = 620f;

    // Bullet variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate;

    void Start()
    {
      
    }

    void Update()
    {
        adjustGunRotation();
        checkForShoot();
    }

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

        if (targetGunAngle > gunRotationUpperLimit)
        {
            targetGunAngle = gunRotationUpperLimit;
        }
        else if (targetGunAngle < gunRotationLowerLimit)
        {
            targetGunAngle = gunRotationLowerLimit;
        }

        float newGunAngle = adjustAngle(fishRotationAngle + targetGunAngle);

        transform.rotation = Quaternion.Euler(0, 0, newGunAngle);
    }

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

    float adjustAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) { angle -= 360; }
        if (angle < -180) { angle += 360; }
        return angle;
    }

    void checkForShoot()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
