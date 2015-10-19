using UnityEngine;
using System.Collections;
using CnControls;

/// <summary>
/// Manages the main menu.
/// </summary>
public class MenuManager : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			// Start the game when pressing space.
			if (Input.GetKeyUp(KeyCode.Space))
			{
				Application.LoadLevel("level1");
			}
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			if (CnInputManager.GetButtonUp("Jump"))
			{
				Application.LoadLevel("level1");
			}
		}
	}
}
