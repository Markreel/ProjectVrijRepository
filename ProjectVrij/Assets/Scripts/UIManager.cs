using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private InputManager inputManager;
	[SerializeField] private Image dashImage;

    private void Start()
    {
		inputManager = FindObjectOfType<InputManager>();   
    }

    private void Update()
    {
		DashCooldownImage();
	}

	private void DashCooldownImage()
	{
		dashImage.fillAmount = 1 - (1 / inputManager.DashDelay * inputManager.CurrentDashDelay);
	}
}
