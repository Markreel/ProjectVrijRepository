using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private InputManager inputManager;
	[SerializeField] private Image dashImage;

    // Start is called before the first frame update
    void Start()
    {
		inputManager = FindObjectOfType<InputManager>();   
    }

    // Update is called once per frame
    void Update()
    {
		DashCooldownImage();

	}

	private void DashCooldownImage()
	{
		dashImage.fillAmount = 1 / inputManager.DashDelay * inputManager.CurrentDashDelay;
	}
}
