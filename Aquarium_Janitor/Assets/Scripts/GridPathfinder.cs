using System.Collections.Generic;
using UnityEngine;

public class GridPathfinder : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(40, 40);
    public float cellSize = 1f;
    public LayerMask obstacleLayer;
    public float obstacleCheckRadius = 0.3f;

    private class Node
    {
        public Vector2 position;
        public bool walkable;
        public int gCost;
        public int hCost;
        public Node parent;

        public int FCost => gCost + hCost;

        public Node(Vector2 pos, bool canWalk)
        {
            position = pos;
            walkable = canWalk;
        }
    }

    private Node[,] grid;
    private int width;
    private int height;

    void Awake()
    {
        width = Mathf.RoundToInt(gridSize.x / cellSize);
        height = Mathf.RoundToInt(gridSize.y / cellSize);

        grid = new Node[width, height];

        Vector2 origin = (Vector2)transform.position - gridSize / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = origin + new Vector2(
                    (x + 0.5f) * cellSize,
                    (y + 0.5f) * cellSize
                );

                bool blocked = Physics2D.OverlapCircle(
                    pos, obstacleCheckRadius, obstacleLayer
                );

                grid[x, y] = new Node(pos, !blocked);
            }
        }
    }

    private Node GetNode(Vector2 position)
    {
        Vector2 origin = (Vector2)transform.position - gridSize / 2f;

        int x = Mathf.FloorToInt(
            (position.x - origin.x) / cellSize
        );

        int y = Mathf.FloorToInt(
            (position.y - origin.y) / cellSize
        );

        if (x < 0 || y < 0 || x >= width || y >= height)
            return null;

        return grid[x, y];
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int nx = Mathf.RoundToInt(
                    (node.position.x -
                    ((Vector2)transform.position - gridSize / 2f).x)
                    / cellSize - 0.5f
                ) + x;

                int ny = Mathf.RoundToInt(
                    (node.position.y -
                    ((Vector2)transform.position - gridSize / 2f).y)
                    / cellSize - 0.5f
                ) + y;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;

                Node neighbor = grid[nx, ny];

                if (!neighbor.walkable)
                    continue;

                // Prevent diagonal movement through blocked corners.
                if (x != 0 && y != 0)
                {
                    if (!grid[nx, ny - y].walkable ||
                        !grid[nx - x, ny].walkable)
                        continue;
                }

                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private int Distance(Node a, Node b)
    {
        int dx = Mathf.Abs(
            Mathf.RoundToInt(
                (a.position.x - b.position.x) / cellSize
            )
        );

        int dy = Mathf.Abs(
            Mathf.RoundToInt(
                (a.position.y - b.position.y) / cellSize
            )
        );

        int diagonal = Mathf.Min(dx, dy);
        int straight = Mathf.Max(dx, dy) - diagonal;

        return diagonal * 14 + straight * 10;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        Node startNode = GetNode(start);
        Node goalNode = GetNode(goal);

        if (startNode == null || goalNode == null ||
            !startNode.walkable || !goalNode.walkable)
            return new List<Vector2>();

        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();

        foreach (Node node in grid)
        {
            node.gCost = int.MaxValue;
            node.hCost = 0;
            node.parent = null;
        }

        startNode.gCost = 0;
        startNode.hCost = Distance(startNode, goalNode);
        open.Add(startNode);

        while (open.Count > 0)
        {
            Node current = open[0];

            foreach (Node node in open)
            {
                if (node.FCost < current.FCost ||
                    (node.FCost == current.FCost &&
                     node.hCost < current.hCost))
                    current = node;
            }

            if (current == goalNode)
            {
                List<Vector2> path = new List<Vector2>();

                while (current != null)
                {
                    path.Add(current.position);
                    current = current.parent;
                }

                path.Reverse();
                return path;
            }

            open.Remove(current);
            closed.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closed.Contains(neighbor))
                    continue;

                int newCost = current.gCost +
                    Distance(current, neighbor);

                if (newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = Distance(neighbor, goalNode);
                    neighbor.parent = current;

                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }

        return new List<Vector2>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}