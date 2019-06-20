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
			virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

    private void ApplyShake(float _duration, float _amplitude, float _frequency)
    {
        shakeElapsedTime = _duration;
        shakeAmplitude = _amplitude;
        shakeFrequence = _frequency;
    }

	private void ApplyDashShake(float duration)
	{
        ApplyShake(0.1f, 1, 1);
	}

	private void OnEnable()
	{
		InputManager.DashAttackEvent += ApplyDashShake;
	}

	private void OnDisable()
	{
		InputManager.DashAttackEvent += ApplyDashShake;
	}
}