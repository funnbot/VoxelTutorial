using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    public Dictionary<SerializableVector3Int, Block> blocks =
        new Dictionary<SerializableVector3Int, Block>();

    public Save(Chunk chunk) {
        for (int x = 0; x < Chunk.chunkSize; x++) {
            for (int y = 0; y < Chunk.chunkSize; y++) {
                for (int z = 0; z < Chunk.chunkSize; z++) {
                    if (!chunk.blocks[x, y, z].changed)
                        continue;
                    var pos = VecToSerial(new Vector3Int(x, y, z));
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
    }

    SerializableVector3Int VecToSerial(Vector3Int vec) {
        return new SerializableVector3Int(vec.x, vec.y, vec.z);
    }

    [System.Serializable]
    public struct SerializableVector3Int {
        public int x, y, z;
        public SerializableVector3Int(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj) {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }
    }
}