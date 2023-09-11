using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Terrain terrain; // Reference to your Terrain GameObject
    public float padding = 1.0f; // Padding to prevent the camera from getting too close to terrain edges

    private Vector3 minLimit;
    private Vector3 maxLimit;

    private void Start()
    {
        // Calculate camera limits based on terrain bounds
        CalculateCameraLimits();
    }

    private void CalculateCameraLimits()
    {
        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;
            Bounds terrainBounds = terrainData.bounds;

            // Calculate minimum and maximum limits with padding
            minLimit = terrainBounds.min + new Vector3(padding, 0, padding);
            maxLimit = terrainBounds.max - new Vector3(padding, 0, padding);
        }
        else
        {
            Debug.LogError("Terrain reference is missing!");
        }
    }

    private void LateUpdate()
    {
        // Get the current camera position
        Vector3 newPosition = transform.position;

        // Clamp the camera position within the calculated limits
        newPosition.x = Mathf.Clamp(newPosition.x, minLimit.x, maxLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minLimit.z, maxLimit.z);

        // Update the camera position
        transform.position = newPosition;
    }
}

