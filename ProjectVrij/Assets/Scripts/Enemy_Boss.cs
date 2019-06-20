public class Enemy_Boss : EnemyParent
{
    EnumStorage.BossEnemyState CurrentState = EnumStorage.BossEnemyState.Idle;



	public override void DoAttack()
	{
		base.DoAttack();
	}

	public override void CheckDeathState()
	{
		base.CheckDeathState();
	}
}