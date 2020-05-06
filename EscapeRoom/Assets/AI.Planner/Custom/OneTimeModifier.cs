using Generated.AI.Planner.StateRepresentation;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;

namespace AI.Planner.Custom.Escape
{
	public struct OneTimeModifier : ICustomTraitReward<Waypoint>
	{
		public float RewardModifier(Waypoint trait)
		{
			return trait.Visited > 0 ? -0.1f : 1;
		}
	}
}