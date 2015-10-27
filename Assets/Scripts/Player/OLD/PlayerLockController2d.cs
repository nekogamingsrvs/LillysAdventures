using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
[RequireComponent(typeof(MasterPlayerController2d), typeof(CharacterController2D))]
class PlayerLockController2d : PlayerController2d
{
	protected override void OnAwake()
	{
		Trigger2DEvents lockTriggerEvents = this.transform.Find("LockCheck").GetComponent<Trigger2DEvents>();
		lockTriggerEvents.OnTriggerEnter2DEvent += new Trigger2DEvents.OnTriggerEnter2DHandler(Lock_OnTriggerEnter2DEvent);
		lockTriggerEvents.OnTriggerExit2DEvent += new Trigger2DEvents.OnTriggerExit2DHandler(Lock_OnTriggerExit2DEvent);
	}

	void Lock_OnTriggerEnter2DEvent(Collider2D collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Locks"))
		{
			collider.gameObject.GetComponent<LockIdentifier>().CheckLock();
		}
	}

	void Lock_OnTriggerExit2DEvent(Collider2D collider)
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