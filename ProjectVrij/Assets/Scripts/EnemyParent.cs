using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyParent : MonoBehaviour
{
	public static Action<float> EnemyDamageEvent;

	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float movementSpeed = 4f;
	[SerializeField] private float damage = 5f;

	[SerializeField] private float playerSpottedRange = 10f;
	[SerializeField] private float attackRange = 3f;
	[SerializeField] private GameObject player;
	[SerializeField] private float attackCooldownTimer = 3f;

	private float rotationSpeed = 10f;
	private float tempMoveSpeed;
	private float attackTimer;
	private bool canAttack;
	private float currentHealth;

	public virtual void Awake()
	{
		ResetHealth();
	}

	private void ResetHealth()
	{
		currentHealth = maxHealth;
	}

    public virtual void Update()
    {
		float _step = movementSpeed * Time.deltaTime; // calculate distance to move

		if (player != null)
		{
			if (Vector3.Distance(player.transform.position, transform.position) <= playerSpottedRange && Vector3.Distance(player.transform.position, transform.position) > attackRange)
			{
				tempMoveSpeed = movementSpeed;

				Vector3 _normalizedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
				transform.position = (Vector3.MoveTowards(transform.position, _normalizedPlayerPos, _step));

				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_normalizedPlayerPos - transform.position), rotationSpeed * Time.deltaTime);

				Debug.Log("InRange");

			}
			if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
			{
				tempMoveSpeed = 0f;
				//play attack state
				Debug.Log("ATTACK");
				DoAttack();
			}
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
		attackTimer += Time.deltaTime;

		if(attackTimer >= attackCooldownTimer)
		{
			canAttack = true;
			attackTimer = 0;
		}

		if(canAttack)
		{
			canAttack = false;

			if (EnemyDamageEvent != null)
			{
				EnemyDamageEvent(damage);
			}
		}
	}

	public virtual void DeathState()
	{
		if(currentHealth <= 0)
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