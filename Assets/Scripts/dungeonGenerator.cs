using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject[] trees;
    [SerializeField] GameObject props;
    [SerializeField] Sprite[] propSprites;
    [SerializeField]
    private Tile groundTile;
    [SerializeField]
    private Tile pitTile;
    [SerializeField]
    private Tile topWallTile;
    [SerializeField]
    private Tile botWallTile;
    [SerializeField]
    private Tilemap groundMap;
    [SerializeField]
    private Tilemap pitMap;
    [SerializeField]
    private Tilemap wallMap;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int deviationRate = 10;
    [SerializeField]
    private int roomRate = 15;
    [SerializeField]
    private int maxRouteLength;
    [SerializeField]
    private int maxRoutes = 20;
    private int routeCount = 0;
    
    public GameObject[] powerups;
    int powerupSpawnCount;
    
    private void Start()
    {
        int x = 0;
        int y = 0;
        int routeLength = 0;
        powerupSpawnCount=0;
        GenerateSquare(x, y, 1);
        Vector2Int previousPos = new Vector2Int(x, y);
        Vector3Int pos = new Vector3Int(x,y,0);
        player.transform.position = groundMap.GetCellCenterLocal(pos);
        y += 3;
        GenerateSquare(x, y, 1);
        NewRoute(x, y, routeLength, previousPos);

        FillWalls();
    }

    private void FillWalls()
    {
        BoundsInt bounds = groundMap.cellBounds;
        for (int xMap = bounds.xMin - 10; xMap <= bounds.xMax + 10; xMap++)
        {
            for (int yMap = bounds.yMin - 10; yMap <= bounds.yMax + 10; yMap++)
            {
                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);
                TileBase tile = groundMap.GetTile(pos);
                TileBase tileBelow = groundMap.GetTile(posBelow);
                TileBase tileAbove = groundMap.GetTile(posAbove);
                if (tile == null)
                {
                    pitMap.SetTile(pos, pitTile);   //setting grass tile
                    if(Random.Range(0f,1f)<0.1f)
                    {
                        GameObject newTree = Instantiate(trees[Random.Range(0,3)]);
                        newTree.transform.position = pitMap.GetCellCenterLocal(pos);
                    }
                    if(Random.Range(0f,1f)<0.05f)
                    {
                        GameObject newProp = Instantiate(props);
                        newProp.GetComponent<SpriteRenderer>().sprite = propSprites[Random.Range(0,12)];
                        newProp.transform.position = pitMap.GetCellCenterLocal(pos);
                    }

                    if (tileBelow != null)
                    {
                        wallMap.SetTile(pos, topWallTile);
                    }
                    else if (tileAbove != null)
                    {
                        wallMap.SetTile(pos, botWallTile);
                    }
                }
            }
        }
    }

    private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos)
    {
        if (routeCount < maxRoutes)
        {
            routeCount++;
            while (++routeLength < maxRouteLength)
            {
                //Initialize
                bool routeUsed = false;
                int xOffset = x - previousPos.x; //0
                int yOffset = y - previousPos.y; //3
                int roomSize = 1; //Hallway size
                if (Random.Range(1, 100) <= roomRate)
                    roomSize = Random.Range(3, 6);
                previousPos = new Vector2Int(x, y);

                //Go Straight
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
                        NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        x = previousPos.x + xOffset;
                        y = previousPos.y + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }

                //Go left
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
                        NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y + xOffset;
                        x = previousPos.x - yOffset;
                        GenerateSquare(x, y, roomSize);
                        
                        routeUsed = true;
                    }
                }
                //Go right
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
                        NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y - xOffset;
                        x = previousPos.x + yOffset;
                        GenerateSquare(x, y, roomSize);
                        
                        routeUsed = true;
                    }
                }

                if (!routeUsed)
                {
                    x = previousPos.x + xOffset;
                    y = previousPos.y + yOffset;
                    GenerateSquare(x, y, roomSize);
                    spawnSpawner(x,y,0.1f);
                }
            }
        }
    }

    private void GenerateSquare(int x, int y, int radius)
    {
        for (int tileX = x - radius; tileX <= x + radius; tileX++)
        {
            for (int tileY = y - radius; tileY <= y + radius; tileY++)
            {
                Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
                groundMap.SetTile(tilePos, groundTile);

                //setting up spawners in rooms
                if(radius>1)
                    spawnSpawner(x,y,0.001f);
                
                //spawning props
                if(Random.Range(0f,1f)<0.05f)
                {
                    GameObject newProp = Instantiate(props);
                    newProp.GetComponent<SpriteRenderer>().sprite = propSprites[Random.Range(0,12)];
                    newProp.transform.position = pitMap.GetCellCenterLocal(tilePos);
                }
                if(Random.Range(0f,1f)<0.001f && radius>1)
                    spawnPowerups(x,y);
            }
        }
    }

    void spawnSpawner(int x, int y, float p)
    {
        if(Random.Range(0f,1f)<p)
        {
            GameObject newSpawner = Instantiate(enemySpawner);
            Vector3Int cellPos = new Vector3Int(x,y,0);
            newSpawner.transform.position = groundMap.GetCellCenterLocal(cellPos);
        }
    }

    void spawnPowerups(int x, int y)
    {
        if(powerupSpawnCount<2)
        {
            int roll = Random.Range(0,4);
            if(CEO_script.powerupSpawned[roll]==0)
            {
                CEO_script.powerupSpawned[roll]=1;
                GameObject powerUp = Instantiate(powerups[roll]);
                Vector3Int cellPos = new Vector3Int(x,y,0);
                powerUp.transform.position = groundMap.GetCellCenterLocal(cellPos);
                powerupSpawnCount++;
                return;
            }
        }
    }
    
}

