using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    [SerializeField] private GameObject rotationCam;
    [SerializeField] private GameObject movementCam;

    [SerializeField] private MovementTrack currentMovementTrack;

    public float CurrentDashDelay { get { return currentDashDelay; } }
    public float DashDelay { get { return dashDelay; } }

    private int currentJumpAmount = 1;
    private float currentDashDelay;
    private Vector2 velocity;
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
        HandleRotation();
    }

    private void HandleRotation()
    {
        float _turnRot = isTurned ? -90 : 90;
        transform.eulerAngles = new Vector3(0, rotationCam.transform.eulerAngles.y + _turnRot, 0);
    }

    private void HandleMovement()
    {
        CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
        float _pathLenght = _dolly.m_Path.PathLength;
        float _hor = Input.GetAxis("Horizontal");
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer, QueryTriggerInteraction.Ignore);

        CoolDownDash();

        //Turn left or right

        //Walk
        if (_hor != 0)
        {
            isTurned = _hor > 0 ? false : true;
            Walk();
        }
        else
            velocity.x = 0;

        //Jump With DoubleJump
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpAmount > 0)
            Jump();

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashDelay <= 0)
            Dash();

        //Apply velocity
        velocity.y += gravity * Time.deltaTime;

        velocity.y /= 1 + drag.y * Time.deltaTime;
        CheckIfGrounded();

        _dolly.m_PathPosition = Mathf.Clamp(_dolly.m_PathPosition + velocity.x, 0, _pathLenght);
        transform.position = new Vector3(movementCam.transform.position.x, transform.position.y + velocity.y, movementCam.transform.position.z);// * Time.deltaTime;
    }

    private void Walk()
    {
        //CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
        //float _pathLenght = _dolly.m_Path.PathLength;
        //float _camPos = _dolly.m_PathPosition;

        velocity.x = (isTurned ? -Time.deltaTime : Time.deltaTime) * moveSpeed;
        //_dolly.m_PathPosition = _camPos;

        //transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
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
        float _value = dashDistance * Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime;
        velocity.x += isTurned ? -_value : _value; // Vector3.Scale(transform.forward, dashDistance * new Vector3((), 0, 0));
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