using System;
using System.Collections.Generic;
using System.Linq;
using AI.Planner.Domains;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine.AI.Planner.Controller;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Actions.Match3Plan
{
    public struct DefaultHeuristic : IHeuristic<StateData>
    {
        public BoundedValue Evaluate(StateData state)
        {
            return new BoundedValue(-5, 0, 5);
        }
    }

    public struct TerminationEvaluator : ITerminationEvaluator<StateData>
    {
        public bool IsTerminal(StateData state, out float terminalReward)
        {
            terminalReward = 0f;
            var terminal = false;
            
            var SearchEndInstance = new SearchEnd();
            if (SearchEndInstance.IsTerminal(state))
            {
                terminal = true;
                terminalReward += SearchEndInstance.TerminalReward(state);
            }
            var SearchPrunedInstance = new SearchPruned();
            if (SearchPrunedInstance.IsTerminal(state))
            {
                terminal = true;
                terminalReward += SearchPrunedInstance.TerminalReward(state);
            }
            return terminal;
        }
    }

    class Match3PlanExecutor : BasePlanExecutor<TraitBasedObject, StateEntityKey, StateData, StateDataContext, ActionScheduler, DefaultHeuristic, TerminationEvaluator, StateManager, ActionKey, DestroyStatesJobScheduler>
    {
        static Dictionary<Guid, string> s_ActionGuidToNameLookup = new Dictionary<Guid,string>()
        {
            { ActionScheduler.SwapRightGuid, nameof(SwapRight) },
            { ActionScheduler.SwapUpGuid, nameof(SwapUp) },
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
                case var actionGuid when actionGuid == ActionScheduler.SwapRightGuid:
                    actionName = nameof(SwapRight);
                    break;
                case var actionGuid when actionGuid == ActionScheduler.SwapUpGuid:
                    actionName = nameof(SwapUp);
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
                    case nameof(SwapRight):
                        parameterIndex = SwapRight.GetIndexForParameterName(traitBasedObjectName);
                        break;
                    case nameof(SwapUp):
                        parameterIndex = SwapUp.GetIndexForParameterName(traitBasedObjectName);
                        break;
                }

                var traitBasedObjectIndex = actionKey[parameterIndex];
                if (split.Length > 1)
                {
                    switch (split[1])
                    {
                        case nameof(Game):
                            var traitGame = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Game>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitGame.GetField(split[2]) : traitGame;
                            break;
                        case nameof(Coordinate):
                            var traitCoordinate = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Coordinate>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCoordinate.GetField(split[2]) : traitCoordinate;
                            break;
                        case nameof(Cell):
                            var traitCell = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Cell>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitCell.GetField(split[2]) : traitCell;
                            break;
                        case nameof(Blocker):
                            var traitBlocker = stateData.GetTraitOnObjectAtIndex<AI.Planner.Domains.Blocker>(traitBasedObjectIndex);
                            arguments[i] = split.Length == 3 ? traitBlocker.GetField(split[2]) : traitBlocker;
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
