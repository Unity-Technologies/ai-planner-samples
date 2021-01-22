using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Planner.Controller;
using UnityEngine;

[Serializable]
public class Scenario
{
    [SerializeField]
    string m_Title;

    [SerializeField]
    GameObject m_PlannerControllerPrefab;

    [SerializeField]
    Material m_HighlightGoalMaterial;

    [SerializeField]
    Material m_HighlightPerceptionMaterial;

    [SerializeField]
    Camera m_VirtualCamera;

    [SerializeField]
    List<SpawnObject> m_Spawns;

    public Camera virtualCamera
    {
        get => m_VirtualCamera;
        set => m_VirtualCamera = value;
    }

    public Material highlightGoalMaterial
    {
        get => m_HighlightGoalMaterial;
        set => m_HighlightGoalMaterial = value;
    }

    public Material highlightPerceptionMaterial
    {
        get => m_HighlightPerceptionMaterial;
        set => m_HighlightPerceptionMaterial = value;
    }

    public string title
    {
        get => m_Title;
        set => m_Title = value;
    }

    public List<SpawnObject> spawns
    {
        get { return m_Spawns; }
        set { m_Spawns = value; }
    }

    public GameObject plannerControllerPrefab
    {
        get => m_PlannerControllerPrefab;
        set => m_PlannerControllerPrefab = value;
    }
}

[Serializable]
public class SpawnObject
{
    [SerializeField]
    GameObject m_Prefab;

    [SerializeField]
    Transform m_Position;

    public Transform position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

    public GameObject prefab
    {
        get { return m_Prefab; }
        set { m_Prefab = value; }
    }
}

public class GameScenario : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_Title = default;

    [SerializeField]
    TMP_Text m_SubTitle = default;

    DecisionController m_DecisionController;

    [SerializeField]
    List<Scenario> m_Scenarios = default;

    int m_ScenarioIndex = -1;
    Coroutine m_CurrentScenario;
    List<GameObject> m_ScenarioObjects = new List<GameObject>();

    Vector3 m_DefaultCameraPosition;
    Quaternion m_DefaultCameraRotation;
    Dictionary<Material, Color> m_DefaultColor = new Dictionary<Material, Color>();

    public void Start()
    {
        SaveCameraTransform();

        foreach (var scenario in m_Scenarios)
        {
            if (!m_DefaultColor.ContainsKey(scenario.highlightPerceptionMaterial))
                m_DefaultColor.Add(scenario.highlightPerceptionMaterial, scenario.highlightPerceptionMaterial.color);

            if (!m_DefaultColor.ContainsKey(scenario.highlightGoalMaterial))
                m_DefaultColor.Add(scenario.highlightGoalMaterial, scenario.highlightGoalMaterial.color);
        }
    }

    void SaveCameraTransform()
    {
         var mainCamera = Camera.main;
         m_DefaultCameraPosition = mainCamera.transform.position;
         m_DefaultCameraRotation = mainCamera.transform.rotation;
    }

    public void Update()
    {
        if (m_CurrentScenario == null && (m_ScenarioIndex == -1 || Input.GetKeyDown(KeyCode.Space)))
        {
            if (m_ScenarioIndex < 0)
                m_ScenarioIndex = 0;
            
            foreach (var scenarioObject in m_ScenarioObjects)
            {
                Destroy(scenarioObject);
            }

            m_ScenarioObjects.Clear();

            ResetMaterialToDefault();

            m_CurrentScenario = StartCoroutine(PlayScenarioSequence(m_ScenarioIndex));
        }
    }

    IEnumerator PlayScenarioSequence(int scenarioIndex)
    {
        var mainCamera = Camera.main;

        yield return new WaitForSeconds(0.1f);

        while (Vector3.Distance(mainCamera.transform.position, m_DefaultCameraPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, m_DefaultCameraPosition, 10f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, m_DefaultCameraRotation, 10f * Time.deltaTime);
            yield return null;
        }

        var scenario = m_Scenarios[scenarioIndex];

        m_Title.text = "Scenario";
        m_SubTitle.text = scenario.title;

        foreach (var spawn in scenario.spawns)
        {
            var spawnedObject = Instantiate(spawn.prefab, spawn.position.position, spawn.position.rotation);
            m_ScenarioObjects.Add(spawnedObject);
        }

        yield return null;

        var decisionObject = Instantiate(scenario.plannerControllerPrefab);
        m_ScenarioObjects.Add(decisionObject);

        yield return null;

        m_DecisionController = decisionObject.transform.GetComponent<DecisionController>();
        m_DecisionController.Initialize();

        var virtualCamera = scenario.virtualCamera;

        yield return new WaitForSeconds(0.1f);

        while (Vector3.Distance(mainCamera.transform.position, virtualCamera.transform.position) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, virtualCamera.transform.position, 5f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, virtualCamera.transform.rotation, 5f * Time.deltaTime);
            yield return null;
        }

        var highlightMaterial = scenario.highlightGoalMaterial;

        int blinkCount = 0;
        while (blinkCount < 4)
        {
            highlightMaterial.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            highlightMaterial.color = new Color(224/255f, 0, 0);
            yield return new WaitForSeconds(0.1f);
            blinkCount++;
        }

        yield return new WaitForSeconds(0.3f);

        highlightMaterial = scenario.highlightPerceptionMaterial;

        blinkCount = 0;
        while (blinkCount < 4)
        {
            highlightMaterial.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            highlightMaterial.color = new Color(253/255f, 158/255f, 0);
            yield return new WaitForSeconds(0.1f);
            blinkCount++;
        }
        m_DecisionController.AutoUpdate = true;

        while (!m_DecisionController.IsIdle)
            yield return null;

        m_CurrentScenario = null;

        m_ScenarioIndex = (m_ScenarioIndex + 1) % m_Scenarios.Count;
    }

    public void OnDestroy()
    {
        ResetMaterialToDefault();
    }

    void ResetMaterialToDefault()
    {
        foreach (var scenario in m_Scenarios)
        {
            scenario.highlightPerceptionMaterial.color = m_DefaultColor[scenario.highlightPerceptionMaterial];
            scenario.highlightGoalMaterial.color = m_DefaultColor[scenario.highlightGoalMaterial];
        }
    }
}

