using System;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Entities;
using Generated.AI.Planner.StateRepresentation.Clean;

namespace Generated.AI.Planner.Plans.Clean
{
    public class PlanningSystemsProvider : IPlanningSystemsProvider
    {
        public ITraitBasedStateConverter StateConverter => m_StateConverter;
        PlannerStateConverter<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager> m_StateConverter;

        public ITraitBasedPlanExecutor PlanExecutor => m_Executor;
        CleanExecutor m_Executor;

        public IPlannerScheduler PlannerScheduler => m_Scheduler;
        PlannerScheduler<StateEntityKey, ActionKey, StateManager, StateData, StateDataContext, ActionScheduler, global::AI.Planner.Actions.Clean.CustomVacuumRobotHeuristic, TerminationEvaluator, DestroyStatesJobScheduler> m_Scheduler;

        public void Initialize(ProblemDefinition problemDefinition, string planningSimulationWorldName)
        {
            var world = new World(planningSimulationWorldName);
            var stateManager = world.GetOrCreateSystem<StateManager>();
            world.GetOrCreateSystem<SimulationSystemGroup>().AddSystemToUpdateList(stateManager);
            var playerLoop = UnityEngine.LowLevel.PlayerLoop.GetCurrentPlayerLoop();
            ScriptBehaviourUpdateOrder.AddWorldToPlayerLoop(world, ref playerLoop);

            m_StateConverter = new PlannerStateConverter<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager>(problemDefinition, stateManager);

            m_Scheduler = new PlannerScheduler<StateEntityKey, ActionKey, StateManager, StateData, StateDataContext, ActionScheduler, global::AI.Planner.Actions.Clean.CustomVacuumRobotHeuristic, TerminationEvaluator, DestroyStatesJobScheduler>();
            m_Scheduler.Initialize(stateManager, new global::AI.Planner.Actions.Clean.CustomVacuumRobotHeuristic(), new TerminationEvaluator(), problemDefinition.DiscountFactor);

            m_Executor = new CleanExecutor(stateManager, m_StateConverter);

            // Ensure planning jobs are not running when destroying the state manager
            stateManager.Destroying += () => m_Scheduler.CurrentJobHandle.Complete();
        }
    }
}
