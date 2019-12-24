using System;
using System.Collections.Generic;
using System.Linq;
using AI.Planner.Domains;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine.AI.Planner.Controller;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Actions.Escape
{
    public struct DefaultHeuristic : IHeuristic<StateData>
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

    class EscapeExecutor : BasePlanExecutor<TraitBasedObject, StateEntityKey, StateData, StateDataContext, ActionScheduler, AI.Planner.Custom.Escape.HeuristicExploration, TerminationEvaluator, StateManager, ActionKey, DestroyStatesJobScheduler>
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

                var traitBasedObjectIndex = actionKey[parameterIndex];
                if (split.Length > 1)
                {
                    switch (split[1])
                    {
                        case nameof(Character):
                            var traitCharacter = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Character>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCharacter.GetField(split[2]) : traitCharacter;
                            break;
                        case nameof(Waypoint):
                            var traitWaypoint = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Waypoint>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitWaypoint.GetField(split[2]) : traitWaypoint;
                            break;
                        case nameof(ActivationLock):
                            var traitActivationLock = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.ActivationLock>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitActivationLock.GetField(split[2]) : traitActivationLock;
                            break;
                        case nameof(KeyLock):
                            var traitKeyLock = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.KeyLock>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitKeyLock.GetField(split[2]) : traitKeyLock;
                            break;
                        case nameof(Carrier):
                            var traitCarrier = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Carrier>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCarrier.GetField(split[2]) : traitCarrier;
                            break;
                        case nameof(Carriable):
                            var traitCarriable = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Carriable>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCarriable.GetField(split[2]) : traitCarriable;
                            break;
                        case nameof(Position):
                            var traitPosition = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Position>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitPosition.GetField(split[2]) : traitPosition;
                            break;
                        case nameof(Item):
                            var traitItem = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Item>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitItem.GetField(split[2]) : traitItem;
                            break;
                        case nameof(ActivationSwitch):
                            var traitActivationSwitch = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.ActivationSwitch>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitActivationSwitch.GetField(split[2]) : traitActivationSwitch;
                            break;
                        case nameof(EscapePoint):
                            var traitEscapePoint = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.EscapePoint>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitEscapePoint.GetField(split[2]) : traitEscapePoint;
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
