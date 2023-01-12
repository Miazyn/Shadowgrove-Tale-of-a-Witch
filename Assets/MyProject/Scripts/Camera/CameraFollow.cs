using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

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
    }

    private void Update()
    {
        yPos = characterToFollow.transform.position.y + camHeight;
        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);

    }


    [ContextMenu("Preview Camera Angle")]
    public void Preview()
    {
        yPos = characterToFollow.transform.position.y + camHeight;
        zPos = characterToFollow.transform.position.z - camZoom;

        xPos = characterToFollow.transform.position.x;

        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
