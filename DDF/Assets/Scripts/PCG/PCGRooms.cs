using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PCGRooms : MonoBehaviour
{
    public int RoomsCount=12;
    public Rooms[] RoomsPrefabs;
    public Rooms FirstRoom;
    private Rooms[,] spawnedRooms;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        spawnedRooms = new Rooms[20, 11];

        spawnedRooms[5, 5] = FirstRoom;

        for (int i = 0; i < RoomsCount; i++)
        {
            PlaceOneRoom();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }


    void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantplaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {


            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null)
                    continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null)
                    vacantplaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null)
                    vacantplaces.Add(new Vector2Int(x, y - 1));

                if (x < maxX && spawnedRooms[x + 1, y] == null)
                    vacantplaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null)
                    vacantplaces.Add(new Vector2Int(x, y + 1));

            }
        }

        Rooms newRoom = Instantiate(RoomsPrefabs[Random.Range(0, RoomsPrefabs.Length)]);

        int limit = 500;
        while (limit-->0)
        {
            Vector2Int position = vacantplaces.ElementAt(Random.Range(0, vacantplaces.Count));


            newRoom.RotateRandom();
            if(RoomConnecter(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 50;
                spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
        }
        Destroy(newRoom.gameObject);
    }

    private bool RoomConnecter(Rooms room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.Door1 != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.Door3 != null)
        {
            neighbours.Add(Vector2Int.up);
        }

        if (room.Door3 != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.Door1 != null)
        {
            neighbours.Add(Vector2Int.down);
        }

        if (room.Door2 != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.Door4 != null)
        {
            neighbours.Add(Vector2Int.right);
        }

        if (room.Door4 != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.Door2 != null)
        {
            neighbours.Add(Vector2Int.left);
        }

        if (neighbours.Count == 0)
        {
            return false;
        }

        Vector2Int selectedDiretion = neighbours[Random.Range(0, neighbours.Count)];
        Rooms selectedRoom = spawnedRooms[p.x + selectedDiretion.x, p.y + selectedDiretion.y];

        if (selectedDiretion == Vector2Int.up)
        {
            room.Door1.SetActive(false);
            selectedRoom.Door3.SetActive(false);
        }
        if (selectedDiretion == Vector2Int.down)
        {
            room.Door3.SetActive(false);
            selectedRoom.Door1.SetActive(false);
        }
        if (selectedDiretion == Vector2Int.right)
        {
            room.Door2.SetActive(false);
            selectedRoom.Door4.SetActive(false);
        }
        if (selectedDiretion == Vector2Int.left)
        {
            room.Door4.SetActive(false);
            selectedRoom.Door2.SetActive(false);
        }
        return true;
    }
}
