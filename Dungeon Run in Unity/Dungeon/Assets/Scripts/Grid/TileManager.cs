using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour


{
    public int generator;
    public bool randomgenerator;

    [Range(1, 6)]
    public int floorSize;
    public int totalFloorCount;

    public List<Vector3Int> floorList = new List<Vector3Int>();

    public Tile floorTile, wallTile;


    DungeonScape dungeonScape;

    public static TileManager instance;
    public Tilemap tilemap;
    public Dictionary<Vector3, GameTile> tiles;



    void Awake()
    {
        instance = this;
        if (randomgenerator)
        {
            //fix dungeon set as if it were a level
            generator = Random.Range(0, 99999);
        }
        Random.InitState(generator);
        dungeonScape = GetComponent<DungeonScape>();
        dungeonScape.RandomWalker();
    }

    public void AddTile(Vector3Int tilePos, Tile tileType)
    {
        tilemap.SetTile(tilePos, tileType);

        var tile = new GameTile
        {
            LocalPosition = tilePos,
            WorldPosition = tilemap.CellToWorld(tilePos),

            TileBase = tilemap.GetTile(tilePos),
            TilemapMember = tilemap,

            TileName = tilePos.x + "," + tilePos.y
        };

        GameTile foundTile;

        if (tiles.TryGetValue(tilePos, out foundTile))
        {
            tiles.Remove(tilePos);
        }
        tiles.Add(tilePos, tile);
    }

    public Vector3Int RandomDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1: return new Vector3Int(0, 1 + (floorSize * 2), 0);
            case 2: return new Vector3Int(1, 1 + (floorSize * 2),  0);
            case 3: return new Vector3Int(0, -1 - (floorSize * 2), 0);
            case 4: return new Vector3Int(-1 - (floorSize * 2), 0, 0);
        }
        return Vector3Int.zero;
    }

    public bool InFloorList(Vector3Int myPos)
    {
        for (int i = 0; i < floorList.Count; i++)
        {
            if (Vector3Int.Equals(myPos, floorList[i]))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator CreateDungeon()
    {
        tiles = new Dictionary<Vector3, GameTile>();

        int i = 0;
        while (i < floorList.Count)
        {
            var localPos = new Vector3Int(floorList[i].x, floorList[i].y, floorList[i].z);
            AddTile(localPos, floorTile);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int wallPos = new Vector3Int(localPos.x + x, localPos.y + y, localPos.z);
                    if (tilemap.HasTile(wallPos)) continue;
                    AddTile(wallPos, wallTile);
                }
            }
            i++;
            yield return new WaitForSeconds(0.001f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
