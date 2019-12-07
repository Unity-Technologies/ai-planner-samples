

using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;

#if PLANNER_DOMAIN_GENERATED
using AI.Planner.Domains;

namespace AI.Planner.Custom.Escape
{
	public class OneTimeModifier : ICustomTraitReward<Waypoint>
	{
		public float RewardModifier(Waypoint trait)
		{
			return (long)trait.GetField(Waypoint.FieldVisited) > 0 ? -0.1f : 1;
		}
	}
}
#endif