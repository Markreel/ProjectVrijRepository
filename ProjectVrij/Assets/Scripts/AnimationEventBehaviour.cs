using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventBehaviour : MonoBehaviour
{
	public UnityEvent OnEvent;

	public void ExecuteEvent()
	{
		OnEvent.Invoke();
	}
}