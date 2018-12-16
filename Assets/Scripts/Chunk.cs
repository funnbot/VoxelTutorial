using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
    public static int chunkSize = 16;

    public World world { get; set; }
    public Vector3Int pos { get; set; }
    public bool update { get; set; }
    public bool rendered { get; set; }

    public Block[, , ] blocks = new Block[chunkSize, chunkSize, chunkSize];

    MeshFilter filter;
    MeshCollider coll;

    void Start() {
        filter = GetComponent<MeshFilter>();
        coll = GetComponent<MeshCollider>();
    }

    void Update() {
        if (update) {
            update = false;
            UpdateChunk();
        }
    }

    public Block GetBlock(int x, int y, int z) {
        if (InRange(x, y, z)) return blocks[x, y, z];
        else return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    public void SetBlock(int x, int y, int z, Block block) {
        if (InRange(x, y, z)) {
            blocks[x, y, z] = block;
            update = true;
        } else world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
    }
    public void SetBlocksUnmodified() {
        foreach (var block in blocks) {
            block.changed = false;
        }
    }

    void UpdateChunk() {
        rendered = true;
        var meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                for (int z = 0; z < chunkSize; z++) {
                    meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
    }

    void RenderMesh(MeshData meshData) {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

    public static bool InRange(int x, int y, int z) {
        return InRange(x) && InRange(y) && InRange(z);
    }

    public static bool InRange(int index) {
        return index >= 0 && index < chunkSize;
    }
}