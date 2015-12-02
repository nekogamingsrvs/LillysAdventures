using UnityEditor;
using UnityEngine;

public class DebugManagerWindow : EditorWindow
{
	public GameObject player = null;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/DebugManagerWindow")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		EditorWindow.GetWindow(typeof(DebugManagerWindow));
	}
	
	void OnGUI()
	{
		player = (GameObject)EditorGUILayout.ObjectField("Player", player, typeof(GameObject), true);
		if (player != null)
		{
			EditorGUILayout.LabelField("x: " + player.transform.position.x);
			EditorGUILayout.LabelField("y: " + player.transform.position.y);
			EditorGUILayout.LabelField("z: " + player.transform.position.z);
		}
	}
}
