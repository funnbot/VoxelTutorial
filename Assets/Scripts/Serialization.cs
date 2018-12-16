using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Serialization {
    public static string saveFolder = "voxelGameSaves";

    public static void Save(Chunk chunk) {
        var save = new Save(chunk);
        if (save.blocks.Count == 0) return;

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static bool Load(Chunk chunk) {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        if (!File.Exists(saveFile)) return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        var save = (Save)formatter.Deserialize(stream);
        foreach (var block in save.blocks) {
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }

        stream.Close();
        return true;
    }

    public static string FileName(Vector3Int chunkLocation) {
        return chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
    }

    public static string SaveLocation(string worldName) {
        string saveLocation = saveFolder + "/" + worldName + "/";
        if (!Directory.Exists(saveLocation)) {
            Directory.CreateDirectory(saveLocation);
        }
        return saveLocation;
    }
}