
#if PLANNER_DOMAIN_GENERATED
using AI.Planner.Domains.Enums;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

using AI.Planner.Domains; 

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
#endif
