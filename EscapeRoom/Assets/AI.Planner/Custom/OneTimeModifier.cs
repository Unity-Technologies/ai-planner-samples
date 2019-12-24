using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;

#if PLANNER_DOMAINS_GENERATED
using AI.Planner.Domains;

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
#endif