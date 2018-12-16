using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer), typeof (MeshCollider))]
public class Chunk : MonoBehaviour {
    public static int chunkSize = 16;
    public bool update = true;

    MeshFilter filter;
    MeshCollider coll;

    private Block[, , ] blocks;

    void Start () {
        filter = GetComponent<MeshFilter> ();
        coll = GetComponent<MeshCollider> ();

        blocks = new Block[chunkSize, chunkSize, chunkSize];
        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                for (int z = 0; z < chunkSize; z++) {
                    blocks[x, y, z] = new BlockAir ();
                }
            }
        }
        blocks[3, 5, 2] = new Block ();
        blocks[4, 5, 2] = new BlockGrass(); 
        UpdateChunk ();
    }

    public Block GetBlock (int x, int y, int z) {
        return blocks[x, y, z];
    }

    void UpdateChunk () {
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

    void RenderMesh (MeshData meshData) {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();
    }
}