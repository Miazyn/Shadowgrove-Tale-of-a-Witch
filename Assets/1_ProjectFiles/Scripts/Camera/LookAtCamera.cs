using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found in the scene!");
        }
    }

    private void Update()
    {
        if (mainCameraTransform != null)
        {
            transform.LookAt(mainCameraTransform);
        }
    }
}
