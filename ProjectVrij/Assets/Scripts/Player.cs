using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float maxHealth = 100f;
	private float currentHealth;
	private Animator anim;

    private void Awake()
    {
		currentHealth = maxHealth;
		anim = GetComponentInChildren<Animator>();
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		DeathState();
	}

	private void DeathState()
	{
		if(currentHealth <= 0f)
		{
			InputManager.Instance.CurrentPlayerState = EnumStorage.PlayerState.Dead;
			anim.SetBool("isDeath", true);
		}
	}

	private void OnEnable()
	{
		EnemyParent.EnemyDamageEvent += TakeDamage;
	}

	private void OnDisable()
	{
		EnemyParent.EnemyDamageEvent -= TakeDamage;
	}
}