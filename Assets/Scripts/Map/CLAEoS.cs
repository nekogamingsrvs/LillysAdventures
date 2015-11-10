//CLAEoS - Change Level At End of Screen - Also a pretty noice name :3 - But the inspector really f's up the spacing >:|
using UnityEngine;
using System.Collections;

[System.Serializable]
public class BounderyRect
{
    public Vector2 topLeft = new Vector2(-8, 8), topRight = new Vector2(8, 8), bottomLeft = new Vector2(-8, -8), bottomRight = new Vector2(8, -8);
}

[ExecuteInEditMode]
public class CLAEoS : MonoBehaviour {
    public BounderyRect bounds;

    public GameObject Player;

    public string levelName;

    [HideInInspector]
    public float yPositionWhileEntering;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var boundsActual = new BounderyRect();

        boundsActual.topLeft = new Vector2(bounds.topLeft.x * gameObject.transform.localScale.x, bounds.topLeft.y * gameObject.transform.localScale.y);

        boundsActual.topRight = new Vector2(bounds.topRight.x * gameObject.transform.localScale.x, bounds.topRight.y * gameObject.transform.localScale.y);

        boundsActual.bottomLeft = new Vector2(bounds.bottomLeft.x * gameObject.transform.localScale.x, bounds.bottomLeft.y * gameObject.transform.localScale.y);

        boundsActual.bottomRight = new Vector2(bounds.bottomRight.x * gameObject.transform.localScale.x, bounds.bottomRight.y * gameObject.transform.localScale.y);

        //Lines
        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0), new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.topRight.x, transform.position.y + boundsActual.topRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomRight.x, transform.position.y + boundsActual.bottomRight.y, 0), new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0));

        Gizmos.DrawLine(new Vector3(transform.position.x + boundsActual.bottomLeft.x, transform.position.y + boundsActual.bottomLeft.y, 0), new Vector3(transform.position.x + boundsActual.topLeft.x, transform.position.y + boundsActual.topLeft.y, 0));
    }

    public void Update()
    {
        
    }
}