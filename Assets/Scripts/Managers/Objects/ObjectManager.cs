using UnityEngine;

namespace VoidInc
{
	public class ObjectManager : MonoBehaviour
	{
		/// <summary>
		/// The item type enum to determine the item function.
		/// </summary>
		public enum _ObjectType
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
		public struct _Sign
		{
			/// <summary>
			/// The type of sign.
			/// </summary>
			public enum _SignType
			{
				dialog,
				heal,
				save
			}

			/// <summary>
			/// The variable for the type of sign.
			/// </summary>
			public _SignType SignType;

			/// <summary>
			/// The dialog for the sign.
			/// </summary>
			public string Dialog;
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
		public _Sign _SignType;

		/// <summary>
		/// What type of item is the item.
		/// </summary>
		public _ObjectType ObjectType;

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
		public void ActivateObject()
		{
			switch (ObjectType)
			{
				case _ObjectType.ItemBlock:
					TriggerItemBlock();
					break;
				case _ObjectType.Lock:
					CheckLock();
					break;
				case _ObjectType.ObjectBlock:
					TriggerObjectBlock();
					break;
				case _ObjectType.Sign:
					TriggerSign();
					break;
				case _ObjectType.TrapBlock:
					TriggerTrapBlock();
					break;
				default:
					break;
			}
		}

		private void CheckLock()
		{
			if (_GameManager.GameDataManager.Keys >= 1 && _GameManager.GameDataManager.KeyIdentifiers[KeyID] == Identifier)
			{
				_GameManager.GameDataManager.DestroyedGameObjects.Add(gameObject.GetInstanceID());
				_GameManager.GameDataManager.Keys -= 1;
				_GameManager.GameDataManager.KeyIdentifiers.Remove(KeyID);
				Destroy(gameObject);
			}
		}

		private void TriggerItemBlock()
		{
			GameObjectSpawn.SetActive(true);
			GameObjectSpawn.transform.localPosition = new Vector3(0, -32, 0);
		}

		private void TriggerObjectBlock()
		{
			GameObjectSpawn.SetActive(true);
			GameObjectSpawn.transform.localPosition = new Vector3(0, -32, 0);
		}

		private void TriggerSign()
		{
			if (_SignType.SignType == _Sign._SignType.dialog)
			{

			}
		}

		private void TriggerTrapBlock()
		{

		}

		public void DoUpdate()
		{
		}
	}
}