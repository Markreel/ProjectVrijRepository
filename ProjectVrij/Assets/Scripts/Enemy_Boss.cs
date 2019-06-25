using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Enemy_Boss : EnemyParent
{
    EnumStorage.BossEnemyState CurrentState = EnumStorage.BossEnemyState.Idle;

    Coroutine attackBehaviourRoutine;
    CinemachineTrackedDolly _dolly;

    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;

    public void Start()
    {
        StartAttackingBehaviour();
    }

    public override void Update()
    {
        switch (CurrentState)
        {
            default:
            case EnumStorage.BossEnemyState.Idle:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
            case EnumStorage.BossEnemyState.AttackingMelee:

                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);

                if (player != null)
                {
                    if (distanceBetweenPlayer <= playerSpottedRange && distanceBetweenPlayer > attackRange)
                    {
                        LookAtPlayer();
                        MoveTowardsPlayer();
                    }

                    if (distanceBetweenPlayer <= attackRange)
                    {
                        LookAtPlayer();
                        anim.SetTrigger("AttackMelee");
                        //transform.eulerAngles += Vector3.up * 180;
                        canAttack = false;
                    }
                }

                CheckDeathState();
                break;
            case EnumStorage.BossEnemyState.AttackingRanged:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                MoveTowardsPlayer();
                break;
            case EnumStorage.BossEnemyState.Moving:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
            case EnumStorage.BossEnemyState.Dead:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
        }
    }

    public void DoRangedAttack(bool _isLeftArm)
    {
        Vector3 _pos = _isLeftArm ? leftArm.transform.position : rightArm.transform.position;
        RaycastHit _hit;
        Debug.Log("Hitting Player With arm");

        if (!Physics.SphereCast(_pos, 30, Vector3.forward, out _hit, 1, playerMask))
        {
            return;
        }

        tempMoveSpeed = 0f;

        if (EnemyDamageEvent != null)
        {
            EnemyDamageEvent(damage);
        }
    }

    public override void DoAttack()
	{
        RaycastHit _hit;

        if (!Physics.SphereCast(playerChecker.transform.position, 30, playerChecker.transform.forward, out _hit, 1, playerMask))
        {
            return;
        }

        else
            Debug.Log("Hitting Player");

        tempMoveSpeed = 0f;

        if (EnemyDamageEvent != null)
        {
            EnemyDamageEvent(damage);
        }
    }

	public override void CheckDeathState()
	{
		base.CheckDeathState();
	}

    public void StartAttackingBehaviour()
    {
        attackBehaviourRoutine = StartCoroutine(IAttackBehaviour());
    }

    IEnumerator IAttackBehaviour()
    {
        _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();

        yield return new WaitForSeconds(5);
        CurrentState = EnumStorage.BossEnemyState.AttackingRanged;

        anim.SetTrigger("AttackLeftArm");
        yield return new WaitForSeconds(10);

        anim.SetTrigger("AttackRightArm");
        yield return new WaitForSeconds(10);

        _dolly.m_PathOffset.x = 0;
        CurrentState = EnumStorage.BossEnemyState.AttackingMelee;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(playerChecker.transform.position, 30);

        //Gizmos.DrawSphere(leftArm.transform.position, 30);
        //Gizmos.DrawSphere(rightArm.transform.position, 30);
    }
}