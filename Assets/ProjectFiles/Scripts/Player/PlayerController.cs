using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    InputControls controls;

    [SerializeField] float speed = 1;
    [SerializeField] float sprintSpeed = 2;
    [SerializeField] float characterRotationSmoothing = 0.03f;
    float playerHeight;

    [SerializeField] CharacterController charControl;
    Rigidbody rb;

    bool IsSprinting = false;

    Vector2 moveDirection;
    bool IsGrounded;

    public float gravity = -9.8f;

    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    bool canMove;

    private void Awake()
    {
        canMove = true;
    }
    private void Start()
    {

        controls = gameObject.GetComponent<Player>().controls;
        controls.Player.Sprint.performed += sprinting => Sprinting();
        controls.Player.StopSprint.performed += sprintStop => StopSprinting();

        playerHeight = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.OnDialogueStart.AddListener(DisableMovement);
        EventManager.OnDialogueEnd.AddListener(EnableMovement);
    }

    private void OnDisable()
    {
        EventManager.OnDialogueStart.RemoveListener(DisableMovement);
        EventManager.OnDialogueEnd.RemoveListener(EnableMovement);
    }

    void Update()
    {
        if (canMove)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2;
            }
            moveDirection = controls.Player.Move.ReadValue<Vector2>();

            charControl.Move(new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime);
            Rotation();


            velocity.y += gravity * Time.deltaTime;
            charControl.Move(velocity * speed);
        }
        
        
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

    void EnableMovement()
    {
        canMove = true;
    }
 
    void DisableMovement()
    {
        canMove = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 3);

        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }


}
