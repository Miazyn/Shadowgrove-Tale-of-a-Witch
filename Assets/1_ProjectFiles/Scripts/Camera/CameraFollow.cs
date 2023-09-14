using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //public Terrain terrain; // Reference to your Terrain GameObject
    //public float zpadding = 1.0f;
    //public float xpadding = 1.0f;

    private Vector3 minLimit;
    private Vector3 maxLimit;

    //THIRD PERSON FOLLOW SCRIPT

    [SerializeField] public GameObject characterToFollow;

    float xPos;
    float yPos;
    float zPos;

    Vector3 playerPos;

    public float camHeight = 0;
    public float camZoom = 0;

    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask terrainLayer;

    private void Start()
    {
        if (!characterToFollow)
        {
            Debug.LogWarning("No Character to follow");
        }

        //CalculateCameraLimits();
    }

    private void Update()
    {



        yPos = characterToFollow.transform.position.y + camHeight;

        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        Ray ray = new Ray(characterToFollow.transform.position, -transform.forward);
        RaycastHit hit;
        

        if (Physics.Raycast(ray, out hit, raycastDistance + camZoom, terrainLayer))
        {
            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(xPos, yPos, zPos);
        }
    }


    [ContextMenu("Preview Camera Angle")]
    public void Preview()
    {
        yPos = characterToFollow.transform.position.y + camHeight;

        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other}");
    }

}
