using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChunks : MonoBehaviour {
    public World world;

    List<Vector3Int> updateList = new List<Vector3Int>();
    List<Vector3Int> buildList = new List<Vector3Int>();

    int timer = 0;

    void Update() {
        if (DeleteChunks())
            return;
        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    void FindChunksToLoad() {
        // Player pos centered around a chunk coord
        Vector3Int playerPos = new Vector3Int(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
        );

        if (updateList.Count == 0) {
            for (int i = 0; i < chunkPositions.Length; i++) {
                var newChunkPos = new Vector3Int(
                    chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                    0,
                    chunkPositions[i].z * Chunk.chunkSize + playerPos.z);

                var newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

                if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

                for (int y = -4; y < 4; y++) {
                    for (int x = newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize) {
                        for (int z = newChunkPos.z - Chunk.chunkSize; z <= newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize) {
                            buildList.Add(new Vector3Int(
                                x, y * Chunk.chunkSize, z));
                        }
                    }
                    updateList.Add(new Vector3Int(
                        newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                }
                return;
            }
        }
    }

    void BuildChunk(Vector3Int pos) {
        if (world.GetChunk(pos.x,pos.y,pos.z) == null)
             world.CreateChunk(pos.x,pos.y,pos.z);
    }

    void LoadAndRenderChunks() {
        if (buildList.Count != 0) {
            for (int i = 0; i < buildList.Count && i < 8; i++) {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }
            //If chunks were built return early
            return;
        }
        if (updateList.Count != 0) {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.update = true;
            updateList.RemoveAt(0);
        }
    }

    bool DeleteChunks() {
        if (timer == 10) {
            var chunksToDelete = new List<Vector3Int>();
            foreach (var chunk in world.chunks) {
                float distance = Vector3.Distance(new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));
                if (distance > 256) chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete) {
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);
            }
            timer = 0;
            return true;
        }
        timer++;
        return false;
    }

    static Vector3Int[] chunkPositions = {
        new Vector3Int(0, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, 0, 1),
        new Vector3Int(-2, 0, 0),
        new Vector3Int(0, 0, -2),
        new Vector3Int(0, 0, 2),
        new Vector3Int(2, 0, 0),
        new Vector3Int(-2, 0, -1),
        new Vector3Int(-2, 0, 1),
        new Vector3Int(-1, 0, -2),
        new Vector3Int(-1, 0, 2),
        new Vector3Int(1, 0, -2),
        new Vector3Int(1, 0, 2),
        new Vector3Int(2, 0, -1),
        new Vector3Int(2, 0, 1),
        new Vector3Int(-2, 0, -2),
        new Vector3Int(-2, 0, 2),
        new Vector3Int(2, 0, -2),
        new Vector3Int(2, 0, 2),
        new Vector3Int(-3, 0, 0),
        new Vector3Int(0, 0, -3),
        new Vector3Int(0, 0, 3),
        new Vector3Int(3, 0, 0),
        new Vector3Int(-3, 0, -1),
        new Vector3Int(-3, 0, 1),
        new Vector3Int(-1, 0, -3),
        new Vector3Int(-1, 0, 3),
        new Vector3Int(1, 0, -3),
        new Vector3Int(1, 0, 3),
        new Vector3Int(3, 0, -1),
        new Vector3Int(3, 0, 1),
        new Vector3Int(-3, 0, -2),
        new Vector3Int(-3, 0, 2),
        new Vector3Int(-2, 0, -3),
        new Vector3Int(-2, 0, 3),
        new Vector3Int(2, 0, -3),
        new Vector3Int(2, 0, 3),
        new Vector3Int(3, 0, -2),
        new Vector3Int(3, 0, 2),
        new Vector3Int(-4, 0, 0),
        new Vector3Int(0, 0, -4),
        new Vector3Int(0, 0, 4),
        new Vector3Int(4, 0, 0),
        new Vector3Int(-4, 0, -1),
        new Vector3Int(-4, 0, 1),
        new Vector3Int(-1, 0, -4),
        new Vector3Int(-1, 0, 4),
        new Vector3Int(1, 0, -4),
        new Vector3Int(1, 0, 4),
        new Vector3Int(4, 0, -1),
        new Vector3Int(4, 0, 1),
        new Vector3Int(-3, 0, -3),
        new Vector3Int(-3, 0, 3),
        new Vector3Int(3, 0, -3),
        new Vector3Int(3, 0, 3),
        new Vector3Int(-4, 0, -2),
        new Vector3Int(-4, 0, 2),
        new Vector3Int(-2, 0, -4),
        new Vector3Int(-2, 0, 4),
        new Vector3Int(2, 0, -4),
        new Vector3Int(2, 0, 4),
        new Vector3Int(4, 0, -2),
        new Vector3Int(4, 0, 2),
        new Vector3Int(-5, 0, 0),
        new Vector3Int(-4, 0, -3),
        new Vector3Int(-4, 0, 3),
        new Vector3Int(-3, 0, -4),
        new Vector3Int(-3, 0, 4),
        new Vector3Int(0, 0, -5),
        new Vector3Int(0, 0, 5),
        new Vector3Int(3, 0, -4),
        new Vector3Int(3, 0, 4),
        new Vector3Int(4, 0, -3),
        new Vector3Int(4, 0, 3),
        new Vector3Int(5, 0, 0),
        new Vector3Int(-5, 0, -1),
        new Vector3Int(-5, 0, 1),
        new Vector3Int(-1, 0, -5),
        new Vector3Int(-1, 0, 5),
        new Vector3Int(1, 0, -5),
        new Vector3Int(1, 0, 5),
        new Vector3Int(5, 0, -1),
        new Vector3Int(5, 0, 1),
        new Vector3Int(-5, 0, -2),
        new Vector3Int(-5, 0, 2),
        new Vector3Int(-2, 0, -5),
        new Vector3Int(-2, 0, 5),
        new Vector3Int(2, 0, -5),
        new Vector3Int(2, 0, 5),
        new Vector3Int(5, 0, -2),
        new Vector3Int(5, 0, 2),
        new Vector3Int(-4, 0, -4),
        new Vector3Int(-4, 0, 4),
        new Vector3Int(4, 0, -4),
        new Vector3Int(4, 0, 4),
        new Vector3Int(-5, 0, -3),
        new Vector3Int(-5, 0, 3),
        new Vector3Int(-3, 0, -5),
        new Vector3Int(-3, 0, 5),
        new Vector3Int(3, 0, -5),
        new Vector3Int(3, 0, 5),
        new Vector3Int(5, 0, -3),
        new Vector3Int(5, 0, 3),
        new Vector3Int(-6, 0, 0),
        new Vector3Int(0, 0, -6),
        new Vector3Int(0, 0, 6),
        new Vector3Int(6, 0, 0),
        new Vector3Int(-6, 0, -1),
        new Vector3Int(-6, 0, 1),
        new Vector3Int(-1, 0, -6),
        new Vector3Int(-1, 0, 6),
        new Vector3Int(1, 0, -6),
        new Vector3Int(1, 0, 6),
        new Vector3Int(6, 0, -1),
        new Vector3Int(6, 0, 1),
        new Vector3Int(-6, 0, -2),
        new Vector3Int(-6, 0, 2),
        new Vector3Int(-2, 0, -6),
        new Vector3Int(-2, 0, 6),
        new Vector3Int(2, 0, -6),
        new Vector3Int(2, 0, 6),
        new Vector3Int(6, 0, -2),
        new Vector3Int(6, 0, 2),
        new Vector3Int(-5, 0, -4),
        new Vector3Int(-5, 0, 4),
        new Vector3Int(-4, 0, -5),
        new Vector3Int(-4, 0, 5),
        new Vector3Int(4, 0, -5),
        new Vector3Int(4, 0, 5),
        new Vector3Int(5, 0, -4),
        new Vector3Int(5, 0, 4),
        new Vector3Int(-6, 0, -3),
        new Vector3Int(-6, 0, 3),
        new Vector3Int(-3, 0, -6),
        new Vector3Int(-3, 0, 6),
        new Vector3Int(3, 0, -6),
        new Vector3Int(3, 0, 6),
        new Vector3Int(6, 0, -3),
        new Vector3Int(6, 0, 3),
        new Vector3Int(-7, 0, 0),
        new Vector3Int(0, 0, -7),
        new Vector3Int(0, 0, 7),
        new Vector3Int(7, 0, 0),
        new Vector3Int(-7, 0, -1),
        new Vector3Int(-7, 0, 1),
        new Vector3Int(-5, 0, -5),
        new Vector3Int(-5, 0, 5),
        new Vector3Int(-1, 0, -7),
        new Vector3Int(-1, 0, 7),
        new Vector3Int(1, 0, -7),
        new Vector3Int(1, 0, 7),
        new Vector3Int(5, 0, -5),
        new Vector3Int(5, 0, 5),
        new Vector3Int(7, 0, -1),
        new Vector3Int(7, 0, 1),
        new Vector3Int(-6, 0, -4),
        new Vector3Int(-6, 0, 4),
        new Vector3Int(-4, 0, -6),
        new Vector3Int(-4, 0, 6),
        new Vector3Int(4, 0, -6),
        new Vector3Int(4, 0, 6),
        new Vector3Int(6, 0, -4),
        new Vector3Int(6, 0, 4),
        new Vector3Int(-7, 0, -2),
        new Vector3Int(-7, 0, 2),
        new Vector3Int(-2, 0, -7),
        new Vector3Int(-2, 0, 7),
        new Vector3Int(2, 0, -7),
        new Vector3Int(2, 0, 7),
        new Vector3Int(7, 0, -2),
        new Vector3Int(7, 0, 2),
        new Vector3Int(-7, 0, -3),
        new Vector3Int(-7, 0, 3),
        new Vector3Int(-3, 0, -7),
        new Vector3Int(-3, 0, 7),
        new Vector3Int(3, 0, -7),
        new Vector3Int(3, 0, 7),
        new Vector3Int(7, 0, -3),
        new Vector3Int(7, 0, 3),
        new Vector3Int(-6, 0, -5),
        new Vector3Int(-6, 0, 5),
        new Vector3Int(-5, 0, -6),
        new Vector3Int(-5, 0, 6),
        new Vector3Int(5, 0, -6),
        new Vector3Int(5, 0, 6),
        new Vector3Int(6, 0, -5),
        new Vector3Int(6, 0, 5)
    };
}