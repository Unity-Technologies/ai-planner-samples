using System;
using System.Linq;
using Generated.Semantic.Traits;
using Generated.Semantic.Traits.Enums;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Random.InitState(m_RandomSeed);

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
            }
        }

        for (var x = 0; x < m_GridSize; x++)
        {
            for (var y = 0; y < m_GridSize; y++)
            {
                var cell = GameObject.Find($"{x}_{y}");
                var cellData = cell.GetComponent<Cell>();
                if (cellData != null)
                {
                    cellData.Type = (CellType)Random.Range(1, Enum.GetNames(typeof(CellType)).Length);
                    cellData.Left = GameObject.Find($"{x - 1}_{y}");
                    cellData.Right = GameObject.Find($"{x + 1}_{y}");
                    cellData.Top = GameObject.Find($"{x}_{y + 1}");
                    cellData.Bottom = GameObject.Find($"{x}_{y - 1}");
                }
                else
                {
                    Debug.LogError("Cell object doesn't have a Cell Trait");
                    continue;
                }

                var coordinateData = cell.GetComponent<Coordinate>();
                if (coordinateData != null)
                {
                    coordinateData.X = x;
                    coordinateData.Y = y;
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
