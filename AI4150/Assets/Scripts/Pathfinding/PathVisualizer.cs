using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    public void DrawLines(List<PathFindingNode> nodes)
    {
        List<Vector3> positions = new List<Vector3>();

        foreach(PathFindingNode node in nodes)
        {
            positions.Add(new Vector3(node.GetX, node.GetY, 1));
        }

        lineRenderer.SetPositions(positions.ToArray());
    }
}
