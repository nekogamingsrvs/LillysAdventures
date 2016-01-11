using UnityEngine;
using UnityEngine.SceneManagement;

// ============================================================================================================================
// CLAEoS
// Change Level At End of Screen
// By: TheSidOfEvil
// ======================================
// Also a pretty nice name :3 - But the inspector really f's up the spacing >:|
// ============================================================================================================================

namespace VoidInc
{
	[System.Serializable]
	public class BounderyRect
	{
		/// <summary>
		/// The size of the bounding area.
		/// </summary>
		public Vector2 size = new Vector2(1, 1);

		/// <summary>
		/// The bounds of the bounding area?
		/// </summary>
		[HideInInspector]
		public Vector2 topLeft = new Vector2(-8, 8), topRight = new Vector2(8, 8), bottomLeft = new Vector2(-8, -8), bottomRight = new Vector2(8, -8);
	}

	[ExecuteInEditMode]
	public class CLAEoS : MonoBehaviour
	{
		/// <summary>
		/// The bounds of the area.
		/// </summary>
		public BounderyRect Bounds;

		/// <summary>
		/// The player's GameObject.
		/// </summary>
		public GameObject Player;

		/// <summary>
		/// The level to go to.
		/// </summary>
		public int LevelNumber;

		/// <summary>
		/// If the player can go to the next area.
		/// </summary>
		public bool CanTransition;

		/// <summary>
		/// The any other GameObject with this script on it.
		/// </summary>
		public string OtherCLAEoSToGoTo;

		/// <summary>
		/// Spawn's the player at X position.
		/// </summary>
		public float SpawnPlayerAtX = 0;

		/// <summary>
		/// The Y position when entering the area.
		/// </summary>
		[HideInInspector]
		public float YPositionWhileEntering;

		[HideInInspector]
		public SaveFile SaveFile;

		// Start's the script.
		public void Start()
		{
			SaveFile = FindObjectOfType<GameDataManager>().SaveFile;

			// Get the setting to tell where to teleport to.
			if (SaveFile.PlayerData.PlayerPositionWER_TeleTo == gameObject.name)
			{
				// Set the player's position.
				Player.transform.position = new Vector3(gameObject.transform.position.x + SpawnPlayerAtX, SaveFile.PlayerData.PlayerPositionWER_Y, 0);
			}
		}

		// Draw the objects.
		public void OnDrawGizmos()
		{
			Gizmos.color = Color.green;

			var boundsActual = new BounderyRect();

			boundsActual.topLeft = new Vector2((Bounds.topLeft.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.topLeft.y * gameObject.transform.localScale.y) * Bounds.size.y);

			boundsActual.topRight = new Vector2((Bounds.topRight.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.topRight.y * gameObject.transform.localScale.y) * Bounds.size.y);

			boundsActual.bottomLeft = new Vector2((Bounds.bottomLeft.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.bottomLeft.y * gameObject.transform.localScale.y) * Bounds.size.y);

			boundsActual.bottomRight = new Vector2((Bounds.bottomRight.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.bottomRight.y * gameObject.transform.localScale.y) * Bounds.size.y);

			Gizmos.color = Color.red;
			//Spheres
			Gizmos.DrawSphere(new Vector3(SpawnPlayerAtX + gameObject.transform.position.x, gameObject.transform.position.y, 0), 3);

			Gizmos.color = Color.green;
			//Lines
			Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0), new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0));

			Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0));

			Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0));

			Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0), new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0));
		}

		// Runs when updating.
		public void Update()
		{
			var boundsActualWS = new BounderyRect();

			boundsActualWS.topLeft = new Vector2(((Bounds.topLeft.x * gameObject.transform.localScale.x) * Bounds.size.x) + gameObject.transform.position.x, ((Bounds.topLeft.y * gameObject.transform.localScale.y) * Bounds.size.y) + gameObject.transform.position.y);

			boundsActualWS.topRight = new Vector2(((Bounds.topRight.x * gameObject.transform.localScale.x) * Bounds.size.x) + gameObject.transform.position.x, ((Bounds.topRight.y * gameObject.transform.localScale.y) * Bounds.size.y) + gameObject.transform.position.y);

			boundsActualWS.bottomLeft = new Vector2(((Bounds.bottomLeft.x * gameObject.transform.localScale.x) * Bounds.size.x) + gameObject.transform.position.x, ((Bounds.bottomLeft.y * gameObject.transform.localScale.y) * Bounds.size.y) + gameObject.transform.position.y);

			boundsActualWS.bottomRight = new Vector2(((Bounds.bottomRight.x * gameObject.transform.localScale.x) * Bounds.size.x) + gameObject.transform.position.x, ((Bounds.bottomRight.y * gameObject.transform.localScale.y) * Bounds.size.y) + gameObject.transform.position.y);

			// Check if player is in bounds, and load level.
			if (CanTransition && Player.transform.position.x > boundsActualWS.topLeft.x && Player.transform.position.x < boundsActualWS.bottomRight.x && Player.transform.position.y > boundsActualWS.bottomLeft.y && Player.transform.position.y < boundsActualWS.topRight.y)
			{
				SceneManager.MoveGameObjectToScene(GameObject.Find("GameDataManager"), SceneManager.GetSceneByName("level" + LevelNumber));
				SceneManager.LoadScene("level" + LevelNumber);
				SaveFile.PlayerData.Level = LevelNumber;
				SaveFile.PlayerData.PlayerPositionWER_Y = Player.transform.position.y;
				SaveFile.PlayerData.PlayerPositionWER_TeleTo = OtherCLAEoSToGoTo;
				SaveFile.PlayerData.Transitioned = true;
			}
		}
	}
}