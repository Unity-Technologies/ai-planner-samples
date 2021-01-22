using Generated.Semantic.Traits.Enums;
using UnityEngine;

public class GemObject : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    CellType m_Type;
#pragma warning restore 0649

    MeshRenderer m_Renderer;
    Color m_DefaultColor;

    Vector3? m_Destination;
    float m_Speed;
    int m_X;
    int m_Y;
    bool m_Destroyed;

    public int X => m_X;
    public int Y => m_Y;

    public CellType Type => m_Type;

    public bool Destroyed => m_Destroyed;

    public void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_DefaultColor = m_Renderer.material.color;
    }

    public void Initialize(int x, int y)
    {
        m_X = x;
        m_Y = y;
    }

    void Update()
    {
        m_Renderer.material.color = Color.Lerp(m_Renderer.material.color, m_DefaultColor, 0.2f);

        if (m_Destination.HasValue)
        {
            transform.position = Vector3.Lerp(transform.position, m_Destination.Value, m_Speed);

            if (Vector3.Distance(transform.position, m_Destination.Value) < 0.1f)
            {
                var groundDestination = m_Destination.Value;
                groundDestination.y = 0;

                transform.position = groundDestination;
                m_Destination = null;
            }
        }
        else if (Destroyed && m_DefaultColor.a != 0)
        {
            m_Renderer.material.color = m_DefaultColor * 3;
            m_DefaultColor = new Color(0, 0, 0, 0);
        }
    }

    public void SetDestination(int newX, int newY, Vector3 destination, float speed = 0.2f)
    {
        m_X = newX;
        m_Y = newY;

        m_Destination = destination;
        m_Speed = speed;
    }

    public void Explode()
    {
        m_Destroyed = true;
        Destroy(gameObject, 0.25f);
    }
}
