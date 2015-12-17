using System;
using UnityEngine;

namespace VoidInc
{
	public static class InputCheck
	{
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
			RuntimePlatform.WSAPlayerX86,
			RuntimePlatform.WSAPlayerX64,
			RuntimePlatform.WSAPlayerARM,
			RuntimePlatform.TizenPlayer
		};

		public static RuntimePlatform[] MobilePlatforms =
		{
			RuntimePlatform.IPhonePlayer,
			RuntimePlatform.Android,
			RuntimePlatform.WP8Player
		};

		public static RuntimePlatform[] EditorPlatforms =
		{
			RuntimePlatform.OSXEditor,
			RuntimePlatform.WindowsEditor
		};

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
