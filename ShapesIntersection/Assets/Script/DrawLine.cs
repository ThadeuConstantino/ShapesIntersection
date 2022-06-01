using UnityEngine;
using System.Collections;
using TMPro;

public class DrawLine : MonoBehaviour
{
    public int segments;
    public float xRadius;
    public float yRadius;
    LineRenderer lineRenderer;
    public TextMeshPro _text;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawShape(int id, float valueX, float valueY)
    {
        _text.text = id.ToString();
        xRadius = valueX;
        yRadius = valueY;

        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = lineRenderer.endWidth = 1;
        lineRenderer.useWorldSpace = false;

        CreateLines();
    }

    void CreateLines()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 45f;

        for (int i = 0; i < segments + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

            lineRenderer.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
}