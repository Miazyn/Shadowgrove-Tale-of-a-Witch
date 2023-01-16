using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3dFollow : MonoBehaviour
{
    [SerializeField] GameObject characterToFollow;

    [SerializeField] float xPos;
    [SerializeField] float yPos;
    [SerializeField] float zPos;

    public float camHeight = 0;
    public float camZoom = 0;

    private void Start()
    {
        
        if (characterToFollow == null)
        {
            Debug.LogError("Player missing from scene");
        }
        //Player event here: For CamMovement
    }

    //DOES NOT WORK YET
    void UpdateCamMovement()
    {
        yPos = characterToFollow.transform.position.y + camHeight;
        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);
        transform.rotation = characterToFollow.transform.rotation;
    }


    [ContextMenu("Preview Camera Angle")]
    public void Preview()
    {
        yPos = characterToFollow.transform.position.y + camHeight;
        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);
        transform.rotation = characterToFollow.transform.rotation;

    }
}
