using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField]
    List<Vector3> positions = new List<Vector3>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = .25f;
        lineRenderer.endWidth = .25f;
    }

    public void DrawLines(List<AbstractNode> nodes)
    {
        positions = new List<Vector3>();
        foreach(AbstractNode node in nodes)
        {
            positions.Add(new Vector3(node.GetX, node.GetY, 1));
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
