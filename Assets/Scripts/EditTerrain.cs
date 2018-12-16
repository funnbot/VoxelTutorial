using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditTerrain {
    public static Vector3Int GetBlockPos(Vector3 pos) {
        return Vector3Int.RoundToInt(pos);
    }
    public static Vector3Int GetBlockPos(RaycastHit hit, bool adjacent = false) {
        var blockPos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent));
        return GetBlockPos(blockPos);
    }
    public static Block GetBlock(RaycastHit hit, bool adjacent = false) {
        var chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null) return null;

        var pos = GetBlockPos(hit, adjacent);
        return chunk.world.GetBlock(pos.x, pos.y, pos.z);
    }
    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false) {
        var chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null) return false;

        var pos = GetBlockPos(hit, adjacent);
        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
        return true;
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false) {
        if (pos - (int) pos == 0.5f || pos - (int) pos == -0.5f) {
            if (adjacent) pos += (norm / 2);
            else pos -= (norm / 2);
        }
        return pos;
    }
}