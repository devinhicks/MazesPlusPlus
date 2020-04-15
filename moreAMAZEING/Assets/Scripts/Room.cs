using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int width;
    public int height;
}

public class GridLevelWithRooms : GridLevel
{
    Stack<Room> unplacedRooms;
    float CHANCE_OF_ROOM = 1.1f;
    int num = 0;

    public GridLevelWithRooms(int width, int height) : base(width, height)
    {
        unplacedRooms = new Stack<Room>();
        int numRooms = 20;
        for (int i = 0; i < numRooms; i++)
        {
            Room room = new Room();
            room.width = (int)Random.Range(3f, 14.99f);
            room.height = (int)Random.Range(3f, 14.99f);
            unplacedRooms.Push(room);
        }
    }

    public bool canPlaceRoom(Room room, int x, int y)
    {
        bool inBounds = (0 <= x) && (x < (m_width - room.width)) &&
            (0 <= y) && (y <= (m_height - room.height));

        if (!inBounds)
        {
            return false;
        }

        for (int rx = x; rx < x + room.width; rx++)
        {
            for (int ry = y; ry < y + room.height; ry++)
            {
                if (cells[rx,ry].inMaze)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void addRoom(Room room, Location location)
    {
        for (int x = location.x; x < location.x + room.width; x++)
        {
            for (int y = location.y; y < location.y + room.height; y++)
            {
                cells[x, y].inMaze = true;
                // set connections
                if (x != location.x + room.width - 1)
                {
                    cells[x, y].directions[0] = true;
                    cells[x + 1, y].directions[3] = true;
                }

                if (y != location.y + room.height - 1)
                {
                    cells[x, y].directions[1] = true;
                    cells[x, y + 1].directions[2] = true;
                }
            }
        }
    }

    public override Location makeConnection(Location location)
    {
        num++;

        // try to fit a room
        if (unplacedRooms.Count > 0 && num > 5 && (Random.Range(0f, 1.0f) <
            CHANCE_OF_ROOM))
        {
            int x = location.x;
            int y = location.y;

            Room room = unplacedRooms.Pop();
            Vector3 v = NEIGHBORS[(int)Random.Range(0f, 3.99f)];

            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            int nx = x + dx;
            int ny = y + dy;

            if (dx < 0)
            {
                nx -= (room.width - 1);
            }

            if (dy < 0)
            {
                ny -= room.height;
            }

            if (canPlaceRoom(room, nx, ny))
            {
                addRoom(room, location);

                cells[x, y].directions[dirn] = true;
                cells[x + dx, y + dy].directions[3 - dirn] = true;

                return null;
            }
        }

        return base.makeConnection(location);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
