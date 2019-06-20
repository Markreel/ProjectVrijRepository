using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] Animator anim;
	[SerializeField] GameObject settingsObject;

	private void Start()
	{
		settingsObject.SetActive(false);
	}

	public void NewGame()
	{
		SceneManager.LoadScene(1);	
		//LoadsceneASync with loadingScreen
	}

	public void Options()
	{
		settingsObject.SetActive(true);
		anim.SetBool("isSettings", true);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
