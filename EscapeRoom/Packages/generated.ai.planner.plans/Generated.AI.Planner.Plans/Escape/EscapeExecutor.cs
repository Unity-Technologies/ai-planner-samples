using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Escape;

namespace Generated.AI.Planner.Plans.Escape
{
    public struct DefaultCumulativeRewardEstimator : ICumulativeRewardEstimator<StateData>
    {
        public BoundedValue Evaluate(StateData state)
        {
            return new BoundedValue(-50, 0, 100);
        }
    }

    public struct TerminationEvaluator : ITerminationEvaluator<StateData>
    {
        public bool IsTerminal(StateData state, out float terminalReward)
        {
            terminalReward = 0f;
            var terminal = false;
            
            var EscapeGoalInstance = new EscapeGoal();
            if (EscapeGoalInstance.IsTerminal(state))
            {
                terminal = true;
                terminalReward += EscapeGoalInstance.TerminalReward(state);
            }
            return terminal;
        }
    }

    class EscapeExecutor : BaseTraitBasedPlanExecutor<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>
    {
        static Dictionary<Guid, string> s_ActionGuidToNameLookup = new Dictionary<Guid,string>()
        {
            { ActionScheduler.MoveDownGuid, nameof(MoveDown) },
            { ActionScheduler.MoveLeftGuid, nameof(MoveLeft) },
            { ActionScheduler.MoveRightGuid, nameof(MoveRight) },
            { ActionScheduler.MoveUpGuid, nameof(MoveUp) },
            { ActionScheduler.PickupKeyGuid, nameof(PickupKey) },
            { ActionScheduler.UseDoorLeftGuid, nameof(UseDoorLeft) },
            { ActionScheduler.UseDoorRightGuid, nameof(UseDoorRight) },
            { ActionScheduler.UseGateUpGuid, nameof(UseGateUp) },
        };

        PlannerStateConverter<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager> m_StateConverter;

        public  EscapeExecutor(StateManager stateManager, PlannerStateConverter<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager> stateConverter)
        {
            m_StateManager = stateManager;
            m_StateConverter = stateConverter;
        }

        public override string GetActionName(IActionKey actionKey)
        {
            s_ActionGuidToNameLookup.TryGetValue(((IActionKeyWithGuid)actionKey).ActionGuid, out var name);
            return name;
        }

        protected override void Act(ActionKey actionKey)
        {
            var stateData = m_StateManager.GetStateData(CurrentPlanState, false);
            var actionName = string.Empty;

            switch (actionKey.ActionGuid)
            {
                case var actionGuid when actionGuid == ActionScheduler.MoveDownGuid:
                    actionName = nameof(MoveDown);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.MoveLeftGuid:
                    actionName = nameof(MoveLeft);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.MoveRightGuid:
                    actionName = nameof(MoveRight);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.MoveUpGuid:
                    actionName = nameof(MoveUp);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.PickupKeyGuid:
                    actionName = nameof(PickupKey);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.UseDoorLeftGuid:
                    actionName = nameof(UseDoorLeft);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.UseDoorRightGuid:
                    actionName = nameof(UseDoorRight);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.UseGateUpGuid:
                    actionName = nameof(UseGateUp);
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
                    case nameof(MoveDown):
                        parameterIndex = MoveDown.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(MoveLeft):
                        parameterIndex = MoveLeft.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(MoveRight):
                        parameterIndex = MoveRight.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(MoveUp):
                        parameterIndex = MoveUp.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(PickupKey):
                        parameterIndex = PickupKey.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(UseDoorLeft):
                        parameterIndex = UseDoorLeft.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(UseDoorRight):
                        parameterIndex = UseDoorRight.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(UseGateUp):
                        parameterIndex = UseGateUp.GetIndexForParameterName(traitBasedObjectName);
                        break;
                }

                if (parameterIndex == -1)
                    throw new ArgumentException($"Argument '{traitBasedObjectName}' to the '{actionName}' callback on '{m_Actor?.name}' DecisionController is invalid");

                var traitBasedObjectIndex = actionKey[parameterIndex];
                if (split.Length > 1) // argument is a trait
                {
                    switch (split[1])
                    {
                        case nameof(Character):
                            var traitCharacter = stateData.GetTraitOnObjectAtIndex<Character>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCharacter.GetField(split[2]) : traitCharacter;
                            break;
                        case nameof(Waypoint):
                            var traitWaypoint = stateData.GetTraitOnObjectAtIndex<Waypoint>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitWaypoint.GetField(split[2]) : traitWaypoint;
                            break;
                        case nameof(ActivationLock):
                            var traitActivationLock = stateData.GetTraitOnObjectAtIndex<ActivationLock>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitActivationLock.GetField(split[2]) : traitActivationLock;
                            break;
                        case nameof(KeyLock):
                            var traitKeyLock = stateData.GetTraitOnObjectAtIndex<KeyLock>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitKeyLock.GetField(split[2]) : traitKeyLock;
                            break;
                        case nameof(Carrier):
                            var traitCarrier = stateData.GetTraitOnObjectAtIndex<Carrier>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCarrier.GetField(split[2]) : traitCarrier;
                            break;
                        case nameof(Item):
                            var traitItem = stateData.GetTraitOnObjectAtIndex<Item>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitItem.GetField(split[2]) : traitItem;
                            break;
                        case nameof(Position):
                            var traitPosition = stateData.GetTraitOnObjectAtIndex<Position>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitPosition.GetField(split[2]) : traitPosition;
                            break;
                        case nameof(Carriable):
                            var traitCarriable = stateData.GetTraitOnObjectAtIndex<Carriable>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCarriable.GetField(split[2]) : traitCarriable;
                            break;
                        case nameof(ActivationSwitch):
                            var traitActivationSwitch = stateData.GetTraitOnObjectAtIndex<ActivationSwitch>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitActivationSwitch.GetField(split[2]) : traitActivationSwitch;
                            break;
                        case nameof(EscapePoint):
                            var traitEscapePoint = stateData.GetTraitOnObjectAtIndex<EscapePoint>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitEscapePoint.GetField(split[2]) : traitEscapePoint;
                            break;
                    }
                }
                else // argument is an object
                {
                    var planStateId = stateData.GetTraitBasedObjectId(traitBasedObjectIndex);
                    GameObject dataSource;
                    if (m_PlanStateToGameStateIdLookup.TryGetValue(planStateId.Id, out var gameStateId))
                        dataSource = m_StateConverter.GetDataSource(new TraitBasedObjectId { Id = gameStateId });
                    else
                        dataSource = m_StateConverter.GetDataSource(planStateId);

                    Type expectedType = executeInfos.GetParameterType(i);
                    // FIXME - if this is still needed
                    // if (typeof(ITraitBasedObjectData).IsAssignableFrom(expectedType))
                    // {
                    //     arguments[i] = dataSource;
                    // }
                    // else
                    {
                        arguments[i] = null;
                        var obj = dataSource;
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

        public override ActionParameterInfo[] GetActionParametersInfo(IStateKey stateKey, IActionKey actionKey)
        {
            string[] parameterNames = {};
            var stateData = m_StateManager.GetStateData((StateEntityKey)stateKey, false);

            switch (((IActionKeyWithGuid)actionKey).ActionGuid)
            {
                 case var actionGuid when actionGuid == ActionScheduler.MoveDownGuid:
                    parameterNames = MoveDown.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.MoveLeftGuid:
                    parameterNames = MoveLeft.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.MoveRightGuid:
                    parameterNames = MoveRight.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.MoveUpGuid:
                    parameterNames = MoveUp.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.PickupKeyGuid:
                    parameterNames = PickupKey.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.UseDoorLeftGuid:
                    parameterNames = UseDoorLeft.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.UseDoorRightGuid:
                    parameterNames = UseDoorRight.parameterNames;
                        break;
                 case var actionGuid when actionGuid == ActionScheduler.UseGateUpGuid:
                    parameterNames = UseGateUp.parameterNames;
                        break;
            }

            var parameterInfo = new ActionParameterInfo[parameterNames.Length];
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
