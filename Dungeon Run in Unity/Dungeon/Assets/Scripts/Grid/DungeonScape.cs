using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScape : MonoBehaviour
{
    TileManager tileManager;

    void Awake()
    {
        tileManager = GetComponent<TileManager>();
    }

    public void RandomWalker()
    {
        Vector3Int currentPos = Vector3Int.zero;
        tileManager.floorList.Add(currentPos);
        for (int x = -tileManager.floorSize; x <= tileManager.floorSize; x++)
        {
            for (int y = -tileManager.floorSize; y <= tileManager.floorSize; y++)
            {
                Vector3Int offset = new Vector3Int(x, y, 0);
                if (!tileManager.InFloorList(currentPos + offset))
                {
                    tileManager.floorList.Add(currentPos + offset);
                }
            }
        }
        while (tileManager.floorList.Count < tileManager.totalFloorCount)
        {
            currentPos += tileManager.RandomDirection();
            for (int a = -tileManager.floorSize; a <= tileManager.floorSize; a++)
            {
                for (int b = -tileManager.floorSize; b <= tileManager.floorSize; b++)
                {
                    Vector3Int offset = new Vector3Int(a, b, 0);
                    if (!tileManager.InFloorList(currentPos + offset))
                    {
                        tileManager.floorList.Add(currentPos + offset);
                    }
                }
            }
        }
        StartCoroutine(tileManager.CreateDungeon());
    }
}
