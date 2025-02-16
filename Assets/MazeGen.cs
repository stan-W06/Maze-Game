using UnityEngine;
using System.Collections.Generic;

public class MazeGen : MonoBehaviour
{
    [Range(100, 500)] // Bigger the range, the longer it takes to generate maze based on PC specs
    public int mazeWidth = 5, mazeHeight = 5;
    public int Startx, Starty; // Position to where you start (bottom left corner)

    MazeCell[,] maze;
    Vector2Int currentCell;

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }
        CarvePath(Startx, Starty);
        return maze;
    }

    List<Directions> directions = new List<Directions> { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

    List<Directions> GetRandomDirections()
    {
        List<Directions> dir = new List<Directions>(directions);
        List<Directions> rndDir = new List<Directions>();

        while (dir.Count > 0)
        {
            int rnd = Random.Range(0, dir.Count); // Get random index in our list
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }
        return rndDir; // Gives me a random list of directions for maze generation
    }

    bool IsCellValid(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited)
        {
            return false;
        }
        else return true;   // Checks if cell has been seen or not in range of our map
    }

    Vector2Int CheckNeighbour()
    {
        List<Directions> rndDir = GetRandomDirections();
        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell;

            switch (rndDir[i])
            {
                case Directions.Up:
                    neighbour.y++;
                    break;
                case Directions.Down:
                    neighbour.y--;
                    break;
                case Directions.Left:
                    neighbour.x--;
                    break;
                case Directions.Right:
                    neighbour.x++;
                    break;
            }
            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }
        return currentCell;
    }

    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }
        else if (primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y) // Top wall 
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    void CarvePath(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds");
        }

        currentCell = new Vector2Int(x, y);
        List<Vector2Int> path = new List<Vector2Int>();

        bool deadEnd = false;

        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbour();

            if (nextCell == currentCell)
            {
                for (int i = path.Count - 1; i >= 0; i--) // Checks for valid neighbors
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbour();
                    if (nextCell != currentCell) break; // If we can't find neighbor, we break out loop
                }
                if (nextCell == currentCell)
                {
                    deadEnd = true;
                }
            }
            else
            {
                BreakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].visited = true; // Stop an infinite loop so we don't keep coming back on ourselves
                currentCell = nextCell;
                path.Add(currentCell); // Add this cell to your path
            }
        }
    }
}

public enum Directions
{
    Up, Down, Left, Right
}

public class MazeCell
{
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
        // My coordinates of cell in the maze grid

        visited = false; // Make sure the cell is not visited
        topWall = true;
        leftWall = true;
    }
}