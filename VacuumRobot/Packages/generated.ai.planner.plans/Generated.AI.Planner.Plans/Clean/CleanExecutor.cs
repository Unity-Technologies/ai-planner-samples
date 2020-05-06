using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.AI.Planner.Controller;
using UnityEngine;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Clean;

namespace Generated.AI.Planner.Plans.Clean
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

    class CleanExecutor : BaseTraitBasedPlanExecutor<TraitBasedObject, StateEntityKey, StateData, StateDataContext, ActionScheduler, global::AI.Planner.Actions.Clean.CustomVacuumRobotHeuristic, TerminationEvaluator, StateManager, ActionKey, DestroyStatesJobScheduler>
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

        public override void Initialize(MonoBehaviour actor, PlanDefinition planDefinition, IActionExecutionInfo[] actionExecutionInfos)
        {
            base.Initialize(actor, planDefinition, actionExecutionInfos);
            m_StateManager.Destroying += () => PlannerScheduler.CurrentJobHandle.Complete();
        }

        protected override void Act(ActionKey actionKey)
        {
            var stateData = m_StateManager.GetStateData(CurrentPlanState, false);
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

            var executeInfos = GetExecutionInfo(actionName);
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
                    throw new ArgumentException($"An argument to the '{actionName}' callback on '{m_Actor?.name}' DecisionController is invalid");

                switch (actionName)
                {
                    case nameof(Collect):
                        parameterIndex = Collect.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(Navigate):
                        parameterIndex = Navigate.GetIndexForParameterName(traitBasedObjectName);
                        break;
                }

                if (parameterIndex == -1)
                    throw new ArgumentException($"Argument '{traitBasedObjectName}' to the '{actionName}' callback on '{m_Actor?.name}' DecisionController is invalid");

                var traitBasedObjectIndex = actionKey[parameterIndex];
                if (split.Length > 1) // argument is a trait
                {
                    switch (split[1])
                    {
                        case nameof(Location):
                            var traitLocation = stateData.GetTraitOnObjectAtIndex<Location>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitLocation.GetField(split[2]) : traitLocation;
                            break;
                        case nameof(Robot):
                            var traitRobot = stateData.GetTraitOnObjectAtIndex<Robot>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitRobot.GetField(split[2]) : traitRobot;
                            break;
                        case nameof(Dirt):
                            var traitDirt = stateData.GetTraitOnObjectAtIndex<Dirt>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitDirt.GetField(split[2]) : traitDirt;
                            break;
                        case nameof(Moveable):
                            var traitMoveable = stateData.GetTraitOnObjectAtIndex<Moveable>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitMoveable.GetField(split[2]) : traitMoveable;
                            break;
                    }
                }
                else // argument is an object
                {
                    var planStateId = stateData.GetTraitBasedObjectId(traitBasedObjectIndex);
                    ITraitBasedObjectData dataSource;
                    if (m_PlanStateToGameStateIdLookup.TryGetValue(planStateId.Id, out var gameStateId))
                        dataSource = m_StateConverter.GetDataSource(new TraitBasedObjectId { Id = gameStateId });
                    else
                        dataSource = m_StateConverter.GetDataSource(planStateId);

                    Type expectedType = executeInfos.GetParameterType(i);
                    if (typeof(ITraitBasedObjectData).IsAssignableFrom(expectedType))
                    {
                        arguments[i] = dataSource;
                    }
                    else
                    {
                        arguments[i] = null;
                        var obj = dataSource.ParentObject;
                        if (obj != null && obj is GameObject gameObject)
                        {
                            if (expectedType == typeof(GameObject))
                                arguments[i] = gameObject;

                            if (typeof(Component).IsAssignableFrom(expectedType))
                                arguments[i] = gameObject == null ? null : gameObject.GetComponent(expectedType);
                        }
                    }
                }

                i++;
            }

            CurrentActionKey = actionKey;
            StartAction(executeInfos, arguments);
        }

        public override IActionParameterInfo[] GetActionParametersInfo(IStateKey stateKey, IActionKey actionKey)
        {
            string[] parameterNames = {};
            var stateData = m_StateManager.GetStateData((StateEntityKey)stateKey, false);

            switch (((IActionKeyWithGuid)actionKey).ActionGuid)
            {
                 case var actionGuid when actionGuid == ActionScheduler.CollectGuid:
                    parameterNames = Collect.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.NavigateGuid:
                    parameterNames = Navigate.parameterNames;
                        break;
            }

            var parameterInfo = new IActionParameterInfo[parameterNames.Length];
            for (var i = 0; i < parameterNames.Length; i++)
            {
                var traitBasedObjectId = stateData.GetTraitBasedObjectId(((ActionKey)actionKey)[i]);

#if DEBUG
                parameterInfo[i] = new ActionParameterInfo { ParameterName = parameterNames[i], TraitObjectName = traitBasedObjectId.Name.ToString(), TraitObjectId = traitBasedObjectId.Id };
#else
                parameterInfo[i] = new ActionParameterInfo { ParameterName = parameterNames[i], TraitObjectName = traitBasedObjectId.ToString(), TraitObjectId = traitBasedObjectId.Id };
#endif
            }

            return parameterInfo;
        }
    }
}
