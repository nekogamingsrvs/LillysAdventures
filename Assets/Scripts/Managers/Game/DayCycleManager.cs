using UnityEngine;

namespace VoidInc.LWA
{
	/// <summary>
	/// The Day Cycle Manager for Lilly's Wonderful Adventures
	/// </summary>
	public class DayCycleManager : MonoBehaviour
	{
		/// <summary>
		/// Time of day in minutes.
		/// </summary>
		public float DayLength = 1024.0f;
		/// <summary>
		/// The current time of day.
		/// </summary>
		private float CurrentTime = 0.5f;
		/// <summary>
		/// The sun's initial intensity.
		/// </summary>
		private float _SunInitialIntensity = 1.0f;

		void Awake()
		{
			DontDestroyOnLoad(this);

			if (FindObjectsOfType(GetType()).Length > 1)
			{
				Destroy(gameObject);
			}
		}

		// Update is called once per frame
		void Update()
		{
			UpdateSun();

			CurrentTime += (Time.deltaTime / (DayLength)) * 5f;
			
			if (CurrentTime >= 1)
			{
				CurrentTime = 0;
			}
		}

		void UpdateSun()
		{
			float intensityMultiplier = 1.0f;
			float time = 1.0f;

			if (CurrentTime <= 0.10f || CurrentTime >= 0.80f)
			{
				intensityMultiplier = 0.25f;
            }
			else if (CurrentTime <= 0.30f)
			{
				intensityMultiplier = Mathf.Clamp((CurrentTime - 0.23f) * (1 / 0.02f), 0.25f, 1.0f);
				time = Mathf.Clamp((CurrentTime - 0.23f) * (1 / 0.02f), 0.0f, 1.0f);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor = Color.Lerp(Color.black, new Color(0.6705882352941176f, 0.9764705882352941f, 1.00f), time);
			}
			else if (CurrentTime >= 0.60f)
			{
				intensityMultiplier = Mathf.Clamp((1 - ((CurrentTime - 0.73f) * (1 / 0.02f))), 0.25f, 1.0f);
				time = Mathf.Clamp((1 - ((CurrentTime - 0.73f) * (1 / 0.02f))), 0.0f, 1.0f);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor = Color.Lerp(Color.black, new Color(0.6705882352941176f, 0.9764705882352941f, 1.00f), time);
			}

			gameObject.GetComponent<Light>().intensity = _SunInitialIntensity * intensityMultiplier;

		}
	}
}
