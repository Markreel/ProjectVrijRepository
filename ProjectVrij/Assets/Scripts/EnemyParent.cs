using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class EnemyParent : MonoBehaviour
{
	public static Action<float> EnemyDamageEvent;

	[Header("Settings: ")]
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float movementSpeed = 4f;
	[SerializeField] private float damage = 5f;

	[SerializeField] private float playerSpottedRange = 10f;
	[SerializeField] private float attackRange = 3f;
	[SerializeField] private float attackCooldownTimer = 3f;

	[Header("References: ")]
	[SerializeField] private InputManager player;
	[SerializeField] private GameObject movementCamPrefab;
	[SerializeField] private Animator anim;

	private GameObject movementCam;
	private float rotationSpeed = 10f;
	private float tempMoveSpeed;
	private float attackTimer;
	private bool canAttack;
	private float currentHealth;

	private float distanceBetweenPlayer { get { return Mathf.Abs(player.CurrentPos - movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition); } }
	private bool isTurned { get { return player.CurrentPos < movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition ? true : false; } }

	public virtual void Awake()
	{
		InitializeMovementCam();
		ResetHealth();
		anim = GetComponentInChildren<Animator>();
	}

	private void InitializeMovementCam()
	{
		movementCam = Instantiate(movementCamPrefab);

		CinemachineVirtualCamera _vCam = movementCam.GetComponent<CinemachineVirtualCamera>();

		_vCam.Follow = transform;
		_vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("DollyTrack1").GetComponent<CinemachinePathBase>();
		// movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().track
	}

	private void ResetHealth()
	{
		currentHealth = maxHealth;
	}

	public virtual void Update()
	{
		CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
		float _pathLenght = _dolly.m_Path.PathLength;
		float _step = movementSpeed * Time.deltaTime; // calculate distance to move

		if (player != null)
		{
			if (distanceBetweenPlayer <= playerSpottedRange && distanceBetweenPlayer > attackRange)
			{
				tempMoveSpeed = (isTurned ? -Time.deltaTime : Time.deltaTime) * movementSpeed;
				_dolly.m_PathPosition = Mathf.Clamp(_dolly.m_PathPosition + tempMoveSpeed, 0, _pathLenght);

				transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
			}
			if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
				anim.SetBool("isAttacking", true);
			else
			{
				anim.SetBool("isAttacking", false);
			}

			//if (Vector3.Distance(player.transform.position, transform.position) <= playerSpottedRange && Vector3.Distance(player.transform.position, transform.position) > attackRange)
			//{
			//    tempMoveSpeed = movementSpeed;

			//    Vector3 _normalizedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
			//    transform.position = (Vector3.MoveTowards(transform.position, _normalizedPlayerPos, _step));

			//    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_normalizedPlayerPos - transform.position), rotationSpeed * Time.deltaTime);

			//    Debug.Log("InRange");

			//}

		}

		DeathState();
	}

	public virtual void TakeDamage(float damage)
	{
		currentHealth -= damage;
	}

	public virtual void Patrol()
	{

	}

	public virtual void DoAttack()
	{
		if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
			return;

		tempMoveSpeed = 0f;
		attackTimer += Time.deltaTime;

		Debug.Log("ATTACKINGDROID");
		if (EnemyDamageEvent != null)
		{
			EnemyDamageEvent(damage);
		}
	}

	public virtual void DeathState()
	{
		if (currentHealth <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnEnable()
	{
		InputManager.DashAttackEvent += TakeDamage;
	}

	private void OnDisable()
	{
		InputManager.DashAttackEvent -= TakeDamage;
	}
}