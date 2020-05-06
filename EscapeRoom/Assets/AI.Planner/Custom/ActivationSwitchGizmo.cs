using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Enums;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

[TraitGizmo(typeof(ActivationSwitch))]
public class ActivationSwitchGizmo
{
	public void DrawGizmos(GameObject gameObject, ITraitData traitData, bool isSelected)
	{
		var type = (ActivationType)(long)traitData.GetValue(ActivationSwitch.FieldType);
		Gizmos.color = (type == ActivationType.Blue) ? Color.cyan : Color.magenta;

		Gizmos.DrawWireCube(gameObject.transform.position , UnityEngine.Vector3.one);
	}
}
