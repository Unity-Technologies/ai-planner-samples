using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using System;
using Generated.Semantic.Traits.Enums;
using Unity.Collections;
using Unity.AI.Planner.Traits;

namespace AI.Planner.Custom.Match3Plan
{
	public static class Match3Utility
	{
		public const int GameIndex = 0;
		public const int Cell1Index = 1;
		public const int Cell2Index = 2;

		public static void SwapCellAndUpdateBoard(ActionKey action, StateData state, Cell cell1, Cell cell2)
		{
            // Retrieve Game trait data
            var gameId = state.GetTraitBasedObjectId(action[GameIndex]);
            var game = state.GetTraitBasedObject(gameId);
            var gameTrait = state.GetTraitOnObject<Game>(game);

			// Swap cell types
			(cell1.Type, cell2.Type) = (cell2.Type, cell1.Type);
            state.SetTraitOnObjectAtIndex(cell1, action[Cell1Index]);
            state.SetTraitOnObjectAtIndex(cell2, action[Cell2Index]);

            int newScore = gameTrait.Score;
            var cellsToDestroy = new NativeList<int>(1, Allocator.Temp);

            // Check match3 and destroy used Gem (set to Type = None)
            CheckMatchOnGem(state, cell1, action[Cell1Index], ref cellsToDestroy);
            CheckMatchOnGem(state, cell2, action[Cell2Index], ref cellsToDestroy);

            if (cellsToDestroy.Length > 0)
            {
                #region debug
                cellsToDestroy.Clear();
                CheckMatchOnGem(state, cell1, action[Cell1Index], ref cellsToDestroy);
                CheckMatchOnGem(state, cell2, action[Cell2Index], ref cellsToDestroy);
                #endregion

                // Unset all destroyed cells
                var cellQueue = new NativeList<int>(Allocator.Temp);
                var cellsQueued = new NativeHashMap<int, byte>(3, Allocator.Temp);
                var cellChanged = new NativeHashMap<int, byte>(3, Allocator.Temp);

                while (cellsToDestroy.Length > 0)
                {
                    for (int i = 0; i < cellsToDestroy.Length; i++)
                    {
                        var cellIndex = cellsToDestroy[i];
                        if (cellsQueued.ContainsKey(cellIndex))
                            continue;

                        var cellTrait = state.GetTraitOnObjectAtIndex<Cell>(cellIndex);

                        if (cellTrait.Type == gameTrait.GoalType)
						    newScore += CustomSwapReward.GoalReward;
                        else
                            newScore += CustomSwapReward.BasicReward;

                        cellTrait.Type = CellType.None;
                        state.SetTraitOnObjectAtIndex(cellTrait, cellIndex);

                        cellQueue.Add(cellIndex);
                        cellsQueued.TryAdd(cellIndex, default);
                    }
                    cellsToDestroy.Clear();

                    // Stitch Unset Gems with Top Gem
                    while (cellQueue.Length > 0)
                    {
                        var cellIndex = cellQueue[0];
                        cellQueue.RemoveAtSwapBack(0);
                        cellsQueued.Remove(cellIndex);
                        var cell = state.GetTraitOnObjectAtIndex<Cell>(cellIndex);

                        if (cell.Top.Id == ObjectId.None)
                            continue;

                        if (cell.Type == CellType.None)
                        {
                            var cellTopObject = state.GetTraitBasedObject(cell.Top);
                            var cellTop = state.GetTraitOnObject<Cell>(cellTopObject);

                            // Find first cell with a known type on top
                            while (cellTop.Type == CellType.None)
                            {
                                if (cellTop.Top.Id == ObjectId.None)
                                    break;

                                cellTopObject = state.GetTraitBasedObject(cellTop.Top);
                                cellTop = state.GetTraitOnObject<Cell>(cellTopObject);
                            }

                            if (cellTop.Type != CellType.None)
                            {
                                cell.Type = cellTop.Type;
                                state.SetTraitOnObjectAtIndex(cell, cellIndex);

                                var newCellTop = cellTop;
                                newCellTop.Type = CellType.None;
                                state.SetTraitOnObject(newCellTop, ref cellTopObject);

                                var index = state.GetTraitBasedObjectIndex(cellTopObject);
                                cellQueue.Add(index);
                                cellsQueued.TryAdd(index, default);

                                // Queue all vertical cells for checking
                                var cellTopIndex = state.GetTraitBasedObjectIndex(cell.Top);
                                while (cellTop.Type != CellType.None)
                                {
                                    cellChanged.TryAdd(cellTopIndex, default);

                                    if (cellTop.Top == TraitBasedObjectId.None)
                                        break;

                                    cellTopIndex = state.GetTraitBasedObjectIndex(cellTop.Top);
                                    cellTop = state.GetTraitOnObjectAtIndex<Cell>(cellTopIndex);
                                }
                            }
                        }
                    }

                    // Check cells affected by stitching for chained-explosion
                    var changedKeys = cellChanged.GetKeyArray(Allocator.Temp);
                    for (int i = 0; i < changedKeys.Length; i++)
                    {
                        var cellIndex = changedKeys[i];
                        var cell = state.GetTraitOnObjectAtIndex<Cell>(cellIndex);
                        CheckMatchOnGem(state, cell, cellIndex, ref cellsToDestroy);
                    }
                    changedKeys.Dispose();
                    cellChanged.Clear();
                }

                cellQueue.Dispose();
                cellsQueued.Dispose();
                cellChanged.Dispose();
            }

            // Score is stored in the Game Object and apply later in the reward function
            gameTrait.Score = newScore;
            state.SetTraitOnObject(gameTrait, ref game);

            cellsToDestroy.Dispose();
        }

        static void CheckMatchOnGem(StateData state, Cell cellTrait, int cellIndex, ref NativeList<int> cellsToDestroy)
        {
            if (cellTrait.Type == CellType.None)
                return;

            var consecutiveHorizontal = ConsecutiveHorizontal(cellTrait, state);
            var consecutiveVertical = ConsecutiveVertical(cellTrait, state);

            if (consecutiveHorizontal >= 2)
            {
                CollectHorizontal(cellTrait, state, cellIndex, ref cellsToDestroy);
            }

            if (consecutiveVertical >= 2)
            {
                CollectVertical(cellTrait, state, cellIndex, ref cellsToDestroy);
            }
        }

        static int ConsecutiveHorizontal(Cell sourceCell, StateData state)
        {
            int consecutive = 0;
            Cell cellTrait;

            for (var cellId = sourceCell.Right; cellId.Id != ObjectId.None; cellId = cellTrait.Right)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                consecutive++;
            }

            for (var cellId = sourceCell.Left; cellId.Id != ObjectId.None; cellId = cellTrait.Left)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                consecutive++;
            }

            return consecutive;
        }

        static void CollectHorizontal(Cell sourceCell, StateData state, int cellIndex, ref NativeList<int> cellsToDestroy)
        {
            Cell cellTrait;

            for (var cellId = sourceCell.Right; cellId.Id != ObjectId.None; cellId = cellTrait.Right)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                cellsToDestroy.Add(state.GetTraitBasedObjectIndex(cellId));
            }

            cellsToDestroy.Add(cellIndex);

            for (var cellId = sourceCell.Left; cellId.Id != ObjectId.None; cellId = cellTrait.Left)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                cellsToDestroy.Add(state.GetTraitBasedObjectIndex(cellId));
            }
        }

        static int ConsecutiveVertical(Cell sourceCell, StateData state)
        {
            int consecutive = 0;
            Cell cellTrait;

            for (var cellId = sourceCell.Bottom; cellId.Id != ObjectId.None; cellId = cellTrait.Bottom)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                consecutive++;
            }

            for (var cellId = sourceCell.Top; cellId.Id != ObjectId.None; cellId = cellTrait.Top)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                consecutive++;
            }

            return consecutive;
        }

        static void CollectVertical(Cell sourceCell, StateData state, int cellIndex, ref NativeList<int> cellsToDestroy)
        {
            Cell cellTrait;

            for (var cellId = sourceCell.Top; cellId.Id != ObjectId.None; cellId = cellTrait.Top)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                cellsToDestroy.Add(state.GetTraitBasedObjectIndex(cellId));
            }

            cellsToDestroy.Add(cellIndex);

            for (var cellId = sourceCell.Bottom; cellId.Id != ObjectId.None; cellId = cellTrait.Bottom)
            {
                var objIndex = state.GetTraitBasedObjectIndex(cellId);
                cellTrait = state.GetTraitOnObjectAtIndex<Cell>(objIndex);

                if (cellTrait.Type != sourceCell.Type)
                    break;

                cellsToDestroy.Add(state.GetTraitBasedObjectIndex(cellId));
            }
        }
	}
}
