using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
	[SerializeField] private bool destroyAfterLifeTime = true;

	ParticleSystem ps;

	private void Awake()
	{
		ps = GetComponentInChildren<ParticleSystem>();
	}

	private void Update()
	{
		if(!ps.isPlaying && destroyAfterLifeTime)
		{
			Destroy(gameObject);
		}
	}
}