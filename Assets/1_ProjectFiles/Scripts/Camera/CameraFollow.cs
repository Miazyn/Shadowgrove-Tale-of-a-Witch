using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Terrain terrain; // Reference to your Terrain GameObject
    public float zpadding = 1.0f;
    public float xpadding = 1.0f;

    private Vector3 minLimit;
    private Vector3 maxLimit;

    //THIRD PERSON FOLLOW SCRIPT

    [SerializeField] GameObject characterToFollow;

    [SerializeField] float xPos;
    [SerializeField] float yPos;
    [SerializeField] float zPos;

    public float camHeight = 0;
    public float camZoom = 0;
    private void Start()
    {
        if (!characterToFollow)
        {
            Debug.LogWarning("No Character to follow");
        }

        CalculateCameraLimits();
    }

    private void Update()
    {

        yPos = characterToFollow.transform.position.y + camHeight;

        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        xPos = Mathf.Clamp(xPos, minLimit.x, maxLimit.x);
        zPos = Mathf.Clamp(zPos, minLimit.z - camZoom, maxLimit.z);

        transform.position = new Vector3(xPos, yPos, zPos);

        Debug.Log($"New pos: {new Vector3(xPos, yPos, zPos)}");
    }


    [ContextMenu("Preview Camera Angle")]
    public void Preview()
    {
        yPos = characterToFollow.transform.position.y + camHeight;

        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);
    }

    private void CalculateCameraLimits()
    {
        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;
            Bounds terrainBounds = terrainData.bounds;

            // Calculate minimum and maximum limits with padding
            minLimit = terrainBounds.min;
            maxLimit = terrainBounds.max;

            minLimit = maxLimit/2 * -1;
            maxLimit = maxLimit / 2;

            minLimit = new Vector3(minLimit.x + xpadding, minLimit.y, minLimit.z + zpadding);
            maxLimit = new Vector3(maxLimit.x - zpadding, maxLimit.y, maxLimit.z - zpadding);
            Debug.Log($"Min is: {minLimit} and max: {maxLimit}");
        }
        else
        {
            Debug.LogError("Terrain reference is missing!");
        }
    }

}
