using System;
using UnityEngine;

namespace VoidInc
{
	public static class InputCheck
	{
		/// <summary>
		/// The list of PC platforms (including Mac, Web, and Linux, and Editors)
		/// </summary>
		public static RuntimePlatform[] PCPlatforms =
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
			RuntimePlatform.TizenPlayer
		};
		/// <summary>
		/// The list of mobile platforms.
		/// </summary>
		public static RuntimePlatform[] MobilePlatforms =
		{
			RuntimePlatform.IPhonePlayer,
			RuntimePlatform.Android,
			RuntimePlatform.WP8Player,
            RuntimePlatform.WSAPlayerX86,
			RuntimePlatform.WSAPlayerX64,
			RuntimePlatform.WSAPlayerARM
		};
		/// <summary>
		/// The list of Editors.
		/// </summary>
		public static RuntimePlatform[] EditorPlatforms =
		{
			RuntimePlatform.OSXEditor,
			RuntimePlatform.WindowsEditor
		};
		/// <summary>
		/// The list of console platforms.
		/// </summary>
		public static RuntimePlatform[] ConsolePlatforms =
		{
			RuntimePlatform.PS3,
			RuntimePlatform.PS4,
			RuntimePlatform.PSM,
			RuntimePlatform.PSP2,
			RuntimePlatform.WiiU,
			RuntimePlatform.XBOX360,
			RuntimePlatform.XboxOne
		};
		/// <summary>
		/// Tests if the game is a PC platform.
		/// </summary>
		public static bool IsPCPlatforms
		{
			get
			{
				if (Array.Exists(PCPlatforms, element => element == Application.platform))
				{
					return true;
				}

				return false;
			}
		}
		/// <summary>
		/// Tests if the game is a mobile platform.
		/// </summary>
		public static bool IsMobilePlatforms
		{
			get
			{
				if (Array.Exists(MobilePlatforms, element => element == Application.platform))
				{
					return true;
				}

				return false;
			}
		}
		/// <summary>
		/// Tests if the game is an editor platform.
		/// </summary>
		public static bool IsEditorPlatforms
		{
			get
			{
				if (Array.Exists(EditorPlatforms, element => element == Application.platform))
				{
					return true;
				}

				return false;
			}
		}
		/// <summary>
		/// Tests if the game is a console platform.
		/// </summary>
		public static bool IsConsolePlatforms
		{
			get
			{
				if (Array.Exists(ConsolePlatforms, element => element == Application.platform))
				{
					return true;
				}

				return false;
			}
		}
		/// <summary>
		/// Tests input of one platform.
		/// </summary>
		/// <param name="currentPlatform">The platform to test.</param>
		/// <returns>If the currentPlatform matches the game's platform.</returns>
		public static bool TestInput(RuntimePlatform currentPlatform)
		{
			if (Application.platform == currentPlatform)
			{
				return true;
			}

			return false;
		}
		/// <summary>
		/// Tests input of multiple platforms.
		/// </summary>
		/// <param name="currentPlatform">The current platforms to test.</param>
		/// <returns>If one of the currentPlatforms matches the game's platform.</returns>
		[Obsolete("TestInput is deprecated, please use IsPCPlatform or the other variable checkers instead.")]
		public static bool TestInput(RuntimePlatform[] currentPlatform)
		{
			if (Array.Exists(currentPlatform, element => element == Application.platform))
			{
				return true;
			}

			return false;
		}
		/// <summary>
		/// Returns what the inputed axis is if 0 is true and 1 is false.
		/// </summary>
		/// <param name="axis">The axis to check.</param>
		/// <returns>If 0 is true and 1 is false.</returns>
		public static bool GetAxisMinMax(float axis)
		{
			if (axis == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
