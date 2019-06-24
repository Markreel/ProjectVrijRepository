using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInstantiator : MonoBehaviour
{
	public static ParticleInstantiator Instance { get { return GetInstance(); } }

	#region Singleton

	private static ParticleInstantiator instance;
	private static ParticleInstantiator GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<ParticleInstantiator>();
		}
		return instance;
	}
	#endregion

    public void SpawnParticle(GameObject prefab, Vector3 position, Vector3 rotation, Transform parent = null)
	{
		Instantiate(prefab, position, Quaternion.Euler(rotation), parent);
	}
}