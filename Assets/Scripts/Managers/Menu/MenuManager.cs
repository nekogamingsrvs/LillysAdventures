using CnControls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VoidInc
{
	/// <summary>
	/// Manages the main menu.
	/// </summary>
	public class MenuManager : MonoBehaviour
	{
		// Update is called once per frame
		public void OnClickStart()
		{
			SceneManager.LoadScene("level1");
		}

		public void OnClickOptions()
		{

		}

		public void OnClickExit()
		{
			Application.Quit();
		}
	}
}