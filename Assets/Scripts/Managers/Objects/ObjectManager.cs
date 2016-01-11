using UnityEngine;

namespace VoidInc
{
	public class ObjectManager : MonoBehaviour
	{
		/// <summary>
		/// The item type enum to determine the item function.
		/// </summary>
		public enum ObjectType
		{
			Lock,
			ItemBlock,
			ObjectBlock,
			TrapBlock,
			Sign
		}

		/// <summary>
		/// The parameters for the sign.
		/// </summary>
		public struct Sign
		{
			/// <summary>
			/// The type of sign.
			/// </summary>
			public enum SignType
			{
				dialog,
				heal,
				save
			}

			/// <summary>
			/// The variable for the type of sign.
			/// </summary>
			public SignType Type;

			/// <summary>
			/// The dialog for the sign.
			/// </summary>
			public string Dialog;
		}

		public struct TrapBlock
		{
			public enum TrapDeployDir
			{
				above,
				below,
				left,
				right
			}

			public TrapDeployDir Direction;

			public string Type;
        }

		/// <summary>
		/// If the item has been destroyed.
		/// </summary>
		[HideInInspector]
		public bool Activated;

		/// <summary>
		/// The type of sign that the sign is.
		/// </summary>
		[HideInInspector]
		public Sign SignType;

		[HideInInspector]
		public TrapBlock TrapBlockType;

		/// <summary>
		/// What type of item is the item.
		/// </summary>
		public ObjectType Type;

		/// <summary>
		/// The identifier for keys to open locks and for locks to be opened by keys.
		/// </summary>
		public string Identifier;

		/// <summary>
		/// The key id to check the identifier with.
		/// </summary>
		public int KeyID;

		/// <summary>
		/// The GameObject to spawn when an item or object block is triggered.
		/// </summary>
		public GameObject GameObjectSpawn;

		/// <summary>
		/// The GameManager class for the items.
		/// </summary>
		private GameManager _GameManager;

		void Awake()
		{
			_GameManager = FindObjectOfType<GameManager>();

			if (GameObjectSpawn != null)
			{
				GameObjectSpawn.transform.position = gameObject.transform.position;
				GameObjectSpawn.SetActive(false);
			}
		}

		/// <summary>
		/// Activates the objects when the player is activating them.
		/// </summary>
		public void ActivateObject(PlayerController2D player)
		{
			switch (Type)
			{
				case ObjectType.ItemBlock:
					TriggerItemBlock(player);
					break;
				case ObjectType.Lock:
					CheckLock(player);
					break;
				case ObjectType.ObjectBlock:
					TriggerObjectBlock(player);
					break;
				case ObjectType.Sign:
					TriggerSign(player);
					break;
				case ObjectType.TrapBlock:
					TriggerTrapBlock(player);
					break;
				default:
					break;
			}
		}

		private void CheckLock(PlayerController2D player)
		{
			if (_GameManager.GameDataManager.Keys >= 1 && _GameManager.GameDataManager.KeyIdentifiers[KeyID] == Identifier)
			{
				_GameManager.GameDataManager.DestroyedGameObjects.Add(gameObject.GetInstanceID());
				_GameManager.GameDataManager.Keys -= 1;
				_GameManager.GameDataManager.KeyIdentifiers.Remove(KeyID);
				Destroy(gameObject);
			}
		}

		private void TriggerItemBlock(PlayerController2D player)
		{
			if (player.Controller.collisionState.above)
			{
				GameObjectSpawn.SetActive(true);
				GameObjectSpawn.transform.localPosition = new Vector3(0, -32, 0);
			}
		}

		private void TriggerSign(PlayerController2D player)
		{
			if (SignType.Type == Sign.SignType.dialog)
			{

			}
		}

		private void TriggerTrapBlock(PlayerController2D player)
		{
			switch (TrapBlockType.Direction)
			{
				case TrapBlock.TrapDeployDir.above:
					if (player.Controller.collisionState.below)
					{
						GameObjectSpawn.SetActive(true);
						Debug.Log("TrapBlock: Activated()");
					}
					break;
				case TrapBlock.TrapDeployDir.below:
					if (player.Controller.collisionState.above)
					{
						GameObjectSpawn.SetActive(true);
						Debug.Log("TrapBlock: Activated()");
					}
					break;
				case TrapBlock.TrapDeployDir.left:
					if (player.Controller.collisionState.right)
					{
						GameObjectSpawn.SetActive(true);
						Debug.Log("TrapBlock: Activated()");
					}
					break;
				case TrapBlock.TrapDeployDir.right:
					if (player.Controller.collisionState.left)
					{
						GameObjectSpawn.SetActive(true);
						Debug.Log("TrapBlock: Activated()");
					}
					break;
			}

		}

		private void TriggerObjectBlock(PlayerController2D player)
		{

		}

		public void DoUpdate()
		{
		}
	}
}