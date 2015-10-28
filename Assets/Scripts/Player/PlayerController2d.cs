using CnControls;
using Prime31;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoidInc
{
	public class PlayerController2D : MonoBehaviour
	{
		public float Gravity = -25.0f;
		public float RunMultiplierSpeed = 2.0f;
		public float WalkSpeed = 16.0f;
		public float GroundDamping = 20.0f;
		public float InAirDamping = 5.0f;
		public float JumpHeight = 48.0f;
		public static readonly float LadderVelocityPerSecond = 1.0f * 60.0f;

		[HideInInspector]
		private float NormalizedHorizontalSpeed = 0.0f;

		private CharacterController2D _Controller;
		private Animator _Animator;
		private Vector3 _Velocity;
		private bool _OnLadder;
		private bool _IsRunning;
		private bool _IsMeowing;

		#region Platforms
		private RuntimePlatform[] _PCPlatforms =
		{
			RuntimePlatform.OSXEditor,
			RuntimePlatform.OSXPlayer,
			RuntimePlatform.WindowsPlayer,
			RuntimePlatform.OSXWebPlayer,
			RuntimePlatform.OSXDashboardPlayer,
			RuntimePlatform.WindowsWebPlayer,
			RuntimePlatform.WindowsEditor,
			RuntimePlatform.LinuxPlayer,
			RuntimePlatform.WebGLPlayer,
			RuntimePlatform.WSAPlayerX86,
			RuntimePlatform.WSAPlayerX64,
			RuntimePlatform.WSAPlayerARM,
			RuntimePlatform.TizenPlayer
		};

		private RuntimePlatform[] _MobilePlatforms =
		{
			RuntimePlatform.IPhonePlayer,
			RuntimePlatform.Android,
			RuntimePlatform.WP8Player
		};
		#endregion

		// Use this for initialization
		void Awake()
		{
			// Set the animator and controller components.
			_Animator = GetComponent<Animator>();
			_Controller = GetComponent<CharacterController2D>();

			// listen to some events for illustration purposes
			_Controller.onControllerCollidedEvent += onControllerCollider;
			_Controller.onTriggerEnterEvent += onTriggerEnterEvent;
			_Controller.onTriggerExitEvent += onTriggerExitEvent;

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityX", _Velocity.x);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("OnLadder", _OnLadder);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().AddToDatabase("IsGrounded", _Controller.isGrounded);
			}
		}

		#region Event Listeners
		void onControllerCollider(RaycastHit2D hit)
		{
			// bail out on plain old ground hits cause they aren't very interesting
			if (hit.normal.y == 1f)
			{
				return;
			}

			if (_OnLadder && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
			{
				_Animator.SetTrigger("ClimbedOffLadder");
				_OnLadder = false;
			}

			// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
			//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
		}
		
		// Update when the trigger enters the event.
		void onTriggerEnterEvent(Collider2D col)
		{
			// If the trigger collided with Coin1 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins1"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Coin5 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins5"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Coin10 layer then remove the coin and add score.
			if (col.gameObject.layer == LayerMask.NameToLayer("Coins10"))
			{
				col.gameObject.GetComponent<CoinIdentifier>().RemoveCoin();
			}
			// If the trigger collided with Gems layer then remove the gem and add to the gem amount.
			if (col.gameObject.layer == LayerMask.NameToLayer("Gems"))
			{
				col.gameObject.GetComponent<GemIdentifier>().RemoveGem();
			}
			// If the trigger collided with Keys layer then remove the key and add to the key amount.
			if (col.gameObject.layer == LayerMask.NameToLayer("Keys"))
			{
				col.gameObject.GetComponent<KeyIdentifier>().RemoveKey();
			}
			// If the trigger collided with Locks layer then check if we have a key and unlock it if we do.
			if (col.gameObject.layer == LayerMask.NameToLayer("Locks"))
			{
				col.gameObject.GetComponent<LockIdentifier>().CheckLock();
			}

			if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			{
				Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
			}
		}

		// Update when the trigger exits the event.
		void onTriggerExitEvent(Collider2D col)
		{
			if (GameObject.Find("GameManager").GetComponent<GameManager>().isDebugActive)
			{
				Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
			}
		}
		#endregion

		// Update is called once per frame
		void Update()
		{
			#region Get or set input values
			// Get if the CharacterController2D is grounded and set the velocity to 0.
			if (_Controller.isGrounded)
			{
				_Velocity.y = 0;
				_Animator.SetBool("Grounded", true);
			}
			else
			{
				_Animator.SetBool("Grounded", false);
			}

			// Get if the CharacterController2D velocity is greater than 0 so that we can play the jumping animation.
			if (_Controller.velocity.y > 0)
			{
				_Animator.SetBool("Jumping", true);
			}
			else
			{
				_Animator.SetBool("Jumping", false);
			}

			if (_Controller.isGrounded && !_IsMeowing && (Input.GetButton("Meow") || CnInputManager.GetButton("Meow")))
			{
				_IsMeowing = true;
			}

			if (InputControlsManager.TestInput(_PCPlatforms))
			{
				_IsRunning = Input.GetButton("Run");
			}

			if (InputControlsManager.TestInput(_MobilePlatforms) && CnInputManager.GetButtonUp("Run"))
			{
				_IsRunning = !_IsRunning;
			}

			_Animator.SetBool("Running", _IsRunning);

			if (InputControlsManager.TestInput(_PCPlatforms))
			{
				NormalizedHorizontalSpeed = Input.GetAxis("Horizontal");
			}
			else if (InputControlsManager.TestInput(_MobilePlatforms))
			{
				NormalizedHorizontalSpeed = CnInputManager.GetAxis("Horizontal");
			}

			_Animator.SetFloat("Speed", Mathf.Abs(NormalizedHorizontalSpeed));

			if (_IsRunning)
			{
				NormalizedHorizontalSpeed *= RunMultiplierSpeed;
            }
			
			if (_Controller.isGrounded && (Input.GetButton("Jump") || CnInputManager.GetButton("Jump")))
			{
				_Velocity.y = Mathf.Sqrt(2.0f * JumpHeight * -Gravity);
			}

			if (_Controller.isGrounded && (Input.GetAxis("Vertical") < 0 || CnInputManager.GetAxis("Vertical") < 0))
			{
				_Velocity.y *= 3f;
				_Controller.ignoreOneWayPlatformsThisFrame = true;
			}

			if (NormalizedHorizontalSpeed == 0)
			{
				_Animator.SetFloat("Speed", 0);
			}

			// Flip the character based off of his speed.
			if (NormalizedHorizontalSpeed > 0)
			{
				if (transform.localScale.x < 0f)
				{
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
			}
			else if (NormalizedHorizontalSpeed < 0)
			{
				if (transform.localScale.x > 0f)
				{
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				}
			}
			#endregion

			#region Movement Controls
			// Apply horizontal speed smoothing it. don't really do this with Lerp. Use SmoothDamp or something that provides more control.
			var smoothedMovementFactor = _Controller.isGrounded ? GroundDamping : InAirDamping; // How fast do we change direction?
			_Velocity.x = Mathf.Lerp(_Velocity.x, NormalizedHorizontalSpeed * WalkSpeed, Time.deltaTime * smoothedMovementFactor);

			// Apply gravity before moving.
			_Velocity.y += Gravity * Time.deltaTime;

			// Move the controller at the velocity and fluctuate it with time.
			_Controller.move(_Velocity * Time.deltaTime);

			// grab our current _velocity to use as a base for all calculations
			_Velocity = _Controller.velocity;
			#endregion

			if (_IsMeowing)
			{
				_Animator.SetBool("Meowing", true);

				gameObject.GetComponent<AudioSource>().Play();

				_IsMeowing = false;
			}

			if (GameObject.FindObjectOfType<GameManager>().isDebugActive)
			{
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityX", _Velocity.x);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("VelocityY", _Velocity.y);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("OnLadder", _OnLadder);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsRunning", _IsRunning);
				GameObject.FindObjectOfType<DebugLabelManager>().UpdateToDatabase("IsGrounded", _Controller.isGrounded);
			}
		}
	}

	public static class InputControlsManager
	{
		public static bool TestInput(RuntimePlatform currentPlatform)
		{
			if (Application.platform == currentPlatform)
			{
				return true;
			}
			
			return false;
		}
		
		public static bool TestInput(RuntimePlatform[] currentPlatform)
		{
			if (Array.Exists(currentPlatform, element => element == Application.platform))
			{
				return true;
			}
			
			return false;
		}
	}
}