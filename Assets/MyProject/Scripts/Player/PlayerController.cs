using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 1;
    [SerializeField] float sprintSpeed = 2;
    [SerializeField] float characterRotationSmoothing = 0.03f;

    Rigidbody rb;

    bool IsSprinting = false;

    Vector2 moveDirection;

    InputControls controls;

    private void Start()
    {
        controls = gameObject.GetComponent<Player>().controls;
        controls.Player.Sprint.performed += sprinting => Sprinting();
        controls.Player.StopSprint.performed += sprintStop => StopSprinting();


        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, 3))
        {
            Debug.LogWarning("Player hit sth infront");
        }

        moveDirection = controls.Player.Move.ReadValue<Vector2>();
        transform.position += new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime;
        Rotation();
    }

    void Rotation()
    {
        float horizontalMovement = moveDirection.x;
        float verticalMovement = moveDirection.y;

        Vector3 rotationOfCharacter = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationOfCharacter), characterRotationSmoothing);
        }
    }
    void Sprinting()
    {

        if (speed != speed + sprintSpeed)
        {
            speed += sprintSpeed;
            IsSprinting = !IsSprinting;
        }
    }
    void StopSprinting()
    {

        speed -= sprintSpeed;
        IsSprinting = !IsSprinting;

    }
 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 3);
    }

    


}
