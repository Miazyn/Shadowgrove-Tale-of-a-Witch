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

    [SerializeField] Animator anim;
    [SerializeField] GameObject playerMesh;


    [SerializeField] float walkStateTransitionTime = 0;

    private float lockedTime;
    private string itemName = "nothing";

    bool IsPassingOut = false;
    bool IsCollapsing = false;

    private float swingAxeTime = 2.3f;
    private float waterTime = 4.5f;
    private float yawnTime = 5.2f;
    private float collapseTime = 3.6f;

    private int currentState;

    private static readonly int Idle = Animator.StringToHash("Standing Idle");
    private static readonly int Walk = Animator.StringToHash("Walking");
    private static readonly int Run = Animator.StringToHash("Running");
    private static readonly int Water = Animator.StringToHash("Watering");
    private static readonly int SwingAxe = Animator.StringToHash("Chop tree");
    private static readonly int Yawn = Animator.StringToHash("Yawn");
    private static readonly int Collapse = Animator.StringToHash("Collapse Exhaustion");

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
        EventManager.OnInteractionStart.AddListener(DisableMovement);
        EventManager.OnInteractionEnd.AddListener(EnableMovement);
        EventManager.OnPlayerPassOut.AddListener(PassOut);
        EventManager.StartNewDay.AddListener(NextDayAfterCollapse);
    }

    private void OnDisable()
    {
        EventManager.OnInteractionStart.RemoveListener(DisableMovement);
        EventManager.OnInteractionEnd.RemoveListener(EnableMovement);
        EventManager.OnPlayerPassOut.RemoveListener(PassOut);
        EventManager.StartNewDay.RemoveListener(NextDayAfterCollapse);
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

        var state = GetState(itemName);

        if (state == currentState) return;

        itemName = "nothing";
        anim.CrossFade(state, 0, 0);
        currentState = state;

        playerMesh.transform.position = transform.position;
        playerMesh.transform.rotation = transform.rotation;
    }

    private int GetState(string _itemName)
    {
        if (Time.time < lockedTime) return currentState;

        if (IsPassingOut)
        {
            IsPassingOut = false;
            return LockState(Yawn, yawnTime);
        }

        if (currentState == Yawn)
        {
            IsCollapsing = true;
            return LockState(Collapse, collapseTime);
        }

        if (IsCollapsing)
        {
            IsCollapsing = false;
            EventManager.StartNewDay?.Invoke();
            EventManager.OnInteractionStart?.Invoke();
        }

        if(!IsPassingOut && !IsCollapsing && currentState != Yawn)
        {
            EventManager.OnInteractionEnd?.Invoke();
        }

        if (_itemName.Contains("Wateringcan")) return LockState(Water, waterTime);

        if (_itemName.Contains("Axe")) return LockState(SwingAxe, swingAxeTime);

        if (_itemName.Contains("Hammer")) return -1;
        if (_itemName.Contains("Garden hoe")) return -1;
        if (_itemName.Contains("Wand")) return -1;


        if (moveDirection == Vector2.zero) return Idle;

        if (IsSprinting) return Run;
        if (moveDirection != Vector2.zero) return Walk;


        int LockState(int s, float t)
        {
            EventManager.OnInteractionStart?.Invoke();

            lockedTime = Time.time + t;
            return s;
        }

        return -1;
    }

    private void NextDayAfterCollapse()
    {
        EventManager.OnInteractionEnd?.Invoke();
    }

    private void PassOut()
    {
        IsPassingOut = true;
    }
    public void GetToolAnimation(string itemName)
    {
        this.itemName = itemName;
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
