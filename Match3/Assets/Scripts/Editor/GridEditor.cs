#if PLANNER_STATEREPRESENTATION_GENERATED
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Enums;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;

[CustomEditor(typeof(Match3Grid), true)]
public class GridEditor : Editor
{
    int m_RandomSeed = 1281;
    int m_GridSize = 8;

    public override void OnInspectorGUI()
    {
        var grid = target as Match3Grid;

        m_RandomSeed = EditorGUILayout.IntField("Random seed", m_RandomSeed);

        if (GUILayout.Button("Generate new Grid"))
        {
            UnityEngine.Random.InitState(m_RandomSeed);

            InitializeGrid(grid);
        }

        DrawPropertiesExcluding(serializedObject, "m_Script");
    }

    void InitializeGrid(Match3Grid grid)
    {
        var currentChilds = grid.transform.Cast<Transform>().ToList();
        foreach (var child in currentChilds)
        {
            DestroyImmediate(child.gameObject);
        }

        for (var x = 0; x < m_GridSize; x++)
        {
            for (var y = 0; y < m_GridSize; y++)
            {
                var cell = Instantiate(grid.CellPrefab, new Vector3(x, 0, y), Quaternion.identity, grid.transform);
                cell.name = $"{x}_{y}";
                var traitHolder = cell.GetComponent<TraitComponent>();
                traitHolder.Name = $"Cell{x}_{y}";

                var cellData = traitHolder.GetTraitData<Cell>();
                if (cellData != null)
                {
                    cellData.InitializeFieldValues();
                    cellData.SetValue(Cell.FieldType, (CellType)UnityEngine.Random.Range(1, Enum.GetNames(typeof(CellType)).Length));
                    cellData.SetValue(Cell.FieldLeft, $"Cell{x - 1}_{y}");
                    cellData.SetValue(Cell.FieldRight, $"Cell{x + 1}_{y}");
                    cellData.SetValue(Cell.FieldTop, $"Cell{x}_{y + 1}");
                    cellData.SetValue(Cell.FieldBottom, $"Cell{x}_{y - 1}");
                }
                else
                {
                    Debug.LogError("Cell object doesn't have a Cell Trait");
                    continue;
                }

                var coordinateData = traitHolder.GetTraitData<Coordinate>();
                if (coordinateData != null)
                {
                    coordinateData.InitializeFieldValues();

                    coordinateData.SetValue(Coordinate.FieldX, (long)x);
                    coordinateData.SetValue(Coordinate.FieldY, (long)y);
                }
                else
                {
                    Debug.LogError("Cell object doesn't have a Coordinate Trait");
                    continue;
                }
            }
        }
    }
}
#endif
