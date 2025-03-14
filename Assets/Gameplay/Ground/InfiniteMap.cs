using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteMap : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap baseTilemap;
    public Tilemap collisionTilemap;
    public int chunkSize = 10;
    public int renderDistance = 1;
    public GameObject parent;
    public GameObject collisionPefab;

    //private Chunk[] chunks;
    //private Dictionary<Vector2Int, GameObject> activeChunks = new();
    //private Queue<GameObject> chunkPool = new();
    //private const string collisionName = "CollisionLayer";

    //public class Chunk
    //{
    //    public TileBase[,] tiles;
    //    public TileBase[,] collisionTiles;
    //    public Vector2Int posChunk;

    //    public Chunk(TileBase[,] tiles, Vector2Int posChunk, TileBase[,] collisionTiles)
    //    {
    //        this.tiles = tiles;
    //        this.posChunk = posChunk;
    //        this.collisionTiles = collisionTiles;
    //    }
    //}

    //private void Awake()
    //{
    //    PlayerController.OnPositionChange += UpdateMap;
    //}

    //private void OnDestroy()
    //{
    //    PlayerController.OnPositionChange -= UpdateMap;
    //}



    private Chunk[] chunks;
    private Dictionary<Vector2Int, GameObject> activeChunks = new();
    private Queue<GameObject> chunkPool = new();
    private const string collisionName = "CollisionLayer";
    private PlayerMovement player;

    public class Chunk
    {
        public TileBase[,] tiles;
        public TileBase[,] collisionTiles;
        public Vector2Int posChunk;

        public Chunk(TileBase[,] tiles, Vector2Int posChunk, TileBase[,] collisionTiles)
        {
            this.tiles = tiles;
            this.posChunk = posChunk;
            this.collisionTiles = collisionTiles;
        }
    }

    private void OnDrawGizmos()
    {
        baseTilemap.CompressBounds();
        Gizmos.color = Color.red;
        BoundsInt bounds = baseTilemap.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;
        Vector3Int topLeftCell = new Vector3Int(bounds.xMin, bounds.yMax, 0);
        Gizmos.DrawWireCube(new Vector3(topLeftCell.x + width / 2f, topLeftCell.y - height / 2f), new Vector3(width, height));
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (player != null)
        {
            UpdateMap(player.transform.position);
        }
    }

    private void Start()
    {
        if (baseTilemap == null)
            baseTilemap = GetComponent<Tilemap>();
        if (collisionTilemap == null)
        {
            GameObject collisionGO = new GameObject("CollisionTilemap");
            collisionGO.transform.parent = parent.transform;
            collisionTilemap = collisionGO.AddComponent<Tilemap>();
            var renderer = collisionGO.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = 1;
            var collider = collisionGO.AddComponent<TilemapCollider2D>();
            collider.usedByComposite = true;
            collisionGO.AddComponent<CompositeCollider2D>();
            collisionGO.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        baseTilemap.CompressBounds();
        BoundsInt bounds = baseTilemap.cellBounds;
        chunks = new Chunk[(bounds.size.x / chunkSize) * (bounds.size.y / chunkSize)];
        CutTileMapInChunk();
    }

    private void CutTileMapInChunk()
    {
        BoundsInt bounds = baseTilemap.cellBounds;
        for (int i = 0; i < bounds.size.y / chunkSize; i++)
        {
            for (int j = 0; j < bounds.size.x / chunkSize; j++)
            {
                chunks[i * (bounds.size.x / chunkSize) + j] = ExtractChunk(j, i);
            }
        }
    }

    private Chunk ExtractChunk(int xPos, int yPos)
    {
        TileBase[,] tiles = new TileBase[chunkSize, chunkSize];
        TileBase[,] collision = new TileBase[chunkSize, chunkSize];
        BoundsInt bounds = baseTilemap.cellBounds;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int tileX = bounds.xMin + x + xPos * chunkSize;
                int tileY = bounds.yMin + y + yPos * chunkSize;
                tiles[x, y] = baseTilemap.GetTile(new Vector3Int(tileX, tileY, 0));
                collision[x, y] = collisionTilemap.GetTile(new Vector3Int(tileX, tileY, 0));
            }
        }
        return new Chunk(tiles, new Vector2Int(xPos, yPos), collision);
    }

    private void UpdateMap(Vector3 playerPos)
    {
        Vector2Int playerChunk = new Vector2Int(
            Mathf.FloorToInt(playerPos.x / (chunkSize * baseTilemap.cellSize.x)),
            Mathf.FloorToInt(playerPos.y / (chunkSize * baseTilemap.cellSize.y))
        );

        HashSet<Vector2Int> visibleChunks = new HashSet<Vector2Int>();

        for (int y = -renderDistance; y <= renderDistance; y++)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                Vector2Int chunkCoords = new Vector2Int(playerChunk.x + x, playerChunk.y + y);
                visibleChunks.Add(chunkCoords);
                GenerateChunkAtPosition(new Vector3(
                    chunkCoords.x * chunkSize * baseTilemap.cellSize.x,
                    chunkCoords.y * chunkSize * baseTilemap.cellSize.y,
                    0));
            }
        }

        RemoveUnseenChunks(visibleChunks);
    }

    private void RemoveUnseenChunks(HashSet<Vector2Int> visibleChunks)
    {
        List<Vector2Int> chunksToRemove = new();

        foreach (var kvp in activeChunks)
        {
            if (!visibleChunks.Contains(kvp.Key))
            {
                kvp.Value.SetActive(false);
                chunkPool.Enqueue(kvp.Value);
                chunksToRemove.Add(kvp.Key);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            activeChunks.Remove(coord);
        }
    }


    private void GenerateChunkAtPosition(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / (chunkSize * baseTilemap.cellSize.x));
        int chunkY = Mathf.FloorToInt(position.y / (chunkSize * baseTilemap.cellSize.y));

        Vector2Int chunkCoords = new Vector2Int(chunkX, chunkY);
        if (activeChunks.ContainsKey(chunkCoords))
            return;

        GameObject chunkGO = GetOrCreateChunkGameObject(chunkCoords);
        Tilemap chunkTilemap = chunkGO.GetComponent<Tilemap>();
        Tilemap chunkCollisionMap = chunkGO.transform.Find(collisionName).GetComponent<Tilemap>();

        Vector3 worldPos = new Vector3(
            chunkX * chunkSize * baseTilemap.cellSize.x,
            chunkY * chunkSize * baseTilemap.cellSize.y,
            0);

        chunkGO.transform.position = worldPos;

        chunkTilemap.ClearAllTiles();
        chunkCollisionMap.ClearAllTiles();

        int chunksPerRow = baseTilemap.cellBounds.size.x / chunkSize;
        int chunksPerColumn = baseTilemap.cellBounds.size.y / chunkSize;

        Chunk chunkData = chunks[
            Mod(chunkY, chunksPerColumn) * chunksPerRow +
            Mod(chunkX, chunksPerRow)
        ];

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                if (chunkData.tiles[x, y] != null)
                    chunkTilemap.SetTile(new Vector3Int(x, y, 0), chunkData.tiles[x, y]);
                if (chunkData.collisionTiles[x, y] != null)
                    chunkCollisionMap.SetTile(new Vector3Int(x, y, 0), chunkData.collisionTiles[x, y]);
            }
        }

        activeChunks[chunkCoords] = chunkGO;
    }

    private GameObject GetOrCreateChunkGameObject(Vector2Int chunkCoords)
    {
        GameObject chunkGO;
        if (chunkPool.Count > 0)
        {
            chunkGO = chunkPool.Dequeue();
            chunkGO.SetActive(true);
        }
        else
        {
            chunkGO = new GameObject($"Chunk_{chunkCoords.x}_{chunkCoords.y}");
            chunkGO.transform.parent = parent.transform;
            chunkGO.AddComponent<Tilemap>();
            chunkGO.AddComponent<TilemapRenderer>();

            GameObject collisionGO = Instantiate(collisionPefab, chunkGO.transform);
            collisionGO.name = collisionName;
        }
        return chunkGO;
    }

    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}