using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class EnemyParent : MonoBehaviour
{
    public static Action<float> EnemyDamageEvent;

    [Header("Settings: ")]
    [SerializeField] private bool isBoss = false; 

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float damage = 5f;

    [SerializeField] private float playerSpottedRange = 10f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackCooldownTimer = 3f;
    [SerializeField] private float attackDistance = 2;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject playerChecker;

    [Header("References: ")]
    [SerializeField] private InputManager player;
    [SerializeField] private GameObject movementCamPrefab;
    [HideInInspector] public GameObject movementCam;
    private Animator anim;

    private float rotationSpeed = 10f;
    private float tempMoveSpeed;
    private float currentHealth;

    public InputManager Player { set { player = value; } }
    private float distanceBetweenPlayer { get { return Mathf.Abs(player.CurrentPos - movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition); } }
    private bool isTurned { get { return player.CurrentPos < movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition ? true : false; } }


    public virtual void Awake()
    {
        ResetHealth();
        anim = GetComponentInChildren<Animator>();
    }

    public void InitializeMovementCam(float _pathPosition)
    {
        movementCam = Instantiate(movementCamPrefab, transform);

        CinemachineVirtualCamera _vCam = movementCam.GetComponent<CinemachineVirtualCamera>();

        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("DollyTrack1").GetComponent<CinemachinePathBase>();
        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;
        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = _pathPosition;

        if (isBoss)
            _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset = new Vector3(-25, 0, 0);

        //gameObject.transform.position = _vCam.transform.position;
        //Debug.Log(_vCam.transform.position);

        _vCam.Follow = transform;
        // movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().track
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public virtual void Update()
    {
        transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);

        if (player != null)
        {
            if (distanceBetweenPlayer <= playerSpottedRange && distanceBetweenPlayer > attackRange)
            {
                LookAtPlayer();
                MoveTowardsPlayer();
            }

            if (distanceBetweenPlayer <= attackRange)
                LookAtPlayer();


            if (Physics.Raycast(playerChecker.transform.position, transform.forward * attackDistance, attackDistance, playerMask) && canAttack)
            {
                LookAtPlayer();
                anim.SetTrigger("Attack");
                canAttack = false;
            }

            //else
            //{
            //    anim.SetBool("isAttacking", false);
            //    canAttack = true;
            //}

            //if (Vector3.Distance(player.transform.position, transform.position) <= playerSpottedRange && Vector3.Distance(player.transform.position, transform.position) > attackRange)
            //{
            //    tempMoveSpeed = movementSpeed;

            //    Vector3 _normalizedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            //    transform.position = (Vector3.MoveTowards(transform.position, _normalizedPlayerPos, _step));

            //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_normalizedPlayerPos - transform.position), rotationSpeed * Time.deltaTime);

            //    Debug.Log("InRange");

            //}
            //if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            //{
            //    tempMoveSpeed = 0f;
            //    //play attack state
            //    Debug.Log("ATTACK");
            //    DoAttack();
            //}
        }

        Debug.DrawLine(playerChecker.transform.position, playerChecker.transform.position + transform.forward * attackDistance);

        CheckDeathState();
    }

    void MoveTowardsPlayer()
    {
        CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
        float _pathLenght = _dolly.m_Path.PathLength;
        float _step = movementSpeed * Time.deltaTime; // calculate distance to move

        tempMoveSpeed = (isTurned ? -Time.deltaTime : Time.deltaTime) * movementSpeed;
        _dolly.m_PathPosition = Mathf.Clamp(_dolly.m_PathPosition + tempMoveSpeed, 0, _pathLenght);
    }

    void LookAtPlayer()
    {
        Vector3 _normalizedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_normalizedPlayerPos - transform.position), rotationSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        InputManager.DashAttackEvent -= TakeDamage;
		AudioManager.Instance.PlayClip(AudioManager.Instance.swordHitClip);
    }

    public virtual void ReactivateAttackState()
    {
        canAttack = true;
    }

    public virtual void DoAttack()
    {
        if (!Physics.Raycast(playerChecker.transform.position, transform.forward, attackDistance, playerMask))
            return;

        tempMoveSpeed = 0f;

        if (EnemyDamageEvent != null)
        {
            EnemyDamageEvent(damage);
        }
    }

    public virtual void CheckDeathState()
    {
		if (currentHealth <= 0 && !isBoss)
		{
			movementSpeed = 0;
			rotationSpeed = 90;
			anim.SetBool("isDeath", true);
			SpawnManager.Instance.RemoveEnemy(gameObject);
		}

		if (currentHealth <= 0 && isBoss)
        {
            SpawnManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
	}
	
	public virtual void DestroyGameObject()
	{
		Destroy(gameObject);
	}
}