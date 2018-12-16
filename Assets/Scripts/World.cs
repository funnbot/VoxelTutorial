using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    public string worldName { get; set; }

    [SerializeField]
    GameObject chunkPrefab;

    public Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    void Start() {
       
    }

    public void CreateChunk(int x, int y, int z) {
        var worldPos = new Vector3Int(x, y, z);
        var newChunkObject = Instantiate(chunkPrefab, worldPos, Quaternion.identity, transform);
        var newChunk = newChunkObject.GetComponent<Chunk>();
        newChunk.pos = worldPos;
        newChunk.world = this;
        chunks.Add(worldPos, newChunk);

        var terrain = new TerrainGen();
        newChunk = terrain.ChunkGen(newChunk);
        newChunk.SetBlocksUnmodified();
        bool loaded = Serialization.Load(newChunk);
    }

    public void DestroyChunk(int x, int y, int z) {
        var pos = new Vector3Int(x, y, z);
        Chunk chunk = null;
        if (chunks.TryGetValue(pos, out chunk)) {
            Serialization.Save(chunk);
            Destroy(chunk.gameObject);
            chunks.Remove(pos);
        }
    }

    public Chunk GetChunk(int x, int y, int z) {
        float multiple = Chunk.chunkSize;
        var pos = Vector3Int.FloorToInt(new Vector3(x, y, z) / multiple) * Chunk.chunkSize;
        Chunk container = null;
        chunks.TryGetValue(pos, out container);
        return container;
    }

    public Block GetBlock(int x, int y, int z) {
        var container = GetChunk(x, y, z);
        if (container != null) {
            var block = container.GetBlock(
                x - container.pos.x,
                y - container.pos.y,
                z - container.pos.z);
            return block;
        } else return new BlockAir();
    }

    public void SetBlock(int x, int y, int z, Block block) {
        var chunk = GetChunk(x, y, z);
        if (chunk != null) {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);

            UpdateIfEqual(x - chunk.pos.x, 0, new Vector3Int(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new Vector3Int(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new Vector3Int(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new Vector3Int(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new Vector3Int(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new Vector3Int(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, Vector3Int pos) {
        if (value1 == value2) {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
}