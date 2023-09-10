using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    private const int DistanceToCam = 3000;
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    public Material WallMaterial;
    public Camera Cam;
    public LayerMask Mask;



    void Update()
    {
        var direction = Cam.transform.position - transform.position;
        var ray = new Ray(transform.position, direction.normalized);

        //Behind the wall
        if(Physics.Raycast(ray, DistanceToCam, Mask))
        {
            WallMaterial.SetFloat(SizeID, 0.5f);

        }
        else
        {
            WallMaterial.SetFloat(SizeID, 0);
        }

        var view = Cam.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, view);
    }
}
