using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	private InputManager inputManager;
	[SerializeField] private Image dashImage;
	[SerializeField] private GameObject uiEscape;
	[SerializeField] private GameObject loseScreen;
	[SerializeField] private GameObject winScreen;


	private void Awake()
	{
		Time.timeScale = 1;

		uiEscape.SetActive(false);
		loseScreen.SetActive(false);
	}

	public void WinScreen()
	{
		Time.timeScale = 0;
		winScreen.SetActive(true);
	}

	public void Continue()
	{
		uiEscape.SetActive(false);
		Time.timeScale = 1;
	}

	public void GameOverScreen()
	{
		Time.timeScale = 0;
		loseScreen.SetActive(true);
	}

	public void Restart()
	{
		SceneManager.LoadScene(1);
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene(0);
		Time.timeScale = 1;
	}

	private void Start()
    {
		inputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
		DashCooldownImage();
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			uiEscape.SetActive(true);
			Time.timeScale = 0;
		}
	}

	private void DashCooldownImage()
	{
		dashImage.fillAmount = 1 - (1 / inputManager.DashDelay * inputManager.CurrentDashDelay);
	}
}