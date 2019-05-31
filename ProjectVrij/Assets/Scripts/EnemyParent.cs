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
    private GameObject movementCam;

    private float rotationSpeed = 10f;
    private float tempMoveSpeed;
    private float attackTimer;
    private bool canAttack;
    private float currentHealth;

    public InputManager Player { set { player = value; } }
    private float distanceBetweenPlayer { get { return Mathf.Abs(player.CurrentPos - movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition); } }
    private bool isTurned { get { return player.CurrentPos < movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition ? true : false; } }


    public virtual void Awake()
    {
        ResetHealth();
    }

    public void InitializeMovementCam(float _pathPosition)
    {
        movementCam = Instantiate(movementCamPrefab);

        CinemachineVirtualCamera _vCam = movementCam.GetComponent<CinemachineVirtualCamera>();

        _vCam.Follow = transform;
        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("DollyTrack1").GetComponent<CinemachinePathBase>();
        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;
        _vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = _pathPosition;
        // movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().track
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public virtual void Update()
    {
        if (player != null)
        {
            if (distanceBetweenPlayer <= playerSpottedRange && distanceBetweenPlayer > attackRange)
            {
                LookAtPlayer();
                MoveTowardsPlayer();
            }

            else if (distanceBetweenPlayer <= attackRange)
            {
                LookAtPlayer();

                tempMoveSpeed = 0f;
                //play attack state
                //Debug.Log("ATTACK");
                DoAttack();
            }

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

        CheckDeathState();
    }

    void MoveTowardsPlayer()
    {
        CinemachineTrackedDolly _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
        float _pathLenght = _dolly.m_Path.PathLength;
        float _step = movementSpeed * Time.deltaTime; // calculate distance to move

        tempMoveSpeed = (isTurned ? -Time.deltaTime : Time.deltaTime) * movementSpeed;
        _dolly.m_PathPosition = Mathf.Clamp(_dolly.m_PathPosition + tempMoveSpeed, 0, _pathLenght);

        transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
    }

    void LookAtPlayer()
    {
        Vector3 _normalizedPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_normalizedPlayerPos - transform.position), rotationSpeed * Time.deltaTime);
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

        if (attackTimer >= attackCooldownTimer)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;

            if (EnemyDamageEvent != null)
            {
                EnemyDamageEvent(damage);
            }
        }
    }

    public virtual void CheckDeathState()
    {
        if (currentHealth <= 0)
        {
            SpawnManager.Instance.RemoveEnemy(gameObject);
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