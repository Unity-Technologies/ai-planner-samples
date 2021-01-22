using System;
using Generated.Semantic.Traits;
using Unity.Semantic.Traits;
using UnityEngine;

[TraitGizmo(typeof(WaypointData))]
public class WaypointGizmo
{
	public void DrawGizmos(GameObject gameObject, ITraitData traitData, bool isSelected)
	{
		Gizmos.color = isSelected ? Color.white : Color.grey;
		Gizmos.DrawWireSphere(gameObject.transform.position + Vector3.up * 0.15f, 0.2f);

        var waypoint = gameObject.GetComponent<Waypoint>();
        if (waypoint == null)
            return;
        
		Gizmos.color = isSelected ? Color.green : Opacity(Color.green, 0.5f);
		DrawLink(gameObject, waypoint.Up);
		Gizmos.color = isSelected ? Color.red : Opacity( Color.red,0.5f);
		DrawLink(gameObject, waypoint.Right);
		Gizmos.color = isSelected ? Color.blue : Opacity(Color.blue, 0.5f);
		DrawLink(gameObject, waypoint.Down, 0.2f);
		Gizmos.color = isSelected ? Color.yellow : Opacity(Color.yellow, 0.5f);
		DrawLink(gameObject, waypoint.Left, 0.2f);
	}

	static void DrawLink(GameObject source, GameObject destination, float offset = 0.1f)
    {
        if (destination == null)
            return;
        
		var offsetVector = Vector3.up * offset;
		var startPos = source.transform.position + offsetVector;
		var endPos = destination.transform.position + offsetVector;

		Gizmos.DrawLine(startPos, endPos);

		var direction = endPos - startPos;

		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180 + 20.0f,0) * new Vector3(0,0,1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180 - 20.0f,0) * new Vector3(0,0,1);
		Gizmos.DrawRay(startPos + direction, right * 0.25f);
		Gizmos.DrawRay(startPos + direction, left * 0.25f);
	}

    public static Color Opacity(Color color, float opacity)
    {
        return new Color(color.r, color.g, color.b, opacity);
    }
}