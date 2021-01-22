using Generated.Semantic.Traits;
using Generated.Semantic.Traits.Enums;
using Unity.Semantic.Traits;
using UnityEngine;

[TraitGizmo(typeof(ActivationSwitchData))]
public class ActivationSwitchGizmo
{
	public void DrawGizmos(GameObject gameObject, ITraitData traitData, bool isSelected)
	{
		var type = ((ActivationSwitchData)traitData).Type;
		Gizmos.color = (type == ActivationType.Blue) ? Color.cyan : Color.magenta;

		Gizmos.DrawWireCube(gameObject.transform.position , UnityEngine.Vector3.one);
	}
}
