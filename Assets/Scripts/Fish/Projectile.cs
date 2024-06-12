using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    // fields (note: lifeTime is number of frames)
    [SerializeField] float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the vector from the inputs (since in 2d obj rotates on z)
        float currentAngleInDegrees = transform.eulerAngles.z;
        float radianAngle = currentAngleInDegrees * Mathf.Deg2Rad;

        Vector2 movement = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * Speed;

        // Space.World make sure that the obj moves based on the world coordinates
        transform.Translate(movement * Time.deltaTime, Space.World);

    }

}
