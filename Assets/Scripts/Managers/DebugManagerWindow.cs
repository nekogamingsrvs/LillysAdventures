using UnityEngine;
using System.Collections;
using UnityEditor;

public class DebugManagerWindow : EditorWindow
{
	[HideInInspector]
	public Vector3 PlayerPosition;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/My Window")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		DebugManagerWindow window = (DebugManagerWindow)EditorWindow.GetWindow(typeof(DebugManagerWindow));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("Base Data", EditorStyles.boldLabel);
		GUILayout.Label("Player Position (float): (" + PlayerPosition.x + ", " + PlayerPosition.y + ", " + PlayerPosition.z + ")");
		EditorGUILayout.EndToggleGroup();
	}
}
