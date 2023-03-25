using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_light : MonoBehaviour
{
     public float rotationSpeed = 90f; // The rotation speed in degrees per second
    // Start is called before the first frame updat
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f,rotationSpeed * Time.deltaTime);
    }
}
