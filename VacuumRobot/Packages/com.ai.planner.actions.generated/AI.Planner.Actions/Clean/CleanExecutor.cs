using System;
using System.Collections.Generic;
using System.Linq;
using AI.Planner.Domains;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine.AI.Planner.Controller;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Actions.Clean
{
    public struct DefaultHeuristic : IHeuristic<StateData>
    {
        public BoundedValue Evaluate(StateData state)
        {
            return new BoundedValue(-100, 0, 100);
        }
    }

    public struct TerminationEvaluator : ITerminationEvaluator<StateData>
    {
        public bool IsTerminal(StateData state, out float terminalReward)
        {
            terminalReward = 0f;
            return false;
        }
    }

    class CleanExecutor : BasePlanExecutor<TraitBasedObject, StateEntityKey, StateData, StateDataContext, ActionScheduler, AI.Planner.Actions.Clean.CustomVacuumRobotHeuristic, TerminationEvaluator, StateManager, ActionKey, DestroyStatesJobScheduler>
    {
        static Dictionary<Guid, string> s_ActionGuidToNameLookup = new Dictionary<Guid,string>()
        {
            { ActionScheduler.CollectGuid, nameof(Collect) },
            { ActionScheduler.NavigateGuid, nameof(Navigate) },
        };

        public override string GetActionName(IActionKey actionKey)
        {
            s_ActionGuidToNameLookup.TryGetValue(((IActionKeyWithGuid)actionKey).ActionGuid, out var name);
            return name;
        }

        protected override void RegisterOnDestroyCallback()
        {
            m_StateManager.Destroying += () => PlannerScheduler.CurrentJobHandle.Complete();
        }

        public override void Act(DecisionController controller)
        {
            var actionKey = GetBestAction();
            var stateData = m_StateManager.GetStateData(CurrentStateKey, false);
            var actionName = string.Empty;

            switch (actionKey.ActionGuid)
            {
                case var actionGuid when actionGuid == ActionScheduler.CollectGuid:
                    actionName = nameof(Collect);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.NavigateGuid:
                    actionName = nameof(Navigate);
                    break;
            }

            var executeInfos = controller.GetExecutionInfo(actionName);
            if (executeInfos == null)
                return;

            var argumentMapping = executeInfos.GetArgumentValues();
            var arguments = new object[argumentMapping.Count()];
            var i = 0;
            foreach (var argument in argumentMapping)
            {
                var split = argument.Split('.');

                int parameterIndex = -1;
                var traitBasedObjectName = split[0];

                if (string.IsNullOrEmpty(traitBasedObjectName))
                    throw new ArgumentException($"An argument to the '{actionName}' callback on '{controller.name}' DecisionController is invalid");

                switch (actionName)
                {
                    case nameof(Collect):
                        parameterIndex = Collect.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(Navigate):
                        parameterIndex = Navigate.GetIndexForParameterName(traitBasedObjectName);
                        break;
                }

                var traitBasedObjectIndex = actionKey[parameterIndex];
                if (split.Length > 1)
                {
                    switch (split[1])
                    {
                        case nameof(Location):
                            var traitLocation = stateData.GetTraitOnObjectAtIndex<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitLocation.GetField(split[2]) : traitLocation;
                            break;
                        case nameof(Robot):
                            var traitRobot = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Robot>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitRobot.GetField(split[2]) : traitRobot;
                            break;
                        case nameof(Dirt):
                            var traitDirt = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Dirt>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitDirt.GetField(split[2]) : traitDirt;
                            break;
                        case nameof(Moveable):
                            var traitMoveable = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Moveable>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitMoveable.GetField(split[2]) : traitMoveable;
                            break;
                    }
                }
                else
                {
                    var planStateId = stateData.GetTraitBasedObjectId(traitBasedObjectIndex);
                    ITraitBasedObjectData dataSource;
                    if (m_PlanStateToGameStateIdLookup.TryGetValue(planStateId.Id, out var gameStateId))
                        dataSource = m_DomainData.GetDataSource(new TraitBasedObjectId { Id = gameStateId });
                    else
                        dataSource = m_DomainData.GetDataSource(planStateId);

                    Type expectedType = executeInfos.GetParameterType(i);
                    if (typeof(ITraitBasedObjectData).IsAssignableFrom(expectedType))
                    {
                        arguments[i] = dataSource;
                    }
                    else
                    {
                        arguments[i] = null;
                        var obj = dataSource.ParentObject;
                        if (obj != null && obj is UnityEngine.GameObject gameObject)
                        {
                            if (expectedType == typeof(UnityEngine.GameObject))
                                arguments[i] = gameObject;

                            if (typeof(UnityEngine.Component).IsAssignableFrom(expectedType))
                                arguments[i] = gameObject == null ? null : gameObject.GetComponent(expectedType);
                        }
                    }
                }

                i++;
            }

            CurrentActionKey = actionKey;
            controller.StartAction(executeInfos, arguments);
        }
    }
}
