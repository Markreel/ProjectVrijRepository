public class EnumStorage
{
	public enum PlayerState
	{
		Idle,
		Dashing,
		Moving,
		Attacking,
		Dead
	}

	public enum AttackStates
	{
		None,
		Attack1,
		Attack2,
		Attack3
	}

	public enum BasicEnemyState
	{
		Idle,
		Attacking,
		Moving,
		Dead
	}

	public enum BossEnemyState
	{
		Idle,
		AttackingType1,
		AttackingType2,
		Moving,
		Dead
	}
}