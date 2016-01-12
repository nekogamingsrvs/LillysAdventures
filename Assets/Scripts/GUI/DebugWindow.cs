using UnityEngine;

namespace VoidInc.LWA
{
	/// <summary>
	/// The debug window for debugging Lilly's Wonderful Adventures
	/// </summary>
	public class DebugWindow : MonoBehaviour
	{
		/// <summary>
		/// The debug window's size.
		/// </summary>
		public Rect DebugWindowRect = new Rect(100, 100, 250, 300);
		/// <summary>
		/// The GameManager to get data from.
		/// </summary>
		[HideInInspector]
		private GameManager _GameManager;
		/// <summary>
		/// The position of the scroll frame.
		/// </summary>
		private Vector2 ScrollPosition;

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
				DebugWindowRect = GUI.Window(0, DebugWindowRect, DebugWindowF, "Debug + " + ScrollPosition.y);
			}
		}

		void DebugWindowF(int windowID)
		{
			GUI.DragWindow(new Rect(0, 0, 250, 25));
			ScrollPosition = GUI.BeginScrollView(new Rect(5, 25, 240, 285), ScrollPosition, new Rect(5, 25, 235, 1000), false, true);
			GUI.Label(new Rect(5, 25, 120, 25), "Player Position");
			GUI.Label(new Rect(5, 50, 70, 25), " x=" + _GameManager.PlayersPosition.x, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(80, 50, 70, 25), " y=" + _GameManager.PlayersPosition.y, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 75, 70, 25), " z=" + _GameManager.PlayersPosition.z, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 100, 125, 25), "Game Time");
			GUI.Label(new Rect(5, 125, 70, 25), "Current=" + _GameManager.CurrentTime, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 150, 70, 25), "Save Time=" + _GameManager.SaveTime, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 175, 120, 25), "Player Collisions");
			GUI.Label(new Rect(5, 200, 70, 25), _GameManager.PlayerController.NameOfCollision, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 225, 120, 25), "Player Details");
			GUI.Label(new Rect(5, 250, 70, 25), "IsClimbing=" + _GameManager.PlayerController.IsClimbing, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(120, 250, 70, 25), "Climb=" + _GameManager.PlayerController.Climb, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 275, 70, 25), "Velocity=" + _GameManager.PlayerController.Velocity, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 300, 70, 25), "IsRunning=" + _GameManager.PlayerController.IsRunning, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(120, 300, 70, 25), "IsJumping=" + _GameManager.PlayerController.IsJumping, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.Label(new Rect(5, 325, 70, 25), "Modifier=" + _GameManager.PlayerController.Modifier, new GUIStyle { overflow = new RectOffset(0, 1, 0, 25) });
			GUI.EndScrollView();
		}
	}
}
