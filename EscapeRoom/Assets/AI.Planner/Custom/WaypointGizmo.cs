using Generated.AI.Planner.StateRepresentation;
using UnityEngine;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

[TraitGizmo(typeof(Waypoint))]
public class WaypointGizmo
{
	public void DrawGizmos(GameObject gameObject, ITraitData traitData, bool isSelected)
	{
		Gizmos.color = isSelected ? Color.white : Color.grey;
		Gizmos.DrawWireSphere(gameObject.transform.position + Vector3.up * 0.15f, 0.2f);

		Gizmos.color = isSelected ? Color.green : Opacity(Color.green, 0.5f);
		DrawLink(gameObject, traitData, Waypoint.FieldUp);
		Gizmos.color = isSelected ? Color.red : Opacity( Color.red,0.5f);
		DrawLink(gameObject, traitData, Waypoint.FieldRight);
		Gizmos.color = isSelected ? Color.blue : Opacity(Color.blue, 0.5f);
		DrawLink(gameObject, traitData, Waypoint.FieldDown, 0.2f);
		Gizmos.color = isSelected ? Color.yellow : Opacity(Color.yellow, 0.5f);
		DrawLink(gameObject, traitData, Waypoint.FieldLeft, 0.2f);
	}

	static void DrawLink(GameObject gameObject, ITraitData traitData, string field, float offset = 0.1f)
	{
		var linkObject = traitData.GetValue(field);
		if (linkObject != null)
		{
			var obj = GameObject.FindObjectsOfType<TraitComponent>();
			foreach (var traitComponent in obj)
			{
				if (traitComponent.Name.Equals(linkObject))
				{
					var offsetVector = Vector3.up * offset;
					var startPos = gameObject.transform.position + offsetVector;
					var endPos = traitComponent.gameObject.transform.position + offsetVector;

					Gizmos.DrawLine(startPos, endPos);

					var direction = endPos - startPos;
					
					Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180 + 20.0f,0) * new Vector3(0,0,1);
					Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180 - 20.0f,0) * new Vector3(0,0,1);
					Gizmos.DrawRay(startPos + direction, right * 0.25f);
					Gizmos.DrawRay(startPos + direction, left * 0.25f);
				}
			}
		}
	}
    
    public static Color Opacity(Color color, float opacity)
    {
        return new Color(color.r, color.g, color.b, opacity);
    }
}

