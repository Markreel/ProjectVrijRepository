using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public static Action<float> DashAttackEvent;

	public Animator anim;

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

    [Header("References: ")]
    [SerializeField] private GameObject rotationCam;
    [SerializeField] private GameObject movementCam;

    [SerializeField] private MovementTrack currentMovementTrack;

    public float CurrentPos { get { return movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition; } }
    public float CurrentDashDelay { get { return currentDashDelay; } }
    public float DashDelay { get { return dashDelay; } }

    private int currentJumpAmount = 1;
    private float currentDashDelay;
    private Vector2 velocity;
    private bool isGrounded = true;
    private Transform groundChecker;
	//private Animator anim;
    private bool isTurned = false;
	private float horizontal = 0f;

    private void Start()
    {
		//anim = GetComponentInChildren<Animator>();
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
		horizontal = Input.GetAxis("Horizontal");
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer, QueryTriggerInteraction.Ignore);

        CoolDownDash();


        //MAAK EEN SYSTEEM WAARBIJ DE SPELER MAAR KAN BEWEGEN BINNEN EEN BEPAALD GEBIED (BIJVOORBEELD TUSSEN PUNT 1 EN 4 OP DE DOLLYTRACK) EN DAT DE SPELER PAS DOORKAN
        //NADAT ALLES ENEMIES DOOD ZIJN (OOK HANDIG VOOR GEBIED VOOR EEN BOSS FIGHT)


        //Walk
        if (horizontal != 0)
        {
            isTurned = horizontal > 0 ? false : true;
            Walk();
        }
		else
		{
			velocity.x = 0;
		}
		
        Debug.DrawRay(transform.position, transform.forward);

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
        _dolly.m_PathPosition = BoundaryManager.Instance.ClampDistance(_dolly.m_PathPosition);
        transform.position = new Vector3(movementCam.transform.position.x, transform.position.y + velocity.y, movementCam.transform.position.z);// * Time.deltaTime;

        BoundaryManager.Instance.CheckIfWithinBoundary(_dolly.m_PathPosition);
    }

    /// <summary>
    /// Moves the player across the dolly track according to the given horizontal input
    /// </summary>
    private void Walk()
    {
        //CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
        //float _pathLenght = _dolly.m_Path.PathLength;
        //float _camPos = _dolly.m_PathPosition;

        velocity.x = (isTurned ? -Time.deltaTime : Time.deltaTime) * moveSpeed;
		//anim.SetBool("isRunning", true);
		//anim.SetFloat("Movement", horizontal);  
		//_dolly.m_PathPosition = _camPos;

		//transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
	}

	/// <summary>
	/// Launches the player upwards according to the amount of jumps available.
	/// </summary>
	private void Jump()
    {
		//anim.SetBool("isGrounded", false);
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        currentJumpAmount--;
    }

    /// <summary>
    /// Launches the player sideways towards the current direction.
    /// </summary>
    private void Dash()
    {
        RaycastHit[] _hits = Physics.RaycastAll(transform.position, transform.forward, dashDistance, attackMask);

        if(_hits != null)
        {
            foreach (var _hit in _hits)
            {
                EnemyParent _enemy = _hit.transform.GetComponent<EnemyParent>();

                if (_enemy != null)
                    _enemy.TakeDamage(dashDamage);

                print(_hit.collider.name);
            }
        }

		velocity.x = 0;

        float _value = dashDistance * Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime;
        velocity.x += isTurned ? -_value : _value; // Vector3.Scale(transform.forward, dashDistance * new Vector3((), 0, 0));
        currentDashDelay = dashDelay;

		//anim.SetBool("isDashing", true);

        if(DashAttackEvent != null)
            DashAttackEvent(1);
    }

    private void CoolDownDash()
    {
        if (currentDashDelay > 0)
		{
			if(currentDashDelay > dashDelay -1)
			{
				velocity.x = 0;
			}
            currentDashDelay -= Time.deltaTime;
		}
			
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
			//anim.SetBool("isGrounded", true);
		}
	}
}