using UnityEngine;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
	/// <summary>
	/// Time of day in minutes.
	/// </summary>
	public float _DayLength;

	/// <summary>
	/// The current time of day.
	/// </summary>
	public float _CurrentTime;

	/// <summary>
	/// The sun.
	/// </summary>
	public Light Sun;

	/// <summary>
	/// The sun's inital intensity.
	/// </summary>
	private float sunInitialIntensity = 1.0f;

	// Use this for initialization
	void Start ()
	{
		_DayLength = 120.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateSun();

		_CurrentTime += (Time.deltaTime / (_DayLength)) * 5f;

		if (_CurrentTime >= 1)
		{
			_CurrentTime = 0;
		}
	}

	void UpdateSun()
	{
		float intensityMultiplier = 1.0f;

		if (_CurrentTime <= 0.10f || _CurrentTime >= 0.80f)
		{
			intensityMultiplier = 0.25f;
		}
		else if (_CurrentTime <= 0.30f)
		{
			intensityMultiplier = Mathf.Clamp((_CurrentTime - 0.23f) * (1 / 0.02f), 0.25f, 1.0f);
		}
		else if (_CurrentTime >= 0.60f)
		{
			intensityMultiplier = Mathf.Clamp((1 - ((_CurrentTime - 0.73f) * (1 / 0.02f))), 0.25f, 1.0f);
		}

		Sun.intensity = sunInitialIntensity * intensityMultiplier;
	}
}
