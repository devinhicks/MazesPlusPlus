using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : Level
{
    public Vector3[] NEIGHBORS = new Vector3[4]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 1),
        new Vector3(0, -1, 2),
        new Vector3(-1, 0, 3)
    };

    public int m_width;
    public int m_height;
    public Connections[,] cells;

    public GridLevel(int width, int height)
    {
        m_width = width;
        m_height = height;
        cells = new Connections[width, height];
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                cells[i, j] = new Connections();
            }
        }
    }

    public override void startAt(Location location)
    {
        cells[location.x, location.y].inMaze = true;
    }

    bool canPlaceCorridor(int x, int y, int dirn)
    {
        return (x >= 0 && x < m_width) && (y >= 0 && y < m_height)
            && !cells[x, y].inMaze;
    }

    void shuffleNeighbors()
    {
        int n = NEIGHBORS.Length;
        while (n > 1)
        {
            n--;
            int k = (int)Random.Range(0, n);
            Vector3 v = NEIGHBORS[k];
            NEIGHBORS[k] = NEIGHBORS[n];
            NEIGHBORS[n] = v;
        }
    }

    public override Location makeConnection(Location location)
    {
        shuffleNeighbors();

        int x = location.x;
        int y = location.y;

        foreach (Vector3 v in NEIGHBORS)
        {
            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            int nx = x + dx;
            int ny = y + dy;
            int fromDirn = 3 - dirn;

            if (canPlaceCorridor(nx, ny, fromDirn))
            {
                cells[x, y].directions[dirn] = true;
                cells[nx, ny].inMaze = true;
                cells[nx, ny].directions[fromDirn] = true;
                return new Location(nx, ny);
            }
        }

        return null;
    }
}