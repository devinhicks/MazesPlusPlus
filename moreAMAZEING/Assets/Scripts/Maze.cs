using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public Location mazeStart = new Location(0, 0);

    GridLevelWithRooms levelOne;
    GameObject WallPrefab;

    public Canvas instructions;

    // Start is called before the first frame update
    void Start()
    {
        //levelOne = new GridLevelWithRooms(mazeWidth, mazeHeight);
        //generateMaze(levelOne, mazeStart);

        WallPrefab = Resources.Load<GameObject>("WallPrefab");
        //BuildMaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            instructions.gameObject.SetActive(false);

            // destroy old walls
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            Debug.Log("Found " + walls.Length + " walls.");
            foreach (GameObject wall in walls)
            {
                //Debug.Log("destroying");
                Destroy(wall);
            }

            // generate new maze
            mazeWidth = (int)Random.Range(20f, 30f);
            mazeHeight = (int)Random.Range(20f, 30f);
            mazeStart = new Location((int)Random.Range(0f, mazeWidth - 1), 0);
            levelOne = new GridLevelWithRooms(mazeWidth, mazeHeight);

            generateMaze(levelOne, mazeStart);
            BuildMaze();
        }

        // debug draw the maze
        if (levelOne != null)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    Connections currentCell = levelOne.cells[x, y];
                    if (levelOne.cells[x, y].inMaze)
                    {
                        Vector3 cellPos = new Vector3(x, 0, y);
                        float lineLength = 1f;
                        if (currentCell.directions[0])
                        {
                            // positive x
                            Vector3 neighborPos = new Vector3(x + lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[1])
                        {
                            // positive y
                            Vector3 neighborPos = new Vector3(x, 0, y + lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[2])
                        {
                            // negative y
                            Vector3 neighborPos = new Vector3(x, 0, y - lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[3])
                        {
                            // negative x
                            Vector3 neighborPos = new Vector3(x - lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                    }
                }
            }
        }
    }

    void BuildMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Connections currentCell = levelOne.cells[x, y];
                if (levelOne.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;

                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(WallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }

                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(WallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }

                    if (y == 0 && !currentCell.directions[2])
                    {
                        // negative y
                        Vector3 wallPos = new Vector3(x, 0, y - lineLength / 2);
                        GameObject wall = Instantiate(WallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }

                    if (x == 0 && !currentCell.directions[3])
                    {
                        // negative x
                        Vector3 wallPos = new Vector3(x - lineLength / 2, 0, y);
                        GameObject wall = Instantiate(WallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                }
            }
        }
    }

    void generateMaze(Level level, Location start)
    {
        Stack<Location> locations = new Stack<Location>();
        locations.Push(start);
        level.startAt(start);

        while (locations.Count > 0)
        {
            Location current = locations.Peek();

            Location next = level.makeConnection(current);
            if (next != null)
            {
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}

