using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 1;
    [SerializeField] float sprintSpeed = 2;
    [SerializeField] float characterRotationSmoothing = 0.03f;
    string horizontalAxis, verticalAxis;
    string sprintButton = "Sprint";

    bool IsSprinting = false;
    private void Start()
    {
        VariableSetup();

    }

    private void VariableSetup()
    {
        horizontalAxis = "Horizontal";
        verticalAxis = "Vertical";
    }

    private void Update()
    {
        Sprinting();

        transform.position += new Vector3(Input.GetAxisRaw(horizontalAxis), 0, Input.GetAxisRaw(verticalAxis)) * speed * Time.deltaTime;

        float horizontalMovement = Input.GetAxisRaw(horizontalAxis);
        float verticalMovement = Input.GetAxisRaw(verticalAxis);

        Vector3 rotationOfCharacter = new Vector3(horizontalMovement, 0.0f, verticalMovement);

        if (verticalMovement != 0 || horizontalMovement != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationOfCharacter), characterRotationSmoothing);
        }

    }

    private void Sprinting()
    {
        if (Input.GetButtonDown(sprintButton))
        {
            if (speed != speed + sprintSpeed)
            {
                speed += sprintSpeed;
                IsSprinting = !IsSprinting;
            }
        }
        if (Input.GetButtonUp(sprintButton))
        {
            speed -= sprintSpeed;
            IsSprinting = !IsSprinting;
        }
    }

}
