using UnityEngine;

public class Render : MonoBehaviour
{
    [SerializeField] MazeGen mazeGen; // Ensure this script exists in your project
    [SerializeField] GameObject MazeCellPrefab;

    public float CellSize = 1f; // Size of cells

    private void Start()
    {

        // Ensure mazeGen is not null
        if (mazeGen == null)
        {
            Debug.LogError("mazeGen is not assigned!");
            return;
        }

        // Get the maze grid
        MazeCell[,] maze = mazeGen.GetMaze();

        // Loop through the maze grid
        for (int x = 0; x < mazeGen.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGen.mazeHeight; y++)
            {
                // Instantiate a new cell
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                // Get the MazeCellObject component
                MazeObject mazeCell = newCell.GetComponent<MazeObject>();

                // Check walls
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                bool right = (x == mazeGen.mazeWidth - 1);
                bool bottom = (y == 0);

                // Initialize the cell
                mazeCell.Init(top, bottom, left, right);
            }
        }
    }
}