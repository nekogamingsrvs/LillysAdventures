//CLAEoS - Change Level At End of Screen - Also a pretty noice name :3 - But the inspector really f's up the spacing >:|
using UnityEngine;
using System.Collections;

[System.Serializable]
public class BounderyRect
{
	/// <summary>
	/// The size of the bounding area.
	/// </summary>
    public Vector2 size = new Vector2(1, 1);

	/// <summary>
	/// The bounds of the bounding area?
	/// </summary>
    [HideInInspector]
    public Vector2 topLeft = new Vector2(-8, 8), topRight = new Vector2(8, 8), bottomLeft = new Vector2(-8, -8), bottomRight = new Vector2(8, -8);
}

[ExecuteInEditMode]
public class CLAEoS : MonoBehaviour
{
	/// <summary>
	/// The bounds of the area.
	/// </summary>
    public BounderyRect bounds;

	/// <summary>
	/// The player's GameObject.
	/// </summary>
    public GameObject Player;
	
	/// <summary>
	/// The level to go to.
	/// </summary>
    public string levelName;

	/// <summary>
	/// The any other GameObject with this script on it.
	/// </summary>
    public string otherCLAEoSToGoTo;

	/// <summary>
	/// Spawn's the player at X position.
	/// </summary>
    public float spawnPlayerAtX = 0;

	/// <summary>
	/// The Y position when entering the area.
	/// </summary>
    [HideInInspector]
    public float yPositionWhileEntering;

	// Start's the script.
    public void Start()
    {
		// Get the setting to tell where to teleport to.
        if (PlayerPrefs.GetString("PlayerPositionWER_TeleTo") == gameObject.name)
        {
			// Set the player's position.
            Player.transform.position = new Vector3(gameObject.transform.position.x + spawnPlayerAtX, PlayerPrefs.GetFloat("PlayerPositionWER_Y"), 0);
        }
    }

	// Draw the objects.
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var boundsActual = new BounderyRect();

        boundsActual.topLeft = new Vector2((bounds.topLeft.x * gameObject.transform.localScale.x) * bounds.size.x, (bounds.topLeft.y * gameObject.transform.localScale.y) * bounds.size.y);

        boundsActual.topRight = new Vector2((bounds.topRight.x * gameObject.transform.localScale.x) * bounds.size.x, (bounds.topRight.y * gameObject.transform.localScale.y) * bounds.size.y);

        boundsActual.bottomLeft = new Vector2((bounds.bottomLeft.x * gameObject.transform.localScale.x) * bounds.size.x, (bounds.bottomLeft.y * gameObject.transform.localScale.y) * bounds.size.y);

        boundsActual.bottomRight = new Vector2((bounds.bottomRight.x * gameObject.transform.localScale.x) * bounds.size.x, (bounds.bottomRight.y * gameObject.transform.localScale.y) * bounds.size.y);

        Gizmos.color = Color.red;
        //Spheres
        Gizmos.DrawSphere(new Vector3(spawnPlayerAtX + gameObject.transform.position.x, gameObject.transform.position.y, 0), 3);

        Gizmos.color = Color.green;
        //Lines
        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0), new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0), new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0));
    }

	// Runs when updating.
    public void Update()
    {
        var boundsActualWS = new BounderyRect();

        boundsActualWS.topLeft = new Vector2(((bounds.topLeft.x * gameObject.transform.localScale.x) * bounds.size.x) + gameObject.transform.position.x, ((bounds.topLeft.y * gameObject.transform.localScale.y) * bounds.size.y) + gameObject.transform.position.y);

        boundsActualWS.topRight = new Vector2(((bounds.topRight.x * gameObject.transform.localScale.x) * bounds.size.x) + gameObject.transform.position.x, ((bounds.topRight.y * gameObject.transform.localScale.y) * bounds.size.y) + gameObject.transform.position.y);

        boundsActualWS.bottomLeft = new Vector2(((bounds.bottomLeft.x * gameObject.transform.localScale.x) * bounds.size.x) + gameObject.transform.position.x, ((bounds.bottomLeft.y * gameObject.transform.localScale.y) * bounds.size.y) + gameObject.transform.position.y);

        boundsActualWS.bottomRight = new Vector2(((bounds.bottomRight.x * gameObject.transform.localScale.x) * bounds.size.x) + gameObject.transform.position.x, ((bounds.bottomRight.y * gameObject.transform.localScale.y) * bounds.size.y) + gameObject.transform.position.y);

		// Check if player is in bounds, and load level.
        if (Player.transform.position.x > boundsActualWS.topLeft.x && Player.transform.position.x < boundsActualWS.bottomRight.x && Player.transform.position.y > boundsActualWS.bottomLeft.y && Player.transform.position.y < boundsActualWS.topRight.y)
        {
            Application.LoadLevel(levelName);
            PlayerPrefs.SetFloat("PlayerPositionWER_Y", Player.transform.position.y);
            PlayerPrefs.SetString("PlayerPositionWER_TeleTo", otherCLAEoSToGoTo);
        }
    }
}