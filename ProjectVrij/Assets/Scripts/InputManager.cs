using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
	public static Action<float> DashAttackEvent;

    [Header("Settings: ")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpHeight = 2f;
	[SerializeField] private int maxJumpAmount = 1;
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float groundDistance = 0.2f;
	[SerializeField] private LayerMask groundLayer;
	[Space]
	[SerializeField] private int dashDamage = 10;
	[SerializeField] private float dashDistance = 5f;
	[SerializeField] private Vector3 drag;
    [SerializeField] private float dashDelay = 1f;
	[SerializeField] private LayerMask attackMask;

	public float CurrentDashDelay { get { return currentDashDelay; } }
	public float DashDelay { get { return dashDelay; } }

	private int currentJumpAmount = 1;
    private float currentDashDelay;
	private CharacterController charController;
	private Vector3 velocity;
	private bool isGrounded = true;
	private Transform groundChecker;

	private Vector3 dashDistanceses;

	private RaycastHit hit;

	private void Start()
	{
		charController = GetComponent<CharacterController>();
		groundChecker = transform.GetChild(0);
        currentDashDelay = 0;
    }

    private void Update()
	{
        HandleMovement();
	}

	private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer, QueryTriggerInteraction.Ignore);

        CheckIfGrounded();
        CoolDownDash();

        Vector3 _move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        charController.Move(_move * Time.deltaTime * moveSpeed);

        if (_move != Vector3.zero)
            transform.forward = _move;

        //Jump With DoubleJump
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpAmount > 0)
            Jump();

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashDelay <= 0)
            Dash();

        velocity.y += gravity * Time.deltaTime;

        velocity.x /= 1 + drag.x * Time.deltaTime;
        velocity.y /= 1 + drag.y * Time.deltaTime;

        charController.Move(velocity * Time.deltaTime);
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
		Ray _ray;
		RaycastHit[] _hits = Physics.RaycastAll(transform.position, transform.forward, dashDistance, attackMask);

		if(_hits != null)
		{
			foreach (var _hit in _hits)
			{

				print(_hit.collider.name);
			}
		}

		velocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, 0));
        currentDashDelay = dashDelay;

		if(DashAttackEvent != null)
		{
			DashAttackEvent(dashDamage);
		}

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