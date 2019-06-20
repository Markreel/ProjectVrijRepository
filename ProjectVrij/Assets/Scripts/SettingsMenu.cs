using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] Animator anim;
	[SerializeField] GameObject settingsObject;

	public Dropdown dropDownResolutions;
	private Resolution[] resolutions;

    private void Start()
    {
		resolutions = Screen.resolutions;
		dropDownResolutions.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " X " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		dropDownResolutions.AddOptions(options);

		dropDownResolutions.value = currentResolutionIndex;
		dropDownResolutions.RefreshShownValue();
	}

	public void GoBack()
	{
		anim.SetBool("isSettings", false);
		settingsObject.SetActive(false);
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}
}