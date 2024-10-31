using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject baseGridObject;
    public Vector2 gridSize;  // 각 노드의 크기

    private int rows = 0;
    private int cols = 0;
    private float gridWidth;
    private float gridHeight;

    public bool didInit = false;

    private List<GridNode> nodes = new List<GridNode>();

    private void Start()
    {
        InitBaseGrid();
    }

    private void InitBaseGrid()
    {
        CalculateGridSize(baseGridObject);
        InitializeGrid();
    }

    private void CalculateGridSize(GameObject baseGrid)
    {
        if (baseGrid != null)
        {
            Vector3 baseSize = baseGrid.GetComponent<Renderer>().bounds.size;
            gridWidth = baseSize.x;
            gridHeight = baseSize.z;

            rows = Mathf.FloorToInt(gridWidth / gridSize.x);
            cols = Mathf.FloorToInt(gridHeight / gridSize.y);
        }
        else
        {
            Debug.LogError("baseGridObject가 설정되지 않았습니다.");
        }
    }

    private void InitializeGrid()
    {
        Vector3 startPosition = transform.position - new Vector3(gridWidth / 2, 0, gridHeight / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 nodePosition = startPosition + new Vector3(j * gridSize.x + gridSize.x / 2, 0, i * gridSize.y + gridSize.y / 2);
                nodes.Add(new GridNode(nodePosition, gridSize));
            }
        }
    }

    public GridNode GetNodeAtPosition(Vector3 position)
    {
        foreach (GridNode node in nodes)
        {
            if (Vector3.Distance(position, node.position) < gridSize.x / 2)
            {
                return node;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (!didInit)
        {
            nodes.Clear();
            InitBaseGrid();
            didInit = true;
        }


        if (nodes == null || nodes.Count == 0) return;

        foreach (GridNode node in nodes)
        {
            Gizmos.color = node.isOccupied ? Color.red : Color.grey;
            Gizmos.DrawWireCube(node.position, new Vector3(gridSize.x, 0.1f, gridSize.y));
        }
    }
}
