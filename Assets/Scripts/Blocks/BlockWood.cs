 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;

 [System.Serializable]
 public class BlockWood : Block {
     public BlockWood() : base() { }

     public override Tile TexturePosition(Direction direction) {
         Tile tile = new Tile();
         switch (direction) {
             case Direction.Up:
                 tile.x = 2;
                 tile.y = 1;
                 return tile;
             case Direction.Down:
                 tile.x = 2;
                 tile.y = 1;
                 return tile;
         }
         tile.x = 1;
         tile.y = 1;
         return tile;
     }
 }