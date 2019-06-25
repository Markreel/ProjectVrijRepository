using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float maxHealth = 100f;
    public float MaxHealth { get { return maxHealth; } }
	private float currentHealth;
	private Animator anim;
	[SerializeField] private UIManager uiManager;

    private void Awake()
    {
		currentHealth = maxHealth;
		anim = GetComponentInChildren<Animator>();
	}

	public void TakeDamage(float _amount)
	{
		currentHealth -= _amount;
        PPManager.Instance.ShiftSaturation(currentHealth - maxHealth);
        CameraShake.Instance.ApplyShake(0.2f, 10f, 1f);
        CheckDeathState();
	}

    public void GainHealth(float _amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _amount, 0, maxHealth);
        PPManager.Instance.ShiftSaturation(currentHealth - maxHealth);
    }

	private void CheckDeathState()
	{
		if(currentHealth <= 0f)
		{
			InputManager.Instance.CurrentPlayerState = EnumStorage.PlayerState.Dead;
			anim.SetBool("isDeath", true);
		}
	}

	public void StartLoseScreen()
	{
		uiManager.GameOverScreen();
	}

	private void OnEnable()
	{
		EnemyParent.EnemyDamageEvent += TakeDamage;
	}

	private void OnDisable()
	{
		EnemyParent.EnemyDamageEvent -= TakeDamage;
	}

    public void StepAudio()
    {
        Debug.Log("voetstap");
    }
}