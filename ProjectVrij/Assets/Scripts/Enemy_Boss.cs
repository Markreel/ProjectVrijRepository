using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Enemy_Boss : EnemyParent
{
    EnumStorage.BossEnemyState CurrentState = EnumStorage.BossEnemyState.Idle;

    Coroutine attackBehaviourRoutine;

    Animator anim;
    CinemachineTrackedDolly _dolly;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        _dolly = movementCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
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
                base.Update();
                break;
            case EnumStorage.BossEnemyState.AttackingRanged:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
            case EnumStorage.BossEnemyState.Moving:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
            case EnumStorage.BossEnemyState.Dead:
                transform.position = new Vector3(movementCam.transform.position.x, transform.position.y, movementCam.transform.position.z);
                break;
        }
    }

    public override void DoAttack()
	{
		base.DoAttack();
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
        _dolly.m_PathOffset.x = 25;

        yield return new WaitForSeconds(5);

        anim.SetTrigger("AttackLeftArm");
        yield return new WaitForSeconds(10);

        anim.SetTrigger("AttackRightArm");
        yield return new WaitForSeconds(10);

        _dolly.m_PathOffset.x = 0;
        CurrentState = EnumStorage.BossEnemyState.AttackingMelee;
    }
}