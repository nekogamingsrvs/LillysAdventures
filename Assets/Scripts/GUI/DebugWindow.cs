using UnityEngine;
using System.Collections;
using VoidInc;

public class DebugWindow : MonoBehaviour
{

	public Rect DebugWindowRect = new Rect(100, 100, 250, 300);

	[HideInInspector]
	private GameManager _GameManager;

	void Awake()
	{
		_GameManager = gameObject.GetComponent<GameManager>();
	}

	void OnGUI()
	{
		if (!InputCheck.IsEditorPlatforms)
		{
			return;
		}
		else
		{
			DebugWindowRect = GUI.Window(0, DebugWindowRect, DebugWindowF, "Debug");
		}
	}

	void DebugWindowF(int windowID)
	{
		GUI.DragWindow();
		GUI.Label(new Rect(5, 25, 125, 25), "Player Position");
		GUI.Label(new Rect(5, 50, 70, 25), " x : " + _GameManager.PlayersPosition.x, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) } );
		GUI.Label(new Rect(80, 50, 70, 25), " y : " + _GameManager.PlayersPosition.y, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) } );
		GUI.Label(new Rect(5, 75, 70, 25), " z : " + _GameManager.PlayersPosition.z, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) } );
	}
}
