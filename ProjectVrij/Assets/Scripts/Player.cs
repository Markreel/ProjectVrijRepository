using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float maxHealth = 100f;
	private float currentHealth;
	private Animator anim;

    // Start is called before the first frame update
    private void Awake()
    {
		currentHealth = maxHealth;
		anim = GetComponentInChildren<Animator>();
	}

    private void Update()
    {
		DeathState();
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		Debug.Log("PlayerHealth: " + currentHealth);
	}

	private void DeathState()
	{
		if(currentHealth <= 0f)
		{ 
			Destroy(this.gameObject);
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