using UnityEngine;
using UnityEngine.SceneManagement;

// ============================================================================================================================
// CLAEoS
// Change Level At End of Screen
// By: TheSidOfEvil
// ======================================
// Also a pretty nice name :3 - But the inspector really f's up the spacing >:|
// ============================================================================================================================

namespace VoidInc.LWA
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
		public Vector2 topLeft = new Vector2(-16, 16), topRight = new Vector2(16, 16), bottomLeft = new Vector2(-16, -16), bottomRight = new Vector2(16, -16);
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
		[HideInInspector]
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
		/// <summary>
		/// 
		/// </summary>
		private GameDataManager gameDataManager;

		// Start's the script.
		public void Start()
		{
			Player = GameObject.FindGameObjectWithTag("Player");

			gameDataManager = FindObjectOfType<GameDataManager>();

			// Get the setting to tell where to teleport to.
			if (gameDataManager.SaveFile.PlayerData.PlayerTranisitionTeleTo == gameObject.name)
			{
				// Set the player's position.
				Player.transform.position = new Vector3(gameObject.transform.position.x + SpawnPlayerAtX, gameDataManager.SaveFile.PlayerData.PlayerTransitionPositionY, 0);
			}
		}

		// Draw the objects.
		public void OnDrawGizmos()
		{
			var boundsActual = new BounderyRect();
			boundsActual.topLeft = new Vector2((Bounds.topLeft.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.topLeft.y * gameObject.transform.localScale.y) * Bounds.size.y);
			boundsActual.topRight = new Vector2((Bounds.topRight.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.topRight.y * gameObject.transform.localScale.y) * Bounds.size.y);
			boundsActual.bottomLeft = new Vector2((Bounds.bottomLeft.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.bottomLeft.y * gameObject.transform.localScale.y) * Bounds.size.y);
			boundsActual.bottomRight = new Vector2((Bounds.bottomRight.x * gameObject.transform.localScale.x) * Bounds.size.x, (Bounds.bottomRight.y * gameObject.transform.localScale.y) * Bounds.size.y);

			//Spheres
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(new Vector3(SpawnPlayerAtX + gameObject.transform.position.x, gameObject.transform.position.y, 0), 3);
			//Lines
			Gizmos.color = Color.green;
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
				SceneManager.LoadScene("Level" + LevelNumber);
				gameDataManager.SaveFile.PlayerData.Level = LevelNumber;
				gameDataManager.SaveFile.PlayerData.PlayerTransitionPositionY = Player.transform.position.y;
				gameDataManager.SaveFile.PlayerData.PlayerTranisitionTeleTo = OtherCLAEoSToGoTo;
				gameDataManager.Transitioned = true;
			}
		}
	}
}