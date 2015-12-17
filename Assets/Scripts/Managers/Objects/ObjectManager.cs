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

		public struct _Sign
		{
			public enum _SignType
			{
				dialog,
				heal,
				save
			}

			public _SignType SignType;

			public string Dialog;

			public int Type;
		}

		/// <summary>
		/// If the item has been destroyed.
		/// </summary>
		[HideInInspector]
		public bool Activated;

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
		private GameManager _GameController;

		/// <summary>
		/// The type of sign that the sign is.
		/// </summary>
		private _Sign _SignType;

		void Awake()
		{
			_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		}

		public void RemoveObject()
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
			if (_GameController.Keys >= 1 && _GameController.KeyIdentifiers[KeyID] == Identifier)
			{
				_GameController.DestroyedGameObjects.Add(gameObject);
				_GameController.Keys -= 1;
				_GameController.KeyIdentifiers.Remove(KeyID);
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