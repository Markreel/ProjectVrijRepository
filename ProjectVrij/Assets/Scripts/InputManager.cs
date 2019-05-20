using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Settings: ")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private int maxJumpAmount = 1;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [Space]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private Vector3 drag;
    [SerializeField] private float dashDelay = 1f;

    [Header("References: ")]
    [SerializeField] private GameObject movementCam;
    [SerializeField] private MovementTrack currentMovementTrack;

    public float CurrentDashDelay { get { return currentDashDelay; } }
    public float DashDelay { get { return dashDelay; } }

    private int currentJumpAmount = 1;
    private float currentDashDelay;
    private Vector3 velocity;
    private bool isGrounded = true;
    private Transform groundChecker;

    private bool isTurned = false;

    private CharacterController charController;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        groundChecker = transform.GetChild(0);
        currentDashDelay = 0;
    }

    private void Update()
    {
        HandleMovement();
        float _turnRot = isTurned ? -90 : 90;
        transform.eulerAngles = new Vector3(0, movementCam.transform.eulerAngles.y + _turnRot, 0);
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer, QueryTriggerInteraction.Ignore);

        CheckIfGrounded();
        CoolDownDash();

        Vector3 _move = new Vector3(0,0,Input.GetAxis("Horizontal"));
        //_move = transform.forward + _move;
        //DE SPELER ROTEERT NU GOED MAAR BEWEEGT NOG STEEDS NIET LOKAAL!!!
        if (_move != Vector3.zero)
        {
            if (_move.z > 0)
                isTurned = false;
            else
                isTurned = true;

            Debug.Log(transform.forward);
            transform.localPosition +=  transform.forward * Time.deltaTime * moveSpeed;
            //charController.Move((_move + transform.forward) * Time.deltaTime * moveSpeed);
            //transform.localEulerAngles = _move.z > 0 ? Vector3.zero : -Vector3.up * 180; //transform.forward = _move;// + new Vector3(movementCam.transform.eulerAngles.y, 0,0);// + currentMovementTrack.CurrentPoint.forward;
        }

        //Jump With DoubleJump
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpAmount > 0)
            Jump();

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashDelay <= 0)
            Dash();

        velocity.y += gravity * Time.deltaTime;

        velocity.x /= 1 + drag.x * Time.deltaTime;
        velocity.y /= 1 + drag.y * Time.deltaTime;
        //Vraag aan pim hoe dit scipt origineel in elkaar zat (met z as)

        //Debug.Log(velocity);

        //charController.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Launches the player upwards according to the amount of jumps available.
    /// </summary>
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        currentJumpAmount--;
    }

    /// <summary>
    /// Launches the player sideways towards the current direction.
    /// </summary>
    private void Dash()
    {
        velocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, 0));
        currentDashDelay = dashDelay;
    }

    private void CoolDownDash()
    {
        if (currentDashDelay > 0)
            currentDashDelay -= Time.deltaTime;
        else
            currentDashDelay = 0;
    }

    /// <summary>
    /// Checks if the player is currently touching the ground (not in the air).
    /// </summary>
    private void CheckIfGrounded()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            currentJumpAmount = maxJumpAmount;
        }
    }
}