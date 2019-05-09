using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpHeight = 2f;
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float groundDistance = 0.2f;
	[SerializeField] private float dashDistance = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Vector3 drag;
	[SerializeField] private int maxJumpAmount = 1;

	private int currentJumpAmount = 1;
	private CharacterController charController;
	private Vector3 velocity;
	private bool isGrounded = true;
	private Transform groundChecker;


	void Start()
	{
		charController = GetComponent<CharacterController>();
		groundChecker = transform.GetChild(0);
	}

	void Update()
	{
		isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer, QueryTriggerInteraction.Ignore);

		//Check Grounded
		if (isGrounded && velocity.y < 0)
		{
			currentJumpAmount = maxJumpAmount;
			velocity.y = 0f;
		}

		Vector3 _move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		charController.Move(_move * Time.deltaTime * moveSpeed);

		if (_move != Vector3.zero)
		{
			transform.forward = _move;
		}

		//Jump With DoubleJump
		if (Input.GetKeyDown(KeyCode.Space) && currentJumpAmount > 0)
		{
			velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
			currentJumpAmount--;
		}
		
		//Dash
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			velocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
		}

		velocity.y += gravity * Time.deltaTime;

		velocity.x /= 1 + drag.x * Time.deltaTime;
		velocity.y /= 1 + drag.y * Time.deltaTime;
		velocity.z /= 1 + drag.z * Time.deltaTime;

		charController.Move(velocity * Time.deltaTime);
	}
}