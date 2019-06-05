using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventBehaviour : MonoBehaviour
{
	public List<UnityEvent> OnEvent = new List<UnityEvent>();

	public void ExecuteEvent(int _index)
	{
		OnEvent[_index].Invoke();
	}
}