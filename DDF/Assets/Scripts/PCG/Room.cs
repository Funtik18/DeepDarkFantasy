using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    Vector2Int LU;//левый верхний угол
    Vector2Int RU;//правый верхний
    Vector2Int LD;//левый нижний
    Vector2Int RD;//правый нижний

    public static bool Intersect(Room a, Room b)
    {
        Debug.Log((a.LD.x < b.RU.x || a.LD.y < b.RU.y) && (a.LU.x > b.RD.x || a.LU.y < b.RD.y) && (a.RD.x < b.LU.x || a.RD.y > b.LU.y) && (a.RU.x < b.LD.x || a.RU.y < b.LD.y));
        if ((a.LD.x < b.RU.x || a.LD.y < b.RU.y) && (a.LU.x > b.RD.x || a.LU.y < b.RD.y) && (a.RD.x < b.LU.x || a.RD.y > b.LU.y) && (a.RU.x < b.LD.x || a.RU.y < b.LD.y))
        {

            return false;
        }
        return true;
    }

    public void PlaceRoom(Vector2Int location, Vector2Int size, GameObject floorPref, GameObject parent)
    {

        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (i == 0 && j == 0)
                {
                    LU.x = location.x + size.x * size.y + 10 * j;
                    LU.y = location.y + size.x * (size.y - 1) - 10 * i;
                }
                if (i == 0 && j == size.x - 1)
                {
                    RU.x = location.x + size.x * size.y + 10 * j;
                    RU.y = location.y + size.x * (size.y - 1) - 10 * i;
                }
                if (i == size.y - 1 && j == 0)
                {
                    LD.x = location.x + size.x * size.y + 10 * j;
                    LD.y = location.y + size.x * (size.y - 1) - 10 * i;
                }
                if (i == size.y - 1 && j == size.x - 1)
                {
                    RD.x = location.x + size.x * size.y + 10 * j;
                    RD.y = location.y + size.x * (size.y - 1) - 10 * i;
                }
                GameObject go = Instantiate(floorPref, new Vector3(location.x + size.x * size.y + 10 * j, 0, location.y + size.x * (size.y - 1) - 10 * i), Quaternion.identity, parent.transform);
            }
        }

    }
}
