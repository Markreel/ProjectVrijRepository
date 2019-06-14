using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
	public CinemachineVirtualCamera virtualCamera;

	[SerializeField] private float shakeAmplitude = 1.5f;
	[SerializeField] private float shakeFrequence = 2.0f;

	private float shakeDuration = 0f;
	private float shakeElapsedTime = 0f;
	private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

	private void Start()
    {
        if(virtualCamera != null)
		{
			virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
		}
    }

   private void Update()
   {
		if (virtualCamera != null || virtualCameraNoise != null)
		{
			if(shakeElapsedTime > 0)
			{
				virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
				virtualCameraNoise.m_FrequencyGain = shakeFrequence;

				shakeElapsedTime -= Time.deltaTime;
			}
			else
			{
				virtualCameraNoise.m_AmplitudeGain = 0f;

				shakeElapsedTime = 0f;
			}
		}
    }

	private void ApplyShake(float duration)
	{
		shakeElapsedTime = duration;
	}

	private void OnEnable()
	{
		InputManager.DashAttackEvent += ApplyShake;
	}

	private void OnDisable()
	{
		InputManager.DashAttackEvent += ApplyShake;
	}
}