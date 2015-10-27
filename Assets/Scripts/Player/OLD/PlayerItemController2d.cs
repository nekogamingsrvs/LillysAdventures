using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
[RequireComponent(typeof(MasterPlayerController2d), typeof(CharacterController2D))]
class PlayerItemController2d : PlayerController2d
{
	protected override void OnAwake()
	{
		Trigger2DEvents itemTriggerEvents = this.transform.Find("ItemCheck").GetComponent<Trigger2DEvents>();
		itemTriggerEvents.OnTriggerEnter2DEvent += new Trigger2DEvents.OnTriggerEnter2DHandler(Item_OnTriggerEnter2DEvent);
		itemTriggerEvents.OnTriggerExit2DEvent += new Trigger2DEvents.OnTriggerExit2DHandler(Item_OnTriggerExit2DEvent);
	}

	void Item_OnTriggerEnter2DEvent(Collider2D collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Coins1"))
		{
			collider.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
		}
		if (collider.gameObject.layer == LayerMask.NameToLayer("Coins5"))
		{
			collider.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
		}
		if (collider.gameObject.layer == LayerMask.NameToLayer("Coins10"))
		{
			collider.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
		}
		if (collider.gameObject.layer == LayerMask.NameToLayer("Jems"))
		{
			collider.gameObject.GetComponent<JemIdentifier>().RemoveJem();
		}
		if (collider.gameObject.layer == LayerMask.NameToLayer("Keys"))
		{
			collider.gameObject.GetComponent<KeyIdentifier>().RemoveKey();
		}
	}

	void Item_OnTriggerExit2DEvent(Collider2D collider)
	{
	}

	protected override void OnControllerEnabled(bool enabled)
	{
	}

	protected override void OnUpdate()
	{

	}
}
*/