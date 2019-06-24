using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Enemy_Boss : EnemyParent
{
    EnumStorage.BossEnemyState CurrentState = EnumStorage.BossEnemyState.Idle;

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

    IEnumerator IAttackRanged()
    {
        yield return new WaitForSeconds(1);
        
    }
}