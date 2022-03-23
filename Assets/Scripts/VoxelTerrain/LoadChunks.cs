using System.Collections.Generic;
using UnityEngine;

namespace VoxelTerrain
{
    public class LoadChunks : MonoBehaviour
    {
        public World world;
        private List<WorldPos> updateList = new List<WorldPos>();
        private List<WorldPos> buildList = new List<WorldPos>();
        //private Vector3Int[] chunkPositions;

        private int timer = 0;

        //[SerializeField, Range(2, 10)] private int perimeter = 7;
        private static WorldPos[] chunkPositions = 
       { 
           new WorldPos(0, 0, 0), new WorldPos(-1, 0, 0), new WorldPos(0, 0, -1), new WorldPos(0, 0, 1),
          new WorldPos(0, 1, 0), new WorldPos(-1, 1, 0), new WorldPos(0, 1, -1), new WorldPos(0, 1, 1),
          new WorldPos(0, 2, 0), new WorldPos(-1, 2, 0), new WorldPos(0, 2, -1), new WorldPos(0, 2, 1),
          new WorldPos(0, 3, 0), new WorldPos(-1, 3, 0), new WorldPos(0, 3, -1), new WorldPos(0, 3, 1),
          new WorldPos(0, 4, 0), new WorldPos(-1, 4, 0), new WorldPos(0, 4, -1), new WorldPos(0, 4, 1),
          new WorldPos(0, 5, 0), new WorldPos(-1, 5, 0), new WorldPos(0, 5, -1), new WorldPos(0, 5, 1),
          new WorldPos(0, 6, 0), new WorldPos(-1, 6, 0), new WorldPos(0, 6, -1), new WorldPos(0, 6, 1),
          new WorldPos(0, 7, 0), new WorldPos(-1, 7, 0), new WorldPos(0, 7, -1), new WorldPos(0, 7, 1),
          new WorldPos(0, -1, 0), new WorldPos(-1, -1, 0), new WorldPos(0, -1, -1), new WorldPos(0, -1, 1),
          new WorldPos(0, -2, 0), new WorldPos(-1, -2, 0), new WorldPos(0, -2, -1), new WorldPos(0, -2, 1),
          new WorldPos(0, -3, 0), new WorldPos(-1, -3, 0), new WorldPos(0, -3, -1), new WorldPos(0, -3, 1),
          new WorldPos(0, -4, 0), new WorldPos(-1, -4, 0), new WorldPos(0, -4, -1), new WorldPos(0, -4, 1),
          new WorldPos(0, -5, 0), new WorldPos(-1, -5, 0), new WorldPos(0, -5, -1), new WorldPos(0, -5, 1),
          new WorldPos(0, -6, 0), new WorldPos(-1, -6, 0), new WorldPos(0, -6, -1), new WorldPos(0, -6, 1),
          new WorldPos(0, -7, 0), new WorldPos(-1, -7, 0), new WorldPos(0, -7, -1), new WorldPos(0, -7, 1),
          new WorldPos(1, 0, 0), new WorldPos(-1, 0, -1), new WorldPos(-1, 0, 1), new WorldPos(1, 0, -1), new WorldPos(1, 0, 1),
          new WorldPos(1, 1, 0), new WorldPos(-1, 1, -1), new WorldPos(-1, 1, 1), new WorldPos(1, 1, -1), new WorldPos(1, 1, 1),
          new WorldPos(1, 2, 0), new WorldPos(-1, 2, -1), new WorldPos(-1, 2, 1), new WorldPos(1, 2, -1), new WorldPos(1, 2, 1),
          new WorldPos(1, 3, 0), new WorldPos(-1, 3, -1), new WorldPos(-1, 3, 1), new WorldPos(1, 3, -1), new WorldPos(1, 3, 1),
          new WorldPos(1, 4, 0), new WorldPos(-1, 4, -1), new WorldPos(-1, 4, 1), new WorldPos(1, 4, -1), new WorldPos(1, 4, 1),
          new WorldPos(1, 5, 0), new WorldPos(-1, 5, -1), new WorldPos(-1, 5, 1), new WorldPos(1, 5, -1), new WorldPos(1, 5, 1),
          new WorldPos(1, 6, 0), new WorldPos(-1, 6, -1), new WorldPos(-1, 6, 1), new WorldPos(1, 6, -1), new WorldPos(1, 6, 1),
          new WorldPos(1, 7, 0), new WorldPos(-1, 7, -1), new WorldPos(-1, 7, 1), new WorldPos(1, 7, -1), new WorldPos(1, 7, 1),
          new WorldPos(1, -1, 0), new WorldPos(-1, -1, -1), new WorldPos(-1, -1, 1), new WorldPos(1, -1, -1), new WorldPos(1, -1, 1),
          new WorldPos(1, -2, 0), new WorldPos(-1, -2, -1), new WorldPos(-1, -2, 1), new WorldPos(1, -2, -1), new WorldPos(1, -2, 1),
          new WorldPos(1, -3, 0), new WorldPos(-1, -3, -1), new WorldPos(-1, -3, 1), new WorldPos(1, -3, -1), new WorldPos(1, -3, 1),
          new WorldPos(1, -4, 0), new WorldPos(-1, -4, -1), new WorldPos(-1, -4, 1), new WorldPos(1, -4, -1), new WorldPos(1, -4, 1),
          new WorldPos(1, -5, 0), new WorldPos(-1, -5, -1), new WorldPos(-1, -5, 1), new WorldPos(1, -5, -1), new WorldPos(1, -5, 1),
          new WorldPos(1, -6, 0), new WorldPos(-1, -6, -1), new WorldPos(-1, -6, 1), new WorldPos(1, -6, -1), new WorldPos(1, -6, 1),
          new WorldPos(1, -7, 0), new WorldPos(-1, -7, -1), new WorldPos(-1, -7, 1), new WorldPos(1, -7, -1), new WorldPos(1, -7, 1),
          new WorldPos(-2, 0, 0), new WorldPos(0, 0, -2), new WorldPos(0, 0, 2), new WorldPos(2, 0, 0), new WorldPos(-2, 0, -1),
          new WorldPos(-2, 1, 0), new WorldPos(0, 1, -2), new WorldPos(0, 1, 2), new WorldPos(2, 1, 0), new WorldPos(-2, 1, -1),
          new WorldPos(-2, 2, 0), new WorldPos(0, 2, -2), new WorldPos(0, 2, 2), new WorldPos(2, 2, 0), new WorldPos(-2, 2, -1),
          new WorldPos(-2, 3, 0), new WorldPos(0, 3, -2), new WorldPos(0, 3, 2), new WorldPos(2, 3, 0), new WorldPos(-2, 3, -1),
          new WorldPos(-2, 4, 0), new WorldPos(0, 4, -2), new WorldPos(0, 4, 2), new WorldPos(2, 4, 0), new WorldPos(-2, 4, -1),
          new WorldPos(-2, 5, 0), new WorldPos(0, 5, -2), new WorldPos(0, 5, 2), new WorldPos(2, 5, 0), new WorldPos(-2, 5, -1),
          new WorldPos(-2, 6, 0), new WorldPos(0, 6, -2), new WorldPos(0, 6, 2), new WorldPos(2, 6, 0), new WorldPos(-2, 6, -1),
          new WorldPos(-2, 7, 0), new WorldPos(0, 7, -2), new WorldPos(0, 7, 2), new WorldPos(2, 7, 0), new WorldPos(-2, 7, -1),
          new WorldPos(-2, -1, 0), new WorldPos(0, -1, -2), new WorldPos(0, -1, 2), new WorldPos(2, -1, 0), new WorldPos(-2, -1, -1),
          new WorldPos(-2, -2, 0), new WorldPos(0, -2, -2), new WorldPos(0, -2, 2), new WorldPos(2, -2, 0), new WorldPos(-2, -2, -1),
          new WorldPos(-2, -3, 0), new WorldPos(0, -3, -2), new WorldPos(0, -3, 2), new WorldPos(2, -3, 0), new WorldPos(-2, -3, -1),
          new WorldPos(-2, -4, 0), new WorldPos(0, -4, -2), new WorldPos(0, -4, 2), new WorldPos(2, -4, 0), new WorldPos(-2, -4, -1),
          new WorldPos(-2, -5, 0), new WorldPos(0, -5, -2), new WorldPos(0, -5, 2), new WorldPos(2, -5, 0), new WorldPos(-2, -5, -1),
          new WorldPos(-2, -6, 0), new WorldPos(0, -6, -2), new WorldPos(0, -6, 2), new WorldPos(2, -6, 0), new WorldPos(-2, -6, -1),
           new WorldPos(-2, -7, 0), new WorldPos(0, -7, -2), new WorldPos(0, -7, 2), new WorldPos(2, -7, 0), new WorldPos(-2, -7, -1),
           new WorldPos(-2, 0, 1), new WorldPos(-1, 0, -2), new WorldPos(-1, 0, 2), new WorldPos(1, 0, -2), new WorldPos(1, 0, 2),
           new WorldPos(-2, 1, 1), new WorldPos(-1, 1, -2), new WorldPos(-1, 1, 2), new WorldPos(1, 1, -2), new WorldPos(1, 1, 2),
           new WorldPos(-2, 2, 1), new WorldPos(-1, 2, -2), new WorldPos(-1, 2, 2), new WorldPos(1, 2, -2), new WorldPos(1, 2, 2),
           new WorldPos(-2, 3, 1), new WorldPos(-1, 3, -2), new WorldPos(-1, 3, 2), new WorldPos(1, 3, -2), new WorldPos(1, 3, 2),
           new WorldPos(-2, 4, 1), new WorldPos(-1, 4, -2), new WorldPos(-1, 4, 2), new WorldPos(1, 4, -2), new WorldPos(1, 4, 2),
           new WorldPos(-2, 5, 1), new WorldPos(-1, 5, -2), new WorldPos(-1, 5, 2), new WorldPos(1, 5, -2), new WorldPos(1, 5, 2),
           new WorldPos(-2, 6, 1), new WorldPos(-1, 6, -2), new WorldPos(-1, 6, 2), new WorldPos(1, 6, -2), new WorldPos(1, 6, 2),
           new WorldPos(-2, 7, 1), new WorldPos(-1, 7, -2), new WorldPos(-1, 7, 2), new WorldPos(1, 7, -2), new WorldPos(1, 7, 2),
           new WorldPos(-2, -1, 1), new WorldPos(-1, -1, -2), new WorldPos(-1, -1, 2), new WorldPos(1, -1, -2), new WorldPos(1, -1, 2),
           new WorldPos(-2, -2, 1), new WorldPos(-1, -2, -2), new WorldPos(-1, -2, 2), new WorldPos(1, -2, -2), new WorldPos(1, -2, 2),
           new WorldPos(-2, -3, 1), new WorldPos(-1, -3, -2), new WorldPos(-1, -3, 2), new WorldPos(1, -3, -2), new WorldPos(1, -3, 2),
           new WorldPos(-2, -4, 1), new WorldPos(-1, -4, -2), new WorldPos(-1, -4, 2), new WorldPos(1, -4, -2), new WorldPos(1, -4, 2),
           new WorldPos(-2, -5, 1), new WorldPos(-1, -5, -2), new WorldPos(-1, -5, 2), new WorldPos(1, -5, -2), new WorldPos(1, -5, 2),
           new WorldPos(-2, -6, 1), new WorldPos(-1, -6, -2), new WorldPos(-1, -6, 2), new WorldPos(1, -6, -2), new WorldPos(1, -6, 2),
           new WorldPos(-2, -7, 1), new WorldPos(-1, -7, -2), new WorldPos(-1, -7, 2), new WorldPos(1, -7, -2), new WorldPos(1, -7, 2),
           new WorldPos(2, 0, -1), new WorldPos(2, 0, 1), new WorldPos(-2, 0, -2), new WorldPos(-2, 0, 2), new WorldPos(2, 0, -2),
           new WorldPos(2, 1, -1), new WorldPos(2, 1, 1), new WorldPos(-2, 1, -2), new WorldPos(-2, 1, 2), new WorldPos(2, 1, -2),
           new WorldPos(2, 2, -1), new WorldPos(2, 2, 1), new WorldPos(-2, 2, -2), new WorldPos(-2, 2, 2), new WorldPos(2, 2, -2),
           new WorldPos(2, 3, -1), new WorldPos(2, 3, 1), new WorldPos(-2, 3, -2), new WorldPos(-2, 3, 2), new WorldPos(2, 3, -2),
           new WorldPos(2, 4, -1), new WorldPos(2, 4, 1), new WorldPos(-2, 4, -2), new WorldPos(-2, 4, 2), new WorldPos(2, 4, -2),
           new WorldPos(2, 5, -1), new WorldPos(2, 5, 1), new WorldPos(-2, 5, -2), new WorldPos(-2, 5, 2), new WorldPos(2, 5, -2),
           new WorldPos(2, 6, -1), new WorldPos(2, 6, 1), new WorldPos(-2, 6, -2), new WorldPos(-2, 6, 2), new WorldPos(2, 6, -2),
           new WorldPos(2, 7, -1), new WorldPos(2, 7, 1), new WorldPos(-2, 7, -2), new WorldPos(-2, 7, 2), new WorldPos(2, 7, -2),
           new WorldPos(2, -1, -1), new WorldPos(2, -1, 1), new WorldPos(-2, -1, -2), new WorldPos(-2, -1, 2), new WorldPos(2, -1, -2),
           new WorldPos(2, -2, -1), new WorldPos(2, -2, 1), new WorldPos(-2, -2, -2), new WorldPos(-2, -2, 2), new WorldPos(2, -2, -2),
           new WorldPos(2, -3, -1), new WorldPos(2, -3, 1), new WorldPos(-2, -3, -2), new WorldPos(-2, -3, 2), new WorldPos(2, -3, -2),
           new WorldPos(2, -4, -1), new WorldPos(2, -4, 1), new WorldPos(-2, -4, -2), new WorldPos(-2, -4, 2), new WorldPos(2, -4, -2),
           new WorldPos(2, -5, -1), new WorldPos(2, -5, 1), new WorldPos(-2, -5, -2), new WorldPos(-2, -5, 2), new WorldPos(2, -5, -2),
           new WorldPos(2, -6, -1), new WorldPos(2, -6, 1), new WorldPos(-2, -6, -2), new WorldPos(-2, -6, 2), new WorldPos(2, -6, -2),
           new WorldPos(2, -7, -1), new WorldPos(2, -7, 1), new WorldPos(-2, -7, -2), new WorldPos(-2, -7, 2), new WorldPos(2, -7, -2),
           new WorldPos(2, 0, 2), new WorldPos(-3, 0, 0), new WorldPos(0, 0, -3), new WorldPos(0, 0, 3), new WorldPos(3, 0, 0),
           new WorldPos(2, 1, 2), new WorldPos(-3, 1, 0), new WorldPos(0, 1, -3), new WorldPos(0, 1, 3), new WorldPos(3, 1, 0),
           new WorldPos(2, 2, 2), new WorldPos(-3, 2, 0), new WorldPos(0, 2, -3), new WorldPos(0, 2, 3), new WorldPos(3, 2, 0),
           new WorldPos(2, 3, 2), new WorldPos(-3, 3, 0), new WorldPos(0, 3, -3), new WorldPos(0, 3, 3), new WorldPos(3, 3, 0),
           new WorldPos(2, 4, 2), new WorldPos(-3, 4, 0), new WorldPos(0, 4, -3), new WorldPos(0, 4, 3), new WorldPos(3, 4, 0),
           new WorldPos(2, 5, 2), new WorldPos(-3, 5, 0), new WorldPos(0, 5, -3), new WorldPos(0, 5, 3), new WorldPos(3, 5, 0),
           new WorldPos(2, 6, 2), new WorldPos(-3, 6, 0), new WorldPos(0, 6, -3), new WorldPos(0, 6, 3), new WorldPos(3, 6, 0),
           new WorldPos(2, 7, 2), new WorldPos(-3, 7, 0), new WorldPos(0, 7, -3), new WorldPos(0, 7, 3), new WorldPos(3, 7, 0),
           new WorldPos(2, -1, 2), new WorldPos(-3, -1, 0), new WorldPos(0, -1, -3), new WorldPos(0, -1, 3), new WorldPos(3, -1, 0),
           new WorldPos(2, -2, 2), new WorldPos(-3, -2, 0), new WorldPos(0, -2, -3), new WorldPos(0, -2, 3), new WorldPos(3, -2, 0),
           new WorldPos(2, -3, 2), new WorldPos(-3, -3, 0), new WorldPos(0, -3, -3), new WorldPos(0, -3, 3), new WorldPos(3, -3, 0),
           new WorldPos(2, -4, 2), new WorldPos(-3, -4, 0), new WorldPos(0, -4, -3), new WorldPos(0, -4, 3), new WorldPos(3, -4, 0),
           new WorldPos(2, -5, 2), new WorldPos(-3, -5, 0), new WorldPos(0, -5, -3), new WorldPos(0, -5, 3), new WorldPos(3, -5, 0),
           new WorldPos(2, -6, 2), new WorldPos(-3, -6, 0), new WorldPos(0, -6, -3), new WorldPos(0, -6, 3), new WorldPos(3, -6, 0),
           new WorldPos(2, -7, 2), new WorldPos(-3, -7, 0), new WorldPos(0, -7, -3), new WorldPos(0, -7, 3), new WorldPos(3, -7, 0),
           new WorldPos(-3, 0, -1), new WorldPos(-3, 0, 1), new WorldPos(-1, 0, -3), new WorldPos(-1, 0, 3), new WorldPos(1, 0, -3),
           new WorldPos(-3, 1, -1), new WorldPos(-3, 1, 1), new WorldPos(-1, 1, -3), new WorldPos(-1, 1, 3), new WorldPos(1, 1, -3),
           new WorldPos(-3, 2, -1), new WorldPos(-3, 2, 1), new WorldPos(-1, 2, -3), new WorldPos(-1, 2, 3), new WorldPos(1, 2, -3),
           new WorldPos(-3, 3, -1), new WorldPos(-3, 3, 1), new WorldPos(-1, 3, -3), new WorldPos(-1, 3, 3), new WorldPos(1, 3, -3),
           new WorldPos(-3, 4, -1), new WorldPos(-3, 4, 1), new WorldPos(-1, 4, -3), new WorldPos(-1, 4, 3), new WorldPos(1, 4, -3),
           new WorldPos(-3, 5, -1), new WorldPos(-3, 5, 1), new WorldPos(-1, 5, -3), new WorldPos(-1, 5, 3), new WorldPos(1, 5, -3),
           new WorldPos(-3, 6, -1), new WorldPos(-3, 6, 1), new WorldPos(-1, 6, -3), new WorldPos(-1, 6, 3), new WorldPos(1, 6, -3),
           new WorldPos(-3, 7, -1), new WorldPos(-3, 7, 1), new WorldPos(-1, 7, -3), new WorldPos(-1, 7, 3), new WorldPos(1, 7, -3),
           new WorldPos(-3, -1, -1), new WorldPos(-3, -1, 1), new WorldPos(-1, -1, -3), new WorldPos(-1, -1, 3), new WorldPos(1, -1, -3),
           new WorldPos(-3, -2, -1), new WorldPos(-3, -2, 1), new WorldPos(-1, -2, -3), new WorldPos(-1, -2, 3), new WorldPos(1, -2, -3),
           new WorldPos(-3, -3, -1), new WorldPos(-3, -3, 1), new WorldPos(-1, -3, -3), new WorldPos(-1, -3, 3), new WorldPos(1, -3, -3),
           new WorldPos(-3, -4, -1), new WorldPos(-3, -4, 1), new WorldPos(-1, -4, -3), new WorldPos(-1, -4, 3), new WorldPos(1, -4, -3),
           new WorldPos(-3, -5, -1), new WorldPos(-3, -5, 1), new WorldPos(-1, -5, -3), new WorldPos(-1, -5, 3), new WorldPos(1, -5, -3),
           new WorldPos(-3, -6, -1), new WorldPos(-3, -6, 1), new WorldPos(-1, -6, -3), new WorldPos(-1, -6, 3), new WorldPos(1, -6, -3),
           new WorldPos(-3, -7, -1), new WorldPos(-3, -7, 1), new WorldPos(-1, -7, -3), new WorldPos(-1, -7, 3), new WorldPos(1, -7, -3),
           new WorldPos(1, 0, 3), new WorldPos(3, 0, -1), new WorldPos(3, 0, 1), new WorldPos(-3, 0, -2), new WorldPos(-3, 0, 2),
           new WorldPos(1, 1, 3), new WorldPos(3, 1, -1), new WorldPos(3, 1, 1), new WorldPos(-3, 1, -2), new WorldPos(-3, 1, 2),
           new WorldPos(1, 2, 3), new WorldPos(3, 2, -1), new WorldPos(3, 2, 1), new WorldPos(-3, 2, -2), new WorldPos(-3, 2, 2),
           new WorldPos(1, 3, 3), new WorldPos(3, 3, -1), new WorldPos(3, 3, 1), new WorldPos(-3, 3, -2), new WorldPos(-3, 3, 2),
           new WorldPos(1, 4, 3), new WorldPos(3, 4, -1), new WorldPos(3, 4, 1), new WorldPos(-3, 4, -2), new WorldPos(-3, 4, 2),
           new WorldPos(1, 5, 3), new WorldPos(3, 5, -1), new WorldPos(3, 5, 1), new WorldPos(-3, 5, -2), new WorldPos(-3, 5, 2),
           new WorldPos(1, 6, 3), new WorldPos(3, 6, -1), new WorldPos(3, 6, 1), new WorldPos(-3, 6, -2), new WorldPos(-3, 6, 2),
           new WorldPos(1, 7, 3), new WorldPos(3, 7, -1), new WorldPos(3, 7, 1), new WorldPos(-3, 7, -2), new WorldPos(-3, 7, 2),
           new WorldPos(1, -1, 3), new WorldPos(3, -1, -1), new WorldPos(3, -1, 1), new WorldPos(-3, -1, -2), new WorldPos(-3, -1, 2),
           new WorldPos(1, -2, 3), new WorldPos(3, -2, -1), new WorldPos(3, -2, 1), new WorldPos(-3, -2, -2), new WorldPos(-3, -2, 2),
           new WorldPos(1, -3, 3), new WorldPos(3, -3, -1), new WorldPos(3, -3, 1), new WorldPos(-3, -3, -2), new WorldPos(-3, -3, 2),
           new WorldPos(1, -4, 3), new WorldPos(3, -4, -1), new WorldPos(3, -4, 1), new WorldPos(-3, -4, -2), new WorldPos(-3, -4, 2),
           new WorldPos(1, -5, 3), new WorldPos(3, -5, -1), new WorldPos(3, -5, 1), new WorldPos(-3, -5, -2), new WorldPos(-3, -5, 2),
           new WorldPos(1, -6, 3), new WorldPos(3, -6, -1), new WorldPos(3, -6, 1), new WorldPos(-3, -6, -2), new WorldPos(-3, -6, 2),
           new WorldPos(1, -7, 3), new WorldPos(3, -7, -1), new WorldPos(3, -7, 1), new WorldPos(-3, -7, -2), new WorldPos(-3, -7, 2),
           new WorldPos(-2, 0, -3), new WorldPos(-2, 0, 3), new WorldPos(2, 0, -3), new WorldPos(2, 0, 3), new WorldPos(3, 0, -2),
           new WorldPos(-2, 1, -3), new WorldPos(-2, 1, 3), new WorldPos(2, 1, -3), new WorldPos(2, 1, 3), new WorldPos(3, 1, -2),
           new WorldPos(-2, 2, -3), new WorldPos(-2, 2, 3), new WorldPos(2, 2, -3), new WorldPos(2, 2, 3), new WorldPos(3, 2, -2),
           new WorldPos(-2, 3, -3), new WorldPos(-2, 3, 3), new WorldPos(2, 3, -3), new WorldPos(2, 3, 3), new WorldPos(3, 3, -2),
           new WorldPos(-2, 4, -3), new WorldPos(-2, 4, 3), new WorldPos(2, 4, -3), new WorldPos(2, 4, 3), new WorldPos(3, 4, -2),
           new WorldPos(-2, 5, -3), new WorldPos(-2, 5, 3), new WorldPos(2, 5, -3), new WorldPos(2, 5, 3), new WorldPos(3, 5, -2),
           new WorldPos(-2, 6, -3), new WorldPos(-2, 6, 3), new WorldPos(2, 6, -3), new WorldPos(2, 6, 3), new WorldPos(3, 6, -2),
           new WorldPos(-2, 7, -3), new WorldPos(-2, 7, 3), new WorldPos(2, 7, -3), new WorldPos(2, 7, 3), new WorldPos(3, 7, -2),
           new WorldPos(-2, -1, -3), new WorldPos(-2, -1, 3), new WorldPos(2, -1, -3), new WorldPos(2, -1, 3), new WorldPos(3, -1, -2),
           new WorldPos(-2, -2, -3), new WorldPos(-2, -2, 3), new WorldPos(2, -2, -3), new WorldPos(2, -2, 3), new WorldPos(3, -2, -2),
           new WorldPos(-2, -3, -3), new WorldPos(-2, -3, 3), new WorldPos(2, -3, -3), new WorldPos(2, -3, 3), new WorldPos(3, -3, -2),
           new WorldPos(-2, -4, -3), new WorldPos(-2, -4, 3), new WorldPos(2, -4, -3), new WorldPos(2, -4, 3), new WorldPos(3, -4, -2),
           new WorldPos(-2, -5, -3), new WorldPos(-2, -5, 3), new WorldPos(2, -5, -3), new WorldPos(2, -5, 3), new WorldPos(3, -5, -2),
           new WorldPos(-2, -6, -3), new WorldPos(-2, -6, 3), new WorldPos(2, -6, -3), new WorldPos(2, -6, 3), new WorldPos(3, -6, -2),
           new WorldPos(-2, -7, -3), new WorldPos(-2, -7, 3), new WorldPos(2, -7, -3), new WorldPos(2, -7, 3), new WorldPos(3, -7, -2),
           new WorldPos(3, 0, 2), new WorldPos(-4, 0, 0), new WorldPos(0, 0, -4), new WorldPos(0, 0, 4), new WorldPos(4, 0, 0),
           new WorldPos(3, 1, 2), new WorldPos(-4, 1, 0), new WorldPos(0, 1, -4), new WorldPos(0, 1, 4), new WorldPos(4, 1, 0),
           new WorldPos(3, 2, 2), new WorldPos(-4, 2, 0), new WorldPos(0, 2, -4), new WorldPos(0, 2, 4), new WorldPos(4, 2, 0),
           new WorldPos(3, 3, 2), new WorldPos(-4, 3, 0), new WorldPos(0, 3, -4), new WorldPos(0, 3, 4), new WorldPos(4, 3, 0),
           new WorldPos(3, 4, 2), new WorldPos(-4, 4, 0), new WorldPos(0, 4, -4), new WorldPos(0, 4, 4), new WorldPos(4, 4, 0),
           new WorldPos(3, 5, 2), new WorldPos(-4, 5, 0), new WorldPos(0, 5, -4), new WorldPos(0, 5, 4), new WorldPos(4, 5, 0),
           new WorldPos(3, 6, 2), new WorldPos(-4, 6, 0), new WorldPos(0, 6, -4), new WorldPos(0, 6, 4), new WorldPos(4, 6, 0),
           new WorldPos(3, 7, 2), new WorldPos(-4, 7, 0), new WorldPos(0, 7, -4), new WorldPos(0, 7, 4), new WorldPos(4, 7, 0),
           new WorldPos(3, -1, 2), new WorldPos(-4, -1, 0), new WorldPos(0, -1, -4), new WorldPos(0, -1, 4), new WorldPos(4, -1, 0),
           new WorldPos(3, -2, 2), new WorldPos(-4, -2, 0), new WorldPos(0, -2, -4), new WorldPos(0, -2, 4), new WorldPos(4, -2, 0),
           new WorldPos(3, -3, 2), new WorldPos(-4, -3, 0), new WorldPos(0, -3, -4), new WorldPos(0, -3, 4), new WorldPos(4, -3, 0),
           new WorldPos(3, -4, 2), new WorldPos(-4, -4, 0), new WorldPos(0, -4, -4), new WorldPos(0, -4, 4), new WorldPos(4, -4, 0),
           new WorldPos(3, -5, 2), new WorldPos(-4, -5, 0), new WorldPos(0, -5, -4), new WorldPos(0, -5, 4), new WorldPos(4, -5, 0),
           new WorldPos(3, -6, 2), new WorldPos(-4, -6, 0), new WorldPos(0, -6, -4), new WorldPos(0, -6, 4), new WorldPos(4, -6, 0),
           new WorldPos(3, -7, 2), new WorldPos(-4, -7, 0), new WorldPos(0, -7, -4), new WorldPos(0, -7, 4), new WorldPos(4, -7, 0),
           new WorldPos(-4, 0, -1), new WorldPos(-4, 0, 1), new WorldPos(-1, 0, -4), new WorldPos(-1, 0, 4), new WorldPos(1, 0, -4),
           new WorldPos(-4, 1, -1), new WorldPos(-4, 1, 1), new WorldPos(-1, 1, -4), new WorldPos(-1, 1, 4), new WorldPos(1, 1, -4),
           new WorldPos(-4, 2, -1), new WorldPos(-4, 2, 1), new WorldPos(-1, 2, -4), new WorldPos(-1, 2, 4), new WorldPos(1, 2, -4),
           new WorldPos(-4, 3, -1), new WorldPos(-4, 3, 1), new WorldPos(-1, 3, -4), new WorldPos(-1, 3, 4), new WorldPos(1, 3, -4),
           new WorldPos(-4, 4, -1), new WorldPos(-4, 4, 1), new WorldPos(-1, 4, -4), new WorldPos(-1, 4, 4), new WorldPos(1, 4, -4),
           new WorldPos(-4, 5, -1), new WorldPos(-4, 5, 1), new WorldPos(-1, 5, -4), new WorldPos(-1, 5, 4), new WorldPos(1, 5, -4),
           new WorldPos(-4, 6, -1), new WorldPos(-4, 6, 1), new WorldPos(-1, 6, -4), new WorldPos(-1, 6, 4), new WorldPos(1, 6, -4),
           new WorldPos(-4, 7, -1), new WorldPos(-4, 7, 1), new WorldPos(-1, 7, -4), new WorldPos(-1, 7, 4), new WorldPos(1, 7, -4),
           new WorldPos(-4, -1, -1), new WorldPos(-4, -1, 1), new WorldPos(-1, -1, -4), new WorldPos(-1, -1, 4), new WorldPos(1, -1, -4),
           new WorldPos(-4, -2, -1), new WorldPos(-4, -2, 1), new WorldPos(-1, -2, -4), new WorldPos(-1, -2, 4), new WorldPos(1, -2, -4),
           new WorldPos(-4, -3, -1), new WorldPos(-4, -3, 1), new WorldPos(-1, -3, -4), new WorldPos(-1, -3, 4), new WorldPos(1, -3, -4),
           new WorldPos(-4, -4, -1), new WorldPos(-4, -4, 1), new WorldPos(-1, -4, -4), new WorldPos(-1, -4, 4), new WorldPos(1, -4, -4),
           new WorldPos(-4, -5, -1), new WorldPos(-4, -5, 1), new WorldPos(-1, -5, -4), new WorldPos(-1, -5, 4), new WorldPos(1, -5, -4),
           new WorldPos(-4, -6, -1), new WorldPos(-4, -6, 1), new WorldPos(-1, -6, -4), new WorldPos(-1, -6, 4), new WorldPos(1, -6, -4),
           new WorldPos(-4, -7, -1), new WorldPos(-4, -7, 1), new WorldPos(-1, -7, -4), new WorldPos(-1, -7, 4), new WorldPos(1, -7, -4),
           new WorldPos(1, 0, 4), new WorldPos(4, 0, -1), new WorldPos(4, 0, 1), new WorldPos(-3, 0, -3), new WorldPos(-3, 0, 3),
           new WorldPos(1, 1, 4), new WorldPos(4, 1, -1), new WorldPos(4, 1, 1), new WorldPos(-3, 1, -3), new WorldPos(-3, 1, 3),
           new WorldPos(1, 2, 4), new WorldPos(4, 2, -1), new WorldPos(4, 2, 1), new WorldPos(-3, 2, -3), new WorldPos(-3, 2, 3),
           new WorldPos(1, 3, 4), new WorldPos(4, 3, -1), new WorldPos(4, 3, 1), new WorldPos(-3, 3, -3), new WorldPos(-3, 3, 3),
           new WorldPos(1, 4, 4), new WorldPos(4, 4, -1), new WorldPos(4, 4, 1), new WorldPos(-3, 4, -3), new WorldPos(-3, 4, 3),
           new WorldPos(1, 5, 4), new WorldPos(4, 5, -1), new WorldPos(4, 5, 1), new WorldPos(-3, 5, -3), new WorldPos(-3, 5, 3),
           new WorldPos(1, 6, 4), new WorldPos(4, 6, -1), new WorldPos(4, 6, 1), new WorldPos(-3, 6, -3), new WorldPos(-3, 6, 3),
           new WorldPos(1, 7, 4), new WorldPos(4, 7, -1), new WorldPos(4, 7, 1), new WorldPos(-3, 7, -3), new WorldPos(-3, 7, 3),
           new WorldPos(1, -1, 4), new WorldPos(4, -1, -1), new WorldPos(4, -1, 1), new WorldPos(-3, -1, -3), new WorldPos(-3, -1, 3),
           new WorldPos(1, -2, 4), new WorldPos(4, -2, -1), new WorldPos(4, -2, 1), new WorldPos(-3, -2, -3), new WorldPos(-3, -2, 3),
           new WorldPos(1, -3, 4), new WorldPos(4, -3, -1), new WorldPos(4, -3, 1), new WorldPos(-3, -3, -3), new WorldPos(-3, -3, 3),
           new WorldPos(1, -4, 4), new WorldPos(4, -4, -1), new WorldPos(4, -4, 1), new WorldPos(-3, -4, -3), new WorldPos(-3, -4, 3),
           new WorldPos(1, -5, 4), new WorldPos(4, -5, -1), new WorldPos(4, -5, 1), new WorldPos(-3, -5, -3), new WorldPos(-3, -5, 3),
           new WorldPos(1, -6, 4), new WorldPos(4, -6, -1), new WorldPos(4, -6, 1), new WorldPos(-3, -6, -3), new WorldPos(-3, -6, 3),
           new WorldPos(1, -7, 4), new WorldPos(4, -7, -1), new WorldPos(4, -7, 1), new WorldPos(-3, -7, -3), new WorldPos(-3, -7, 3),
           new WorldPos(3, 0, -3), new WorldPos(3, 0, 3), new WorldPos(-4, 0, -2), new WorldPos(-4, 0, 2), new WorldPos(-2, 0, -4),
           new WorldPos(3, 1, -3), new WorldPos(3, 1, 3), new WorldPos(-4, 1, -2), new WorldPos(-4, 1, 2), new WorldPos(-2, 1, -4),
           new WorldPos(3, 2, -3), new WorldPos(3, 2, 3), new WorldPos(-4, 2, -2), new WorldPos(-4, 2, 2), new WorldPos(-2, 2, -4),
           new WorldPos(3, 3, -3), new WorldPos(3, 3, 3), new WorldPos(-4, 3, -2), new WorldPos(-4, 3, 2), new WorldPos(-2, 3, -4),
           new WorldPos(3, 4, -3), new WorldPos(3, 4, 3), new WorldPos(-4, 4, -2), new WorldPos(-4, 4, 2), new WorldPos(-2, 4, -4),
           new WorldPos(3, 5, -3), new WorldPos(3, 5, 3), new WorldPos(-4, 5, -2), new WorldPos(-4, 5, 2), new WorldPos(-2, 5, -4),
           new WorldPos(3, 6, -3), new WorldPos(3, 6, 3), new WorldPos(-4, 6, -2), new WorldPos(-4, 6, 2), new WorldPos(-2, 6, -4),
           new WorldPos(3, 7, -3), new WorldPos(3, 7, 3), new WorldPos(-4, 7, -2), new WorldPos(-4, 7, 2), new WorldPos(-2, 7, -4),
           new WorldPos(3, -1, -3), new WorldPos(3, -1, 3), new WorldPos(-4, -1, -2), new WorldPos(-4, -1, 2), new WorldPos(-2, -1, -4),
           new WorldPos(3, -2, -3), new WorldPos(3, -2, 3), new WorldPos(-4, -2, -2), new WorldPos(-4, -2, 2), new WorldPos(-2, -2, -4),
           new WorldPos(3, -3, -3), new WorldPos(3, -3, 3), new WorldPos(-4, -3, -2), new WorldPos(-4, -3, 2), new WorldPos(-2, -3, -4),
           new WorldPos(3, -4, -3), new WorldPos(3, -4, 3), new WorldPos(-4, -4, -2), new WorldPos(-4, -4, 2), new WorldPos(-2, -4, -4),
           new WorldPos(3, -5, -3), new WorldPos(3, -5, 3), new WorldPos(-4, -5, -2), new WorldPos(-4, -5, 2), new WorldPos(-2, -5, -4),
           new WorldPos(3, -6, -3), new WorldPos(3, -6, 3), new WorldPos(-4, -6, -2), new WorldPos(-4, -6, 2), new WorldPos(-2, -6, -4),
           new WorldPos(3, -7, -3), new WorldPos(3, -7, 3), new WorldPos(-4, -7, -2), new WorldPos(-4, -7, 2), new WorldPos(-2, -7, -4),
           new WorldPos(-2, 0, 4), new WorldPos(2, 0, -4), new WorldPos(2, 0, 4), new WorldPos(4, 0, -2), new WorldPos(4, 0, 2),
           new WorldPos(-2, 1, 4), new WorldPos(2, 1, -4), new WorldPos(2, 1, 4), new WorldPos(4, 1, -2), new WorldPos(4, 1, 2),
           new WorldPos(-2, 2, 4), new WorldPos(2, 2, -4), new WorldPos(2, 2, 4), new WorldPos(4, 2, -2), new WorldPos(4, 2, 2),
           new WorldPos(-2, 3, 4), new WorldPos(2, 3, -4), new WorldPos(2, 3, 4), new WorldPos(4, 3, -2), new WorldPos(4, 3, 2),
           new WorldPos(-2, 4, 4), new WorldPos(2, 4, -4), new WorldPos(2, 4, 4), new WorldPos(4, 4, -2), new WorldPos(4, 4, 2),
           new WorldPos(-2, 5, 4), new WorldPos(2, 5, -4), new WorldPos(2, 5, 4), new WorldPos(4, 5, -2), new WorldPos(4, 5, 2),
           new WorldPos(-2, 6, 4), new WorldPos(2, 6, -4), new WorldPos(2, 6, 4), new WorldPos(4, 6, -2), new WorldPos(4, 6, 2),
           new WorldPos(-2, 7, 4), new WorldPos(2, 7, -4), new WorldPos(2, 7, 4), new WorldPos(4, 7, -2), new WorldPos(4, 7, 2),
           new WorldPos(-2, -1, 4), new WorldPos(2, -1, -4), new WorldPos(2, -1, 4), new WorldPos(4, -1, -2), new WorldPos(4, -1, 2),
           new WorldPos(-2, -2, 4), new WorldPos(2, -2, -4), new WorldPos(2, -2, 4), new WorldPos(4, -2, -2), new WorldPos(4, -2, 2),
           new WorldPos(-2, -3, 4), new WorldPos(2, -3, -4), new WorldPos(2, -3, 4), new WorldPos(4, -3, -2), new WorldPos(4, -3, 2),
           new WorldPos(-2, -4, 4), new WorldPos(2, -4, -4), new WorldPos(2, -4, 4), new WorldPos(4, -4, -2), new WorldPos(4, -4, 2),
           new WorldPos(-2, -5, 4), new WorldPos(2, -5, -4), new WorldPos(2, -5, 4), new WorldPos(4, -5, -2), new WorldPos(4, -5, 2),
           new WorldPos(-2, -6, 4), new WorldPos(2, -6, -4), new WorldPos(2, -6, 4), new WorldPos(4, -6, -2), new WorldPos(4, -6, 2),
           new WorldPos(-2, -7, 4), new WorldPos(2, -7, -4), new WorldPos(2, -7, 4), new WorldPos(4, -7, -2), new WorldPos(4, -7, 2),
           new WorldPos(-5, 0, 0), new WorldPos(-4, 0, -3), new WorldPos(-4, 0, 3), new WorldPos(-3, 0, -4), new WorldPos(-3, 0, 4),
           new WorldPos(-5, 1, 0), new WorldPos(-4, 1, -3), new WorldPos(-4, 1, 3), new WorldPos(-3, 1, -4), new WorldPos(-3, 1, 4),
           new WorldPos(-5, 2, 0), new WorldPos(-4, 2, -3), new WorldPos(-4, 2, 3), new WorldPos(-3, 2, -4), new WorldPos(-3, 2, 4),
           new WorldPos(-5, 3, 0), new WorldPos(-4, 3, -3), new WorldPos(-4, 3, 3), new WorldPos(-3, 3, -4), new WorldPos(-3, 3, 4),
           new WorldPos(-5, 4, 0), new WorldPos(-4, 4, -3), new WorldPos(-4, 4, 3), new WorldPos(-3, 4, -4), new WorldPos(-3, 4, 4),
           new WorldPos(-5, 5, 0), new WorldPos(-4, 5, -3), new WorldPos(-4, 5, 3), new WorldPos(-3, 5, -4), new WorldPos(-3, 5, 4),
           new WorldPos(-5, 6, 0), new WorldPos(-4, 6, -3), new WorldPos(-4, 6, 3), new WorldPos(-3, 6, -4), new WorldPos(-3, 6, 4),
           new WorldPos(-5, 7, 0), new WorldPos(-4, 7, -3), new WorldPos(-4, 7, 3), new WorldPos(-3, 7, -4), new WorldPos(-3, 7, 4),
           new WorldPos(-5, -1, 0), new WorldPos(-4, -1, -3), new WorldPos(-4, -1, 3), new WorldPos(-3, -1, -4), new WorldPos(-3, -1, 4),
           new WorldPos(-5, -2, 0), new WorldPos(-4, -2, -3), new WorldPos(-4, -2, 3), new WorldPos(-3, -2, -4), new WorldPos(-3, -2, 4),
           new WorldPos(-5, -3, 0), new WorldPos(-4, -3, -3), new WorldPos(-4, -3, 3), new WorldPos(-3, -3, -4), new WorldPos(-3, -3, 4),
           new WorldPos(-5, -4, 0), new WorldPos(-4, -4, -3), new WorldPos(-4, -4, 3), new WorldPos(-3, -4, -4), new WorldPos(-3, -4, 4),
           new WorldPos(-5, -5, 0), new WorldPos(-4, -5, -3), new WorldPos(-4, -5, 3), new WorldPos(-3, -5, -4), new WorldPos(-3, -5, 4),
           new WorldPos(-5, -6, 0), new WorldPos(-4, -6, -3), new WorldPos(-4, -6, 3), new WorldPos(-3, -6, -4), new WorldPos(-3, -6, 4),
           new WorldPos(-5, -7, 0), new WorldPos(-4, -7, -3), new WorldPos(-4, -7, 3), new WorldPos(-3, -7, -4), new WorldPos(-3, -7, 4),
           new WorldPos(0, 0, -5), new WorldPos(0, 0, 5), new WorldPos(3, 0, -4), new WorldPos(3, 0, 4), new WorldPos(4, 0, -3),
           new WorldPos(0, 1, -5), new WorldPos(0, 1, 5), new WorldPos(3, 1, -4), new WorldPos(3, 1, 4), new WorldPos(4, 1, -3),
           new WorldPos(0, 2, -5), new WorldPos(0, 2, 5), new WorldPos(3, 2, -4), new WorldPos(3, 2, 4), new WorldPos(4, 2, -3),
           new WorldPos(0, 3, -5), new WorldPos(0, 3, 5), new WorldPos(3, 3, -4), new WorldPos(3, 3, 4), new WorldPos(4, 3, -3),
           new WorldPos(0, 4, -5), new WorldPos(0, 4, 5), new WorldPos(3, 4, -4), new WorldPos(3, 4, 4), new WorldPos(4, 4, -3),
           new WorldPos(0, 5, -5), new WorldPos(0, 5, 5), new WorldPos(3, 5, -4), new WorldPos(3, 5, 4), new WorldPos(4, 5, -3),
           new WorldPos(0, 6, -5), new WorldPos(0, 6, 5), new WorldPos(3, 6, -4), new WorldPos(3, 6, 4), new WorldPos(4, 6, -3),
           new WorldPos(0, 7, -5), new WorldPos(0, 7, 5), new WorldPos(3, 7, -4), new WorldPos(3, 7, 4), new WorldPos(4, 7, -3),
           new WorldPos(0, -1, -5), new WorldPos(0, -1, 5), new WorldPos(3, -1, -4), new WorldPos(3, -1, 4), new WorldPos(4, -1, -3),
           new WorldPos(0, -2, -5), new WorldPos(0, -2, 5), new WorldPos(3, -2, -4), new WorldPos(3, -2, 4), new WorldPos(4, -2, -3),
           new WorldPos(0, -3, -5), new WorldPos(0, -3, 5), new WorldPos(3, -3, -4), new WorldPos(3, -3, 4), new WorldPos(4, -3, -3),
           new WorldPos(0, -4, -5), new WorldPos(0, -4, 5), new WorldPos(3, -4, -4), new WorldPos(3, -4, 4), new WorldPos(4, -4, -3),
           new WorldPos(0, -5, -5), new WorldPos(0, -5, 5), new WorldPos(3, -5, -4), new WorldPos(3, -5, 4), new WorldPos(4, -5, -3),
           new WorldPos(0, -6, -5), new WorldPos(0, -6, 5), new WorldPos(3, -6, -4), new WorldPos(3, -6, 4), new WorldPos(4, -6, -3),
           new WorldPos(0, -7, -5), new WorldPos(0, -7, 5), new WorldPos(3, -7, -4), new WorldPos(3, -7, 4), new WorldPos(4, -7, -3),
           new WorldPos(4, 0, 3), new WorldPos(5, 0, 0), new WorldPos(-5, 0, -1), new WorldPos(-5, 0, 1), new WorldPos(-1, 0, -5),
           new WorldPos(4, 1, 3), new WorldPos(5, 1, 0), new WorldPos(-5, 1, -1), new WorldPos(-5, 1, 1), new WorldPos(-1, 1, -5),
           new WorldPos(4, 2, 3), new WorldPos(5, 2, 0), new WorldPos(-5, 2, -1), new WorldPos(-5, 2, 1), new WorldPos(-1, 2, -5),
           new WorldPos(4, 3, 3), new WorldPos(5, 3, 0), new WorldPos(-5, 3, -1), new WorldPos(-5, 3, 1), new WorldPos(-1, 3, -5),
           new WorldPos(4, 4, 3), new WorldPos(5, 4, 0), new WorldPos(-5, 4, -1), new WorldPos(-5, 4, 1), new WorldPos(-1, 4, -5),
           new WorldPos(4, 5, 3), new WorldPos(5, 5, 0), new WorldPos(-5, 5, -1), new WorldPos(-5, 5, 1), new WorldPos(-1, 5, -5),
           new WorldPos(4, 6, 3), new WorldPos(5, 6, 0), new WorldPos(-5, 6, -1), new WorldPos(-5, 6, 1), new WorldPos(-1, 6, -5),
           new WorldPos(4, 7, 3), new WorldPos(5, 7, 0), new WorldPos(-5, 7, -1), new WorldPos(-5, 7, 1), new WorldPos(-1, 7, -5),
           new WorldPos(4, -1, 3), new WorldPos(5, -1, 0), new WorldPos(-5, -1, -1), new WorldPos(-5, -1, 1), new WorldPos(-1, -1, -5),
           new WorldPos(4, -2, 3), new WorldPos(5, -2, 0), new WorldPos(-5, -2, -1), new WorldPos(-5, -2, 1), new WorldPos(-1, -2, -5),
           new WorldPos(4, -3, 3), new WorldPos(5, -3, 0), new WorldPos(-5, -3, -1), new WorldPos(-5, -3, 1), new WorldPos(-1, -3, -5),
           new WorldPos(4, -4, 3), new WorldPos(5, -4, 0), new WorldPos(-5, -4, -1), new WorldPos(-5, -4, 1), new WorldPos(-1, -4, -5),
           new WorldPos(4, -5, 3), new WorldPos(5, -5, 0), new WorldPos(-5, -5, -1), new WorldPos(-5, -5, 1), new WorldPos(-1, -5, -5),
           new WorldPos(4, -6, 3), new WorldPos(5, -6, 0), new WorldPos(-5, -6, -1), new WorldPos(-5, -6, 1), new WorldPos(-1, -6, -5),
           new WorldPos(4, -7, 3), new WorldPos(5, -7, 0), new WorldPos(-5, -7, -1), new WorldPos(-5, -7, 1), new WorldPos(-1, -7, -5),
           new WorldPos(-1, 0, 5), new WorldPos(1, 0, -5), new WorldPos(1, 0, 5), new WorldPos(5, 0, -1), new WorldPos(5, 0, 1),
           new WorldPos(-1, 1, 5), new WorldPos(1, 1, -5), new WorldPos(1, 1, 5), new WorldPos(5, 1, -1), new WorldPos(5, 1, 1),
           new WorldPos(-1, 2, 5), new WorldPos(1, 2, -5), new WorldPos(1, 2, 5), new WorldPos(5, 2, -1), new WorldPos(5, 2, 1),
           new WorldPos(-1, 3, 5), new WorldPos(1, 3, -5), new WorldPos(1, 3, 5), new WorldPos(5, 3, -1), new WorldPos(5, 3, 1),
           new WorldPos(-1, 4, 5), new WorldPos(1, 4, -5), new WorldPos(1, 4, 5), new WorldPos(5, 4, -1), new WorldPos(5, 4, 1),
           new WorldPos(-1, 5, 5), new WorldPos(1, 5, -5), new WorldPos(1, 5, 5), new WorldPos(5, 5, -1), new WorldPos(5, 5, 1),
           new WorldPos(-1, 6, 5), new WorldPos(1, 6, -5), new WorldPos(1, 6, 5), new WorldPos(5, 6, -1), new WorldPos(5, 6, 1),
           new WorldPos(-1, 7, 5), new WorldPos(1, 7, -5), new WorldPos(1, 7, 5), new WorldPos(5, 7, -1), new WorldPos(5, 7, 1),
           new WorldPos(-1, -1, 5), new WorldPos(1, -1, -5), new WorldPos(1, -1, 5), new WorldPos(5, -1, -1), new WorldPos(5, -1, 1),
           new WorldPos(-1, -2, 5), new WorldPos(1, -2, -5), new WorldPos(1, -2, 5), new WorldPos(5, -2, -1), new WorldPos(5, -2, 1),
           new WorldPos(-1, -3, 5), new WorldPos(1, -3, -5), new WorldPos(1, -3, 5), new WorldPos(5, -3, -1), new WorldPos(5, -3, 1),
           new WorldPos(-1, -4, 5), new WorldPos(1, -4, -5), new WorldPos(1, -4, 5), new WorldPos(5, -4, -1), new WorldPos(5, -4, 1),
           new WorldPos(-1, -5, 5), new WorldPos(1, -5, -5), new WorldPos(1, -5, 5), new WorldPos(5, -5, -1), new WorldPos(5, -5, 1),
           new WorldPos(-1, -6, 5), new WorldPos(1, -6, -5), new WorldPos(1, -6, 5), new WorldPos(5, -6, -1), new WorldPos(5, -6, 1),
           new WorldPos(-1, -7, 5), new WorldPos(1, -7, -5), new WorldPos(1, -7, 5), new WorldPos(5, -7, -1), new WorldPos(5, -7, 1),
           new WorldPos(-5, 0, -2), new WorldPos(-5, 0, 2), new WorldPos(-2, 0, -5), new WorldPos(-2, 0, 5), new WorldPos(2, 0, -5),
           new WorldPos(-5, 1, -2), new WorldPos(-5, 1, 2), new WorldPos(-2, 1, -5), new WorldPos(-2, 1, 5), new WorldPos(2, 1, -5),
           new WorldPos(-5, 2, -2), new WorldPos(-5, 2, 2), new WorldPos(-2, 2, -5), new WorldPos(-2, 2, 5), new WorldPos(2, 2, -5),
           new WorldPos(-5, 3, -2), new WorldPos(-5, 3, 2), new WorldPos(-2, 3, -5), new WorldPos(-2, 3, 5), new WorldPos(2, 3, -5),
           new WorldPos(-5, 4, -2), new WorldPos(-5, 4, 2), new WorldPos(-2, 4, -5), new WorldPos(-2, 4, 5), new WorldPos(2, 4, -5),
           new WorldPos(-5, 5, -2), new WorldPos(-5, 5, 2), new WorldPos(-2, 5, -5), new WorldPos(-2, 5, 5), new WorldPos(2, 5, -5),
           new WorldPos(-5, 6, -2), new WorldPos(-5, 6, 2), new WorldPos(-2, 6, -5), new WorldPos(-2, 6, 5), new WorldPos(2, 6, -5),
           new WorldPos(-5, 7, -2), new WorldPos(-5, 7, 2), new WorldPos(-2, 7, -5), new WorldPos(-2, 7, 5), new WorldPos(2, 7, -5),
           new WorldPos(-5, -1, -2), new WorldPos(-5, -1, 2), new WorldPos(-2, -1, -5), new WorldPos(-2, -1, 5), new WorldPos(2, -1, -5),
           new WorldPos(-5, -2, -2), new WorldPos(-5, -2, 2), new WorldPos(-2, -2, -5), new WorldPos(-2, -2, 5), new WorldPos(2, -2, -5),
           new WorldPos(-5, -3, -2), new WorldPos(-5, -3, 2), new WorldPos(-2, -3, -5), new WorldPos(-2, -3, 5), new WorldPos(2, -3, -5),
           new WorldPos(-5, -4, -2), new WorldPos(-5, -4, 2), new WorldPos(-2, -4, -5), new WorldPos(-2, -4, 5), new WorldPos(2, -4, -5),
           new WorldPos(-5, -5, -2), new WorldPos(-5, -5, 2), new WorldPos(-2, -5, -5), new WorldPos(-2, -5, 5), new WorldPos(2, -5, -5),
           new WorldPos(-5, -6, -2), new WorldPos(-5, -6, 2), new WorldPos(-2, -6, -5), new WorldPos(-2, -6, 5), new WorldPos(2, -6, -5),
           new WorldPos(-5, -7, -2), new WorldPos(-5, -7, 2), new WorldPos(-2, -7, -5), new WorldPos(-2, -7, 5), new WorldPos(2, -7, -5),
           new WorldPos(2, 0, 5), new WorldPos(5, 0, -2), new WorldPos(5, 0, 2), new WorldPos(-4, 0, -4), new WorldPos(-4, 0, 4),
           new WorldPos(2, 1, 5), new WorldPos(5, 1, -2), new WorldPos(5, 1, 2), new WorldPos(-4, 1, -4), new WorldPos(-4, 1, 4),
           new WorldPos(2, 2, 5), new WorldPos(5, 2, -2), new WorldPos(5, 2, 2), new WorldPos(-4, 2, -4), new WorldPos(-4, 2, 4),
           new WorldPos(2, 3, 5), new WorldPos(5, 3, -2), new WorldPos(5, 3, 2), new WorldPos(-4, 3, -4), new WorldPos(-4, 3, 4),
           new WorldPos(2, 4, 5), new WorldPos(5, 4, -2), new WorldPos(5, 4, 2), new WorldPos(-4, 4, -4), new WorldPos(-4, 4, 4),
           new WorldPos(2, 5, 5), new WorldPos(5, 5, -2), new WorldPos(5, 5, 2), new WorldPos(-4, 5, -4), new WorldPos(-4, 5, 4),
           new WorldPos(2, 6, 5), new WorldPos(5, 6, -2), new WorldPos(5, 6, 2), new WorldPos(-4, 6, -4), new WorldPos(-4, 6, 4),
           new WorldPos(2, 7, 5), new WorldPos(5, 7, -2), new WorldPos(5, 7, 2), new WorldPos(-4, 7, -4), new WorldPos(-4, 7, 4),
           new WorldPos(2, -1, 5), new WorldPos(5, -1, -2), new WorldPos(5, -1, 2), new WorldPos(-4, -1, -4), new WorldPos(-4, -1, 4),
           new WorldPos(2, -2, 5), new WorldPos(5, -2, -2), new WorldPos(5, -2, 2), new WorldPos(-4, -2, -4), new WorldPos(-4, -2, 4),
           new WorldPos(2, -3, 5), new WorldPos(5, -3, -2), new WorldPos(5, -3, 2), new WorldPos(-4, -3, -4), new WorldPos(-4, -3, 4),
           new WorldPos(2, -4, 5), new WorldPos(5, -4, -2), new WorldPos(5, -4, 2), new WorldPos(-4, -4, -4), new WorldPos(-4, -4, 4),
           new WorldPos(2, -5, 5), new WorldPos(5, -5, -2), new WorldPos(5, -5, 2), new WorldPos(-4, -5, -4), new WorldPos(-4, -5, 4),
           new WorldPos(2, -6, 5), new WorldPos(5, -6, -2), new WorldPos(5, -6, 2), new WorldPos(-4, -6, -4), new WorldPos(-4, -6, 4),
           new WorldPos(2, -7, 5), new WorldPos(5, -7, -2), new WorldPos(5, -7, 2), new WorldPos(-4, -7, -4), new WorldPos(-4, -7, 4),
           new WorldPos(4, 0, -4), new WorldPos(4, 0, 4), new WorldPos(-5, 0, -3), new WorldPos(-5, 0, 3), new WorldPos(-3, 0, -5),
           new WorldPos(4, 1, -4), new WorldPos(4, 1, 4), new WorldPos(-5, 1, -3), new WorldPos(-5, 1, 3), new WorldPos(-3, 1, -5),
           new WorldPos(4, 2, -4), new WorldPos(4, 2, 4), new WorldPos(-5, 2, -3), new WorldPos(-5, 2, 3), new WorldPos(-3, 2, -5),
           new WorldPos(4, 3, -4), new WorldPos(4, 3, 4), new WorldPos(-5, 3, -3), new WorldPos(-5, 3, 3), new WorldPos(-3, 3, -5),
           new WorldPos(4, 4, -4), new WorldPos(4, 4, 4), new WorldPos(-5, 4, -3), new WorldPos(-5, 4, 3), new WorldPos(-3, 4, -5),
           new WorldPos(4, 5, -4), new WorldPos(4, 5, 4), new WorldPos(-5, 5, -3), new WorldPos(-5, 5, 3), new WorldPos(-3, 5, -5),
           new WorldPos(4, 6, -4), new WorldPos(4, 6, 4), new WorldPos(-5, 6, -3), new WorldPos(-5, 6, 3), new WorldPos(-3, 6, -5),
           new WorldPos(4, 7, -4), new WorldPos(4, 7, 4), new WorldPos(-5, 7, -3), new WorldPos(-5, 7, 3), new WorldPos(-3, 7, -5),
           new WorldPos(4, -1, -4), new WorldPos(4, -1, 4), new WorldPos(-5, -1, -3), new WorldPos(-5, -1, 3), new WorldPos(-3, -1, -5),
           new WorldPos(4, -2, -4), new WorldPos(4, -2, 4), new WorldPos(-5, -2, -3), new WorldPos(-5, -2, 3), new WorldPos(-3, -2, -5),
           new WorldPos(4, -3, -4), new WorldPos(4, -3, 4), new WorldPos(-5, -3, -3), new WorldPos(-5, -3, 3), new WorldPos(-3, -3, -5),
           new WorldPos(4, -4, -4), new WorldPos(4, -4, 4), new WorldPos(-5, -4, -3), new WorldPos(-5, -4, 3), new WorldPos(-3, -4, -5),
           new WorldPos(4, -5, -4), new WorldPos(4, -5, 4), new WorldPos(-5, -5, -3), new WorldPos(-5, -5, 3), new WorldPos(-3, -5, -5),
           new WorldPos(4, -6, -4), new WorldPos(4, -6, 4), new WorldPos(-5, -6, -3), new WorldPos(-5, -6, 3), new WorldPos(-3, -6, -5),
           new WorldPos(4, -7, -4), new WorldPos(4, -7, 4), new WorldPos(-5, -7, -3), new WorldPos(-5, -7, 3), new WorldPos(-3, -7, -5),
           new WorldPos(-3, 0, 5), new WorldPos(3, 0, -5), new WorldPos(3, 0, 5), new WorldPos(5, 0, -3), new WorldPos(5, 0, 3),
           new WorldPos(-3, 1, 5), new WorldPos(3, 1, -5), new WorldPos(3, 1, 5), new WorldPos(5, 1, -3), new WorldPos(5, 1, 3),
           new WorldPos(-3, 2, 5), new WorldPos(3, 2, -5), new WorldPos(3, 2, 5), new WorldPos(5, 2, -3), new WorldPos(5, 2, 3),
           new WorldPos(-3, 3, 5), new WorldPos(3, 3, -5), new WorldPos(3, 3, 5), new WorldPos(5, 3, -3), new WorldPos(5, 3, 3),
           new WorldPos(-3, 4, 5), new WorldPos(3, 4, -5), new WorldPos(3, 4, 5), new WorldPos(5, 4, -3), new WorldPos(5, 4, 3),
           new WorldPos(-3, 5, 5), new WorldPos(3, 5, -5), new WorldPos(3, 5, 5), new WorldPos(5, 5, -3), new WorldPos(5, 5, 3),
           new WorldPos(-3, 6, 5), new WorldPos(3, 6, -5), new WorldPos(3, 6, 5), new WorldPos(5, 6, -3), new WorldPos(5, 6, 3),
           new WorldPos(-3, 7, 5), new WorldPos(3, 7, -5), new WorldPos(3, 7, 5), new WorldPos(5, 7, -3), new WorldPos(5, 7, 3),
           new WorldPos(-3, -1, 5), new WorldPos(3, -1, -5), new WorldPos(3, -1, 5), new WorldPos(5, -1, -3), new WorldPos(5, -1, 3),
           new WorldPos(-3, -2, 5), new WorldPos(3, -2, -5), new WorldPos(3, -2, 5), new WorldPos(5, -2, -3), new WorldPos(5, -2, 3),
           new WorldPos(-3, -3, 5), new WorldPos(3, -3, -5), new WorldPos(3, -3, 5), new WorldPos(5, -3, -3), new WorldPos(5, -3, 3),
           new WorldPos(-3, -4, 5), new WorldPos(3, -4, -5), new WorldPos(3, -4, 5), new WorldPos(5, -4, -3), new WorldPos(5, -4, 3),
           new WorldPos(-3, -5, 5), new WorldPos(3, -5, -5), new WorldPos(3, -5, 5), new WorldPos(5, -5, -3), new WorldPos(5, -5, 3),
           new WorldPos(-3, -6, 5), new WorldPos(3, -6, -5), new WorldPos(3, -6, 5), new WorldPos(5, -6, -3), new WorldPos(5, -6, 3),
           new WorldPos(-3, -7, 5), new WorldPos(3, -7, -5), new WorldPos(3, -7, 5), new WorldPos(5, -7, -3), new WorldPos(5, -7, 3),
           new WorldPos(-6, 0, 0), new WorldPos(0, 0, -6), new WorldPos(0, 0, 6), new WorldPos(6, 0, 0), new WorldPos(-6, 0, -1),
           new WorldPos(-6, 1, 0), new WorldPos(0, 1, -6), new WorldPos(0, 1, 6), new WorldPos(6, 1, 0), new WorldPos(-6, 1, -1),
           new WorldPos(-6, 2, 0), new WorldPos(0, 2, -6), new WorldPos(0, 2, 6), new WorldPos(6, 2, 0), new WorldPos(-6, 2, -1),
           new WorldPos(-6, 3, 0), new WorldPos(0, 3, -6), new WorldPos(0, 3, 6), new WorldPos(6, 3, 0), new WorldPos(-6, 3, -1),
           new WorldPos(-6, 4, 0), new WorldPos(0, 4, -6), new WorldPos(0, 4, 6), new WorldPos(6, 4, 0), new WorldPos(-6, 4, -1),
           new WorldPos(-6, 5, 0), new WorldPos(0, 5, -6), new WorldPos(0, 5, 6), new WorldPos(6, 5, 0), new WorldPos(-6, 5, -1),
           new WorldPos(-6, 6, 0), new WorldPos(0, 6, -6), new WorldPos(0, 6, 6), new WorldPos(6, 6, 0), new WorldPos(-6, 6, -1),
           new WorldPos(-6, 7, 0), new WorldPos(0, 7, -6), new WorldPos(0, 7, 6), new WorldPos(6, 7, 0), new WorldPos(-6, 7, -1),
           new WorldPos(-6, -1, 0), new WorldPos(0, -1, -6), new WorldPos(0, -1, 6), new WorldPos(6, -1, 0), new WorldPos(-6, -1, -1),
           new WorldPos(-6, -2, 0), new WorldPos(0, -2, -6), new WorldPos(0, -2, 6), new WorldPos(6, -2, 0), new WorldPos(-6, -2, -1),
           new WorldPos(-6, -3, 0), new WorldPos(0, -3, -6), new WorldPos(0, -3, 6), new WorldPos(6, -3, 0), new WorldPos(-6, -3, -1),
           new WorldPos(-6, -4, 0), new WorldPos(0, -4, -6), new WorldPos(0, -4, 6), new WorldPos(6, -4, 0), new WorldPos(-6, -4, -1),
           new WorldPos(-6, -5, 0), new WorldPos(0, -5, -6), new WorldPos(0, -5, 6), new WorldPos(6, -5, 0), new WorldPos(-6, -5, -1),
           new WorldPos(-6, -6, 0), new WorldPos(0, -6, -6), new WorldPos(0, -6, 6), new WorldPos(6, -6, 0), new WorldPos(-6, -6, -1),
           new WorldPos(-6, -7, 0), new WorldPos(0, -7, -6), new WorldPos(0, -7, 6), new WorldPos(6, -7, 0), new WorldPos(-6, -7, -1),
           new WorldPos(-6, 0, 1), new WorldPos(-1, 0, -6), new WorldPos(-1, 0, 6), new WorldPos(1, 0, -6), new WorldPos(1, 0, 6),
           new WorldPos(-6, 1, 1), new WorldPos(-1, 1, -6), new WorldPos(-1, 1, 6), new WorldPos(1, 1, -6), new WorldPos(1, 1, 6),
           new WorldPos(-6, 2, 1), new WorldPos(-1, 2, -6), new WorldPos(-1, 2, 6), new WorldPos(1, 2, -6), new WorldPos(1, 2, 6),
           new WorldPos(-6, 3, 1), new WorldPos(-1, 3, -6), new WorldPos(-1, 3, 6), new WorldPos(1, 3, -6), new WorldPos(1, 3, 6),
           new WorldPos(-6, 4, 1), new WorldPos(-1, 4, -6), new WorldPos(-1, 4, 6), new WorldPos(1, 4, -6), new WorldPos(1, 4, 6),
           new WorldPos(-6, 5, 1), new WorldPos(-1, 5, -6), new WorldPos(-1, 5, 6), new WorldPos(1, 5, -6), new WorldPos(1, 5, 6),
           new WorldPos(-6, 6, 1), new WorldPos(-1, 6, -6), new WorldPos(-1, 6, 6), new WorldPos(1, 6, -6), new WorldPos(1, 6, 6),
           new WorldPos(-6, 7, 1), new WorldPos(-1, 7, -6), new WorldPos(-1, 7, 6), new WorldPos(1, 7, -6), new WorldPos(1, 7, 6),
           new WorldPos(-6, -1, 1), new WorldPos(-1, -1, -6), new WorldPos(-1, -1, 6), new WorldPos(1, -1, -6), new WorldPos(1, -1, 6),
           new WorldPos(-6, -2, 1), new WorldPos(-1, -2, -6), new WorldPos(-1, -2, 6), new WorldPos(1, -2, -6), new WorldPos(1, -2, 6),
           new WorldPos(-6, -3, 1), new WorldPos(-1, -3, -6), new WorldPos(-1, -3, 6), new WorldPos(1, -3, -6), new WorldPos(1, -3, 6),
           new WorldPos(-6, -4, 1), new WorldPos(-1, -4, -6), new WorldPos(-1, -4, 6), new WorldPos(1, -4, -6), new WorldPos(1, -4, 6),
           new WorldPos(-6, -5, 1), new WorldPos(-1, -5, -6), new WorldPos(-1, -5, 6), new WorldPos(1, -5, -6), new WorldPos(1, -5, 6),
           new WorldPos(-6, -6, 1), new WorldPos(-1, -6, -6), new WorldPos(-1, -6, 6), new WorldPos(1, -6, -6), new WorldPos(1, -6, 6),
           new WorldPos(-6, -7, 1), new WorldPos(-1, -7, -6), new WorldPos(-1, -7, 6), new WorldPos(1, -7, -6), new WorldPos(1, -7, 6),
           new WorldPos(6, 0, -1), new WorldPos(6, 0, 1), new WorldPos(-6, 0, -2), new WorldPos(-6, 0, 2), new WorldPos(-2, 0, -6),
           new WorldPos(6, 1, -1), new WorldPos(6, 1, 1), new WorldPos(-6, 1, -2), new WorldPos(-6, 1, 2), new WorldPos(-2, 1, -6),
           new WorldPos(6, 2, -1), new WorldPos(6, 2, 1), new WorldPos(-6, 2, -2), new WorldPos(-6, 2, 2), new WorldPos(-2, 2, -6),
           new WorldPos(6, 3, -1), new WorldPos(6, 3, 1), new WorldPos(-6, 3, -2), new WorldPos(-6, 3, 2), new WorldPos(-2, 3, -6),
           new WorldPos(6, 4, -1), new WorldPos(6, 4, 1), new WorldPos(-6, 4, -2), new WorldPos(-6, 4, 2), new WorldPos(-2, 4, -6),
           new WorldPos(6, 5, -1), new WorldPos(6, 5, 1), new WorldPos(-6, 5, -2), new WorldPos(-6, 5, 2), new WorldPos(-2, 5, -6),
           new WorldPos(6, 6, -1), new WorldPos(6, 6, 1), new WorldPos(-6, 6, -2), new WorldPos(-6, 6, 2), new WorldPos(-2, 6, -6),
           new WorldPos(6, 7, -1), new WorldPos(6, 7, 1), new WorldPos(-6, 7, -2), new WorldPos(-6, 7, 2), new WorldPos(-2, 7, -6),
           new WorldPos(6, -1, -1), new WorldPos(6, -1, 1), new WorldPos(-6, -1, -2), new WorldPos(-6, -1, 2), new WorldPos(-2, -1, -6),
           new WorldPos(6, -2, -1), new WorldPos(6, -2, 1), new WorldPos(-6, -2, -2), new WorldPos(-6, -2, 2), new WorldPos(-2, -2, -6),
           new WorldPos(6, -3, -1), new WorldPos(6, -3, 1), new WorldPos(-6, -3, -2), new WorldPos(-6, -3, 2), new WorldPos(-2, -3, -6),
           new WorldPos(6, -4, -1), new WorldPos(6, -4, 1), new WorldPos(-6, -4, -2), new WorldPos(-6, -4, 2), new WorldPos(-2, -4, -6),
           new WorldPos(6, -5, -1), new WorldPos(6, -5, 1), new WorldPos(-6, -5, -2), new WorldPos(-6, -5, 2), new WorldPos(-2, -5, -6),
           new WorldPos(6, -6, -1), new WorldPos(6, -6, 1), new WorldPos(-6, -6, -2), new WorldPos(-6, -6, 2), new WorldPos(-2, -6, -6),
           new WorldPos(6, -7, -1), new WorldPos(6, -7, 1), new WorldPos(-6, -7, -2), new WorldPos(-6, -7, 2), new WorldPos(-2, -7, -6),
           new WorldPos(-2, 0, 6), new WorldPos(2, 0, -6), new WorldPos(2, 0, 6), new WorldPos(6, 0, -2), new WorldPos(6, 0, 2),
           new WorldPos(-2, 1, 6), new WorldPos(2, 1, -6), new WorldPos(2, 1, 6), new WorldPos(6, 1, -2), new WorldPos(6, 1, 2),
           new WorldPos(-2, 2, 6), new WorldPos(2, 2, -6), new WorldPos(2, 2, 6), new WorldPos(6, 2, -2), new WorldPos(6, 2, 2),
           new WorldPos(-2, 3, 6), new WorldPos(2, 3, -6), new WorldPos(2, 3, 6), new WorldPos(6, 3, -2), new WorldPos(6, 3, 2),
           new WorldPos(-2, 4, 6), new WorldPos(2, 4, -6), new WorldPos(2, 4, 6), new WorldPos(6, 4, -2), new WorldPos(6, 4, 2),
           new WorldPos(-2, 5, 6), new WorldPos(2, 5, -6), new WorldPos(2, 5, 6), new WorldPos(6, 5, -2), new WorldPos(6, 5, 2),
           new WorldPos(-2, 6, 6), new WorldPos(2, 6, -6), new WorldPos(2, 6, 6), new WorldPos(6, 6, -2), new WorldPos(6, 6, 2),
           new WorldPos(-2, 7, 6), new WorldPos(2, 7, -6), new WorldPos(2, 7, 6), new WorldPos(6, 7, -2), new WorldPos(6, 7, 2),
           new WorldPos(-2, -1, 6), new WorldPos(2, -1, -6), new WorldPos(2, -1, 6), new WorldPos(6, -1, -2), new WorldPos(6, -1, 2),
           new WorldPos(-2, -2, 6), new WorldPos(2, -2, -6), new WorldPos(2, -2, 6), new WorldPos(6, -2, -2), new WorldPos(6, -2, 2),
           new WorldPos(-2, -3, 6), new WorldPos(2, -3, -6), new WorldPos(2, -3, 6), new WorldPos(6, -3, -2), new WorldPos(6, -3, 2),
           new WorldPos(-2, -4, 6), new WorldPos(2, -4, -6), new WorldPos(2, -4, 6), new WorldPos(6, -4, -2), new WorldPos(6, -4, 2),
           new WorldPos(-2, -5, 6), new WorldPos(2, -5, -6), new WorldPos(2, -5, 6), new WorldPos(6, -5, -2), new WorldPos(6, -5, 2),
           new WorldPos(-2, -6, 6), new WorldPos(2, -6, -6), new WorldPos(2, -6, 6), new WorldPos(6, -6, -2), new WorldPos(6, -6, 2),
           new WorldPos(-2, -7, 6), new WorldPos(2, -7, -6), new WorldPos(2, -7, 6), new WorldPos(6, -7, -2), new WorldPos(6, -7, 2),
           new WorldPos(-5, 0, -4), new WorldPos(-5, 0, 4), new WorldPos(-4, 0, -5), new WorldPos(-4, 0, 5), new WorldPos(4, 0, -5),
           new WorldPos(-5, 1, -4), new WorldPos(-5, 1, 4), new WorldPos(-4, 1, -5), new WorldPos(-4, 1, 5), new WorldPos(4, 1, -5),
           new WorldPos(-5, 2, -4), new WorldPos(-5, 2, 4), new WorldPos(-4, 2, -5), new WorldPos(-4, 2, 5), new WorldPos(4, 2, -5),
           new WorldPos(-5, 3, -4), new WorldPos(-5, 3, 4), new WorldPos(-4, 3, -5), new WorldPos(-4, 3, 5), new WorldPos(4, 3, -5),
           new WorldPos(-5, 4, -4), new WorldPos(-5, 4, 4), new WorldPos(-4, 4, -5), new WorldPos(-4, 4, 5), new WorldPos(4, 4, -5),
           new WorldPos(-5, 5, -4), new WorldPos(-5, 5, 4), new WorldPos(-4, 5, -5), new WorldPos(-4, 5, 5), new WorldPos(4, 5, -5),
           new WorldPos(-5, 6, -4), new WorldPos(-5, 6, 4), new WorldPos(-4, 6, -5), new WorldPos(-4, 6, 5), new WorldPos(4, 6, -5),
           new WorldPos(-5, 7, -4), new WorldPos(-5, 7, 4), new WorldPos(-4, 7, -5), new WorldPos(-4, 7, 5), new WorldPos(4, 7, -5),
           new WorldPos(-5, -1, -4), new WorldPos(-5, -1, 4), new WorldPos(-4, -1, -5), new WorldPos(-4, -1, 5), new WorldPos(4, -1, -5),
           new WorldPos(-5, -2, -4), new WorldPos(-5, -2, 4), new WorldPos(-4, -2, -5), new WorldPos(-4, -2, 5), new WorldPos(4, -2, -5),
           new WorldPos(-5, -3, -4), new WorldPos(-5, -3, 4), new WorldPos(-4, -3, -5), new WorldPos(-4, -3, 5), new WorldPos(4, -3, -5),
           new WorldPos(-5, -4, -4), new WorldPos(-5, -4, 4), new WorldPos(-4, -4, -5), new WorldPos(-4, -4, 5), new WorldPos(4, -4, -5),
           new WorldPos(-5, -5, -4), new WorldPos(-5, -5, 4), new WorldPos(-4, -5, -5), new WorldPos(-4, -5, 5), new WorldPos(4, -5, -5),
           new WorldPos(-5, -6, -4), new WorldPos(-5, -6, 4), new WorldPos(-4, -6, -5), new WorldPos(-4, -6, 5), new WorldPos(4, -6, -5),
           new WorldPos(-5, -7, -4), new WorldPos(-5, -7, 4), new WorldPos(-4, -7, -5), new WorldPos(-4, -7, 5), new WorldPos(4, -7, -5),
           new WorldPos(4, 0, 5), new WorldPos(5, 0, -4), new WorldPos(5, 0, 4), new WorldPos(-6, 0, -3), new WorldPos(-6, 0, 3),
           new WorldPos(4, 1, 5), new WorldPos(5, 1, -4), new WorldPos(5, 1, 4), new WorldPos(-6, 1, -3), new WorldPos(-6, 1, 3),
           new WorldPos(4, 2, 5), new WorldPos(5, 2, -4), new WorldPos(5, 2, 4), new WorldPos(-6, 2, -3), new WorldPos(-6, 2, 3),
           new WorldPos(4, 3, 5), new WorldPos(5, 3, -4), new WorldPos(5, 3, 4), new WorldPos(-6, 3, -3), new WorldPos(-6, 3, 3),
           new WorldPos(4, 4, 5), new WorldPos(5, 4, -4), new WorldPos(5, 4, 4), new WorldPos(-6, 4, -3), new WorldPos(-6, 4, 3),
           new WorldPos(4, 5, 5), new WorldPos(5, 5, -4), new WorldPos(5, 5, 4), new WorldPos(-6, 5, -3), new WorldPos(-6, 5, 3),
           new WorldPos(4, 6, 5), new WorldPos(5, 6, -4), new WorldPos(5, 6, 4), new WorldPos(-6, 6, -3), new WorldPos(-6, 6, 3),
           new WorldPos(4, 7, 5), new WorldPos(5, 7, -4), new WorldPos(5, 7, 4), new WorldPos(-6, 7, -3), new WorldPos(-6, 7, 3),
           new WorldPos(4, -1, 5), new WorldPos(5, -1, -4), new WorldPos(5, -1, 4), new WorldPos(-6, -1, -3), new WorldPos(-6, -1, 3),
           new WorldPos(4, -2, 5), new WorldPos(5, -2, -4), new WorldPos(5, -2, 4), new WorldPos(-6, -2, -3), new WorldPos(-6, -2, 3),
           new WorldPos(4, -3, 5), new WorldPos(5, -3, -4), new WorldPos(5, -3, 4), new WorldPos(-6, -3, -3), new WorldPos(-6, -3, 3),
           new WorldPos(4, -4, 5), new WorldPos(5, -4, -4), new WorldPos(5, -4, 4), new WorldPos(-6, -4, -3), new WorldPos(-6, -4, 3),
           new WorldPos(4, -5, 5), new WorldPos(5, -5, -4), new WorldPos(5, -5, 4), new WorldPos(-6, -5, -3), new WorldPos(-6, -5, 3),
           new WorldPos(4, -6, 5), new WorldPos(5, -6, -4), new WorldPos(5, -6, 4), new WorldPos(-6, -6, -3), new WorldPos(-6, -6, 3),
           new WorldPos(4, -7, 5), new WorldPos(5, -7, -4), new WorldPos(5, -7, 4), new WorldPos(-6, -7, -3), new WorldPos(-6, -7, 3),
           new WorldPos(-3, 0, -6), new WorldPos(-3, 0, 6), new WorldPos(3, 0, -6), new WorldPos(3, 0, 6), new WorldPos(6, 0, -3),
           new WorldPos(-3, 1, -6), new WorldPos(-3, 1, 6), new WorldPos(3, 1, -6), new WorldPos(3, 1, 6), new WorldPos(6, 1, -3),
           new WorldPos(-3, 2, -6), new WorldPos(-3, 2, 6), new WorldPos(3, 2, -6), new WorldPos(3, 2, 6), new WorldPos(6, 2, -3),
           new WorldPos(-3, 3, -6), new WorldPos(-3, 3, 6), new WorldPos(3, 3, -6), new WorldPos(3, 3, 6), new WorldPos(6, 3, -3),
           new WorldPos(-3, 4, -6), new WorldPos(-3, 4, 6), new WorldPos(3, 4, -6), new WorldPos(3, 4, 6), new WorldPos(6, 4, -3),
           new WorldPos(-3, 5, -6), new WorldPos(-3, 5, 6), new WorldPos(3, 5, -6), new WorldPos(3, 5, 6), new WorldPos(6, 5, -3),
           new WorldPos(-3, 6, -6), new WorldPos(-3, 6, 6), new WorldPos(3, 6, -6), new WorldPos(3, 6, 6), new WorldPos(6, 6, -3),
           new WorldPos(-3, 7, -6), new WorldPos(-3, 7, 6), new WorldPos(3, 7, -6), new WorldPos(3, 7, 6), new WorldPos(6, 7, -3),
           new WorldPos(-3, -1, -6), new WorldPos(-3, -1, 6), new WorldPos(3, -1, -6), new WorldPos(3, -1, 6), new WorldPos(6, -1, -3),
           new WorldPos(-3, -2, -6), new WorldPos(-3, -2, 6), new WorldPos(3, -2, -6), new WorldPos(3, -2, 6), new WorldPos(6, -2, -3),
           new WorldPos(-3, -3, -6), new WorldPos(-3, -3, 6), new WorldPos(3, -3, -6), new WorldPos(3, -3, 6), new WorldPos(6, -3, -3),
           new WorldPos(-3, -4, -6), new WorldPos(-3, -4, 6), new WorldPos(3, -4, -6), new WorldPos(3, -4, 6), new WorldPos(6, -4, -3),
           new WorldPos(-3, -5, -6), new WorldPos(-3, -5, 6), new WorldPos(3, -5, -6), new WorldPos(3, -5, 6), new WorldPos(6, -5, -3),
           new WorldPos(-3, -6, -6), new WorldPos(-3, -6, 6), new WorldPos(3, -6, -6), new WorldPos(3, -6, 6), new WorldPos(6, -6, -3),
           new WorldPos(-3, -7, -6), new WorldPos(-3, -7, 6), new WorldPos(3, -7, -6), new WorldPos(3, -7, 6), new WorldPos(6, -7, -3),
           new WorldPos(6, 0, 3), new WorldPos(-7, 0, 0), new WorldPos(0, 0, -7), new WorldPos(0, 0, 7), new WorldPos(7, 0, 0),
           new WorldPos(6, 1, 3), new WorldPos(-7, 1, 0), new WorldPos(0, 1, -7), new WorldPos(0, 1, 7), new WorldPos(7, 1, 0),
           new WorldPos(6, 2, 3), new WorldPos(-7, 2, 0), new WorldPos(0, 2, -7), new WorldPos(0, 2, 7), new WorldPos(7, 2, 0),
           new WorldPos(6, 3, 3), new WorldPos(-7, 3, 0), new WorldPos(0, 3, -7), new WorldPos(0, 3, 7), new WorldPos(7, 3, 0),
           new WorldPos(6, 4, 3), new WorldPos(-7, 4, 0), new WorldPos(0, 4, -7), new WorldPos(0, 4, 7), new WorldPos(7, 4, 0),
           new WorldPos(6, 5, 3), new WorldPos(-7, 5, 0), new WorldPos(0, 5, -7), new WorldPos(0, 5, 7), new WorldPos(7, 5, 0),
           new WorldPos(6, 6, 3), new WorldPos(-7, 6, 0), new WorldPos(0, 6, -7), new WorldPos(0, 6, 7), new WorldPos(7, 6, 0),
           new WorldPos(6, 7, 3), new WorldPos(-7, 7, 0), new WorldPos(0, 7, -7), new WorldPos(0, 7, 7), new WorldPos(7, 7, 0),
           new WorldPos(6, -1, 3), new WorldPos(-7, -1, 0), new WorldPos(0, -1, -7), new WorldPos(0, -1, 7), new WorldPos(7, -1, 0),
           new WorldPos(6, -2, 3), new WorldPos(-7, -2, 0), new WorldPos(0, -2, -7), new WorldPos(0, -2, 7), new WorldPos(7, -2, 0),
           new WorldPos(6, -3, 3), new WorldPos(-7, -3, 0), new WorldPos(0, -3, -7), new WorldPos(0, -3, 7), new WorldPos(7, -3, 0),
           new WorldPos(6, -4, 3), new WorldPos(-7, -4, 0), new WorldPos(0, -4, -7), new WorldPos(0, -4, 7), new WorldPos(7, -4, 0),
           new WorldPos(6, -5, 3), new WorldPos(-7, -5, 0), new WorldPos(0, -5, -7), new WorldPos(0, -5, 7), new WorldPos(7, -5, 0),
           new WorldPos(6, -6, 3), new WorldPos(-7, -6, 0), new WorldPos(0, -6, -7), new WorldPos(0, -6, 7), new WorldPos(7, -6, 0),
           new WorldPos(6, -7, 3), new WorldPos(-7, -7, 0), new WorldPos(0, -7, -7), new WorldPos(0, -7, 7), new WorldPos(7, -7, 0),
           new WorldPos(-7, 0, -1), new WorldPos(-7, 0, 1), new WorldPos(-5, 0, -5), new WorldPos(-5, 0, 5), new WorldPos(-1, 0, -7),
           new WorldPos(-7, 1, -1), new WorldPos(-7, 1, 1), new WorldPos(-5, 1, -5), new WorldPos(-5, 1, 5), new WorldPos(-1, 1, -7),
           new WorldPos(-7, 2, -1), new WorldPos(-7, 2, 1), new WorldPos(-5, 2, -5), new WorldPos(-5, 2, 5), new WorldPos(-1, 2, -7),
           new WorldPos(-7, 3, -1), new WorldPos(-7, 3, 1), new WorldPos(-5, 3, -5), new WorldPos(-5, 3, 5), new WorldPos(-1, 3, -7),
           new WorldPos(-7, 4, -1), new WorldPos(-7, 4, 1), new WorldPos(-5, 4, -5), new WorldPos(-5, 4, 5), new WorldPos(-1, 4, -7),
           new WorldPos(-7, 5, -1), new WorldPos(-7, 5, 1), new WorldPos(-5, 5, -5), new WorldPos(-5, 5, 5), new WorldPos(-1, 5, -7),
           new WorldPos(-7, 6, -1), new WorldPos(-7, 6, 1), new WorldPos(-5, 6, -5), new WorldPos(-5, 6, 5), new WorldPos(-1, 6, -7),
           new WorldPos(-7, 7, -1), new WorldPos(-7, 7, 1), new WorldPos(-5, 7, -5), new WorldPos(-5, 7, 5), new WorldPos(-1, 7, -7),
           new WorldPos(-7, -1, -1), new WorldPos(-7, -1, 1), new WorldPos(-5, -1, -5), new WorldPos(-5, -1, 5), new WorldPos(-1, -1, -7),
           new WorldPos(-7, -2, -1), new WorldPos(-7, -2, 1), new WorldPos(-5, -2, -5), new WorldPos(-5, -2, 5), new WorldPos(-1, -2, -7),
           new WorldPos(-7, -3, -1), new WorldPos(-7, -3, 1), new WorldPos(-5, -3, -5), new WorldPos(-5, -3, 5), new WorldPos(-1, -3, -7),
           new WorldPos(-7, -4, -1), new WorldPos(-7, -4, 1), new WorldPos(-5, -4, -5), new WorldPos(-5, -4, 5), new WorldPos(-1, -4, -7),
           new WorldPos(-7, -5, -1), new WorldPos(-7, -5, 1), new WorldPos(-5, -5, -5), new WorldPos(-5, -5, 5), new WorldPos(-1, -5, -7),
           new WorldPos(-7, -6, -1), new WorldPos(-7, -6, 1), new WorldPos(-5, -6, -5), new WorldPos(-5, -6, 5), new WorldPos(-1, -6, -7),
           new WorldPos(-7, -7, -1), new WorldPos(-7, -7, 1), new WorldPos(-5, -7, -5), new WorldPos(-5, -7, 5), new WorldPos(-1, -7, -7),
           new WorldPos(-1, 0, 7), new WorldPos(1, 0, -7), new WorldPos(1, 0, 7), new WorldPos(5, 0, -5), new WorldPos(5, 0, 5),
           new WorldPos(-1, 1, 7), new WorldPos(1, 1, -7), new WorldPos(1, 1, 7), new WorldPos(5, 1, -5), new WorldPos(5, 1, 5),
           new WorldPos(-1, 2, 7), new WorldPos(1, 2, -7), new WorldPos(1, 2, 7), new WorldPos(5, 2, -5), new WorldPos(5, 2, 5),
           new WorldPos(-1, 3, 7), new WorldPos(1, 3, -7), new WorldPos(1, 3, 7), new WorldPos(5, 3, -5), new WorldPos(5, 3, 5),
           new WorldPos(-1, 4, 7), new WorldPos(1, 4, -7), new WorldPos(1, 4, 7), new WorldPos(5, 4, -5), new WorldPos(5, 4, 5),
           new WorldPos(-1, 5, 7), new WorldPos(1, 5, -7), new WorldPos(1, 5, 7), new WorldPos(5, 5, -5), new WorldPos(5, 5, 5),
           new WorldPos(-1, 6, 7), new WorldPos(1, 6, -7), new WorldPos(1, 6, 7), new WorldPos(5, 6, -5), new WorldPos(5, 6, 5),
           new WorldPos(-1, 7, 7), new WorldPos(1, 7, -7), new WorldPos(1, 7, 7), new WorldPos(5, 7, -5), new WorldPos(5, 7, 5),
           new WorldPos(-1, -1, 7), new WorldPos(1, -1, -7), new WorldPos(1, -1, 7), new WorldPos(5, -1, -5), new WorldPos(5, -1, 5),
           new WorldPos(-1, -2, 7), new WorldPos(1, -2, -7), new WorldPos(1, -2, 7), new WorldPos(5, -2, -5), new WorldPos(5, -2, 5),
           new WorldPos(-1, -3, 7), new WorldPos(1, -3, -7), new WorldPos(1, -3, 7), new WorldPos(5, -3, -5), new WorldPos(5, -3, 5),
           new WorldPos(-1, -4, 7), new WorldPos(1, -4, -7), new WorldPos(1, -4, 7), new WorldPos(5, -4, -5), new WorldPos(5, -4, 5),
           new WorldPos(-1, -5, 7), new WorldPos(1, -5, -7), new WorldPos(1, -5, 7), new WorldPos(5, -5, -5), new WorldPos(5, -5, 5),
           new WorldPos(-1, -6, 7), new WorldPos(1, -6, -7), new WorldPos(1, -6, 7), new WorldPos(5, -6, -5), new WorldPos(5, -6, 5),
           new WorldPos(-1, -7, 7), new WorldPos(1, -7, -7), new WorldPos(1, -7, 7), new WorldPos(5, -7, -5), new WorldPos(5, -7, 5),
           new WorldPos(7, 0, -1), new WorldPos(7, 0, 1), new WorldPos(-6, 0, -4), new WorldPos(-6, 0, 4), new WorldPos(-4, 0, -6),
           new WorldPos(7, 1, -1), new WorldPos(7, 1, 1), new WorldPos(-6, 1, -4), new WorldPos(-6, 1, 4), new WorldPos(-4, 1, -6),
           new WorldPos(7, 2, -1), new WorldPos(7, 2, 1), new WorldPos(-6, 2, -4), new WorldPos(-6, 2, 4), new WorldPos(-4, 2, -6),
           new WorldPos(7, 3, -1), new WorldPos(7, 3, 1), new WorldPos(-6, 3, -4), new WorldPos(-6, 3, 4), new WorldPos(-4, 3, -6),
           new WorldPos(7, 4, -1), new WorldPos(7, 4, 1), new WorldPos(-6, 4, -4), new WorldPos(-6, 4, 4), new WorldPos(-4, 4, -6),
           new WorldPos(7, 5, -1), new WorldPos(7, 5, 1), new WorldPos(-6, 5, -4), new WorldPos(-6, 5, 4), new WorldPos(-4, 5, -6),
           new WorldPos(7, 6, -1), new WorldPos(7, 6, 1), new WorldPos(-6, 6, -4), new WorldPos(-6, 6, 4), new WorldPos(-4, 6, -6),
           new WorldPos(7, 7, -1), new WorldPos(7, 7, 1), new WorldPos(-6, 7, -4), new WorldPos(-6, 7, 4), new WorldPos(-4, 7, -6),
           new WorldPos(7, -1, -1), new WorldPos(7, -1, 1), new WorldPos(-6, -1, -4), new WorldPos(-6, -1, 4), new WorldPos(-4, -1, -6),
           new WorldPos(7, -2, -1), new WorldPos(7, -2, 1), new WorldPos(-6, -2, -4), new WorldPos(-6, -2, 4), new WorldPos(-4, -2, -6),
           new WorldPos(7, -3, -1), new WorldPos(7, -3, 1), new WorldPos(-6, -3, -4), new WorldPos(-6, -3, 4), new WorldPos(-4, -3, -6),
           new WorldPos(7, -4, -1), new WorldPos(7, -4, 1), new WorldPos(-6, -4, -4), new WorldPos(-6, -4, 4), new WorldPos(-4, -4, -6),
           new WorldPos(7, -5, -1), new WorldPos(7, -5, 1), new WorldPos(-6, -5, -4), new WorldPos(-6, -5, 4), new WorldPos(-4, -5, -6),
           new WorldPos(7, -6, -1), new WorldPos(7, -6, 1), new WorldPos(-6, -6, -4), new WorldPos(-6, -6, 4), new WorldPos(-4, -6, -6),
           new WorldPos(7, -7, -1), new WorldPos(7, -7, 1), new WorldPos(-6, -7, -4), new WorldPos(-6, -7, 4), new WorldPos(-4, -7, -6),
           new WorldPos(-4, 0, 6), new WorldPos(4, 0, -6), new WorldPos(4, 0, 6), new WorldPos(6, 0, -4), new WorldPos(6, 0, 4),
           new WorldPos(-4, 1, 6), new WorldPos(4, 1,-6), new WorldPos(4, 1, 6), new WorldPos(6, 1, -4), new WorldPos(6, 1, 4),
           new WorldPos(-4, 2, 6), new WorldPos(4, 2, -6), new WorldPos(4, 2, 6), new WorldPos(6, 2, -4), new WorldPos(6, 2, 4),
           new WorldPos(-4, 3, 6), new WorldPos(4, 3, -6), new WorldPos(4, 3, 6), new WorldPos(6, 3, -4), new WorldPos(6, 3, 4),
           new WorldPos(-4, 4, 6), new WorldPos(4, 4, -6), new WorldPos(4, 4, 6), new WorldPos(6, 4, -4), new WorldPos(6, 4, 4),
           new WorldPos(-4, 5, 6), new WorldPos(4, 5, -6), new WorldPos(4, 5, 6), new WorldPos(6, 5, -4), new WorldPos(6, 5, 4),
           new WorldPos(-4, 6, 6), new WorldPos(4, 6, -6), new WorldPos(4, 6, 6), new WorldPos(6, 6, -4), new WorldPos(6, 6, 4),
           new WorldPos(-4, 7, 6), new WorldPos(4, 7, -6), new WorldPos(4, 7, 6), new WorldPos(6, 7, -4), new WorldPos(6, 7, 4),
           new WorldPos(-4, -1, 6), new WorldPos(4, -1,-6), new WorldPos(4, -1, 6), new WorldPos(6, -1, -4), new WorldPos(6, -1, 4),
           new WorldPos(-4, -2, 6), new WorldPos(4, -2, -6), new WorldPos(4,-2, 6), new WorldPos(6, -2, -4), new WorldPos(6, -2, 4),
           new WorldPos(-4, -3, 6), new WorldPos(4, -3, -6), new WorldPos(4,-3, 6), new WorldPos(6, -3, -4), new WorldPos(6, -3, 4),
           new WorldPos(-4, -4, 6), new WorldPos(4, -4, -6), new WorldPos(4,-4, 6), new WorldPos(6, -4, -4), new WorldPos(6, -4, 4),
           new WorldPos(-4, -5, 6), new WorldPos(4, -5, -6), new WorldPos(4,-5, 6), new WorldPos(6, -5, -4), new WorldPos(6, -5, 4),
           new WorldPos(-4, -6, 6), new WorldPos(4, -6, -6), new WorldPos(4,-6, 6), new WorldPos(6, -6, -4), new WorldPos(6, -6, 4),
           new WorldPos(-4, -7, 6), new WorldPos(4, -7, -6), new WorldPos(4,-7, 6), new WorldPos(6, -7, -4), new WorldPos(6, -7, 4),
           new WorldPos(-7, 0, -2), new WorldPos(-7, 0, 2), new WorldPos(-2, 0, -7), new WorldPos(-2, 0, 7),
           new WorldPos(-7, 1, -2), new WorldPos(-7, 1, 2), new WorldPos(-2, 1, -7), new WorldPos(-2, 1, 7),
           new WorldPos(-7, 2, -2), new WorldPos(-7, 2, 2), new WorldPos(-2, 2, -7), new WorldPos(-2, 2, 7),
           new WorldPos(-7, 3, -2), new WorldPos(-7, 3, 2), new WorldPos(-2, 3, -7), new WorldPos(-2, 3, 7),
           new WorldPos(-7, 4, -2), new WorldPos(-7, 4, 2), new WorldPos(-2, 4, -7), new WorldPos(-2, 4, 7),
           new WorldPos(-7, 5, -2), new WorldPos(-7, 5, 2), new WorldPos(-2, 5, -7), new WorldPos(-2, 5, 7),
           new WorldPos(-7, 6, -2), new WorldPos(-7, 6, 2), new WorldPos(-2, 6, -7), new WorldPos(-2, 6, 7),
           new WorldPos(-7, 7, -2), new WorldPos(-7, 7, 2), new WorldPos(-2, 7, -7), new WorldPos(-2, 7, 7),
           new WorldPos(-7, -1, -2), new WorldPos(-7, -1, 2), new WorldPos(-2, -1, -7), new WorldPos(-2, -1, 7),
           new WorldPos(-7, -2, -2), new WorldPos(-7, -2, 2), new WorldPos(-2, -2, -7), new WorldPos(-2, -2, 7),
           new WorldPos(-7, -3, -2), new WorldPos(-7, -3, 2), new WorldPos(-2, -3, -7), new WorldPos(-2, -3, 7),
           new WorldPos(-7, -4, -2), new WorldPos(-7, -4, 2), new WorldPos(-2, -4, -7), new WorldPos(-2, -4, 7),
           new WorldPos(-7, -5, -2), new WorldPos(-7, -5, 2), new WorldPos(-2, -5, -7), new WorldPos(-2, -5, 7),
           new WorldPos(-7, -6, -2), new WorldPos(-7, -6, 2), new WorldPos(-2, -6, -7), new WorldPos(-2, -6, 7),
           new WorldPos(-7, -7, -2), new WorldPos(-7, -7, 2), new WorldPos(-2, -7, -7), new WorldPos(-2, -7, 7),
           new WorldPos(2, 0, -7), new WorldPos(2, 0, 7), new WorldPos(7, 0, -2), new WorldPos(7, 0, 2), new WorldPos(-7, 0, -3),
           new WorldPos(2, 1, -7), new WorldPos(2, 1, 7), new WorldPos(7, 1, -2), new WorldPos(7, 1, 2), new WorldPos(-7, 1, -3),
           new WorldPos(2, 2, -7), new WorldPos(2, 2, 7), new WorldPos(7, 2, -2), new WorldPos(7, 2, 2), new WorldPos(-7, 2, -3),
           new WorldPos(2, 3, -7), new WorldPos(2, 3, 7), new WorldPos(7, 3, -2), new WorldPos(7, 3, 2), new WorldPos(-7, 3, -3),
           new WorldPos(2, 4, -7), new WorldPos(2, 4, 7), new WorldPos(7, 4, -2), new WorldPos(7, 4, 2), new WorldPos(-7, 4, -3),
           new WorldPos(2, 5, -7), new WorldPos(2, 5, 7), new WorldPos(7, 5, -2), new WorldPos(7, 5, 2), new WorldPos(-7, 5, -3),
           new WorldPos(2, 6, -7), new WorldPos(2, 6, 7), new WorldPos(7, 6, -2), new WorldPos(7, 6, 2), new WorldPos(-7, 6, -3),
           new WorldPos(2, 7, -7), new WorldPos(2, 7, 7), new WorldPos(7, 7, -2), new WorldPos(7, 7, 2), new WorldPos(-7, 7, -3),
           new WorldPos(2, -1, -7), new WorldPos(2, -1, 7), new WorldPos(7, -1, -2), new WorldPos(7, -1, 2), new WorldPos(-7, -1, -3),
           new WorldPos(2, -2, -7), new WorldPos(2, -2, 7), new WorldPos(7, -2, -2), new WorldPos(7, -2, 2), new WorldPos(-7, -2, -3),
           new WorldPos(2, -3, -7), new WorldPos(2, -3, 7), new WorldPos(7, -3, -2), new WorldPos(7, -3, 2), new WorldPos(-7, -3, -3),
           new WorldPos(2, -4, -7), new WorldPos(2, -4, 7), new WorldPos(7, -4, -2), new WorldPos(7, -4, 2), new WorldPos(-7, -4, -3),
           new WorldPos(2, -5, -7), new WorldPos(2, -5, 7), new WorldPos(7, -5, -2), new WorldPos(7, -5, 2), new WorldPos(-7, -5, -3),
           new WorldPos(2, -6, -7), new WorldPos(2, -6, 7), new WorldPos(7, -6, -2), new WorldPos(7, -6, 2), new WorldPos(-7, -6, -3),
           new WorldPos(2, -7, -7), new WorldPos(2, -7, 7), new WorldPos(7, -7, -2), new WorldPos(7, -7, 2), new WorldPos(-7, -7, -3),
           new WorldPos(-7, 0, 3), new WorldPos(-3, 0, -7), new WorldPos(-3, 0, 7), new WorldPos(3, 0, -7), new WorldPos(3, 0, 7),
           new WorldPos(-7, 1, 3), new WorldPos(-3, 1, -7), new WorldPos(-3, 1, 7), new WorldPos(3, 1, -7), new WorldPos(3, 1, 7),
           new WorldPos(-7, 2, 3), new WorldPos(-3, 2, -7), new WorldPos(-3, 2, 7), new WorldPos(3, 2, -7), new WorldPos(3, 2, 7),
           new WorldPos(-7, 3, 3), new WorldPos(-3, 3, -7), new WorldPos(-3, 3, 7), new WorldPos(3, 3, -7), new WorldPos(3, 3, 7),
           new WorldPos(-7, 4, 3), new WorldPos(-3, 4, -7), new WorldPos(-3, 4, 7), new WorldPos(3, 4, -7), new WorldPos(3, 4, 7),
           new WorldPos(-7, 5, 3), new WorldPos(-3, 5, -7), new WorldPos(-3, 5, 7), new WorldPos(3, 5, -7), new WorldPos(3, 5, 7),
           new WorldPos(-7, 6, 3), new WorldPos(-3, 6, -7), new WorldPos(-3, 6, 7), new WorldPos(3, 6, -7), new WorldPos(3, 6, 7),
           new WorldPos(-7, 7, 3), new WorldPos(-3, 7, -7), new WorldPos(-3, 7, 7), new WorldPos(3, 7, -7), new WorldPos(3, 7, 7),
           new WorldPos(-7, -1, 3), new WorldPos(-3, -1, -7), new WorldPos(-3, -1, 7), new WorldPos(3, -1, -7), new WorldPos(3, -1, 7),
           new WorldPos(-7, -2, 3), new WorldPos(-3, -2, -7), new WorldPos(-3, -2, 7), new WorldPos(3, -2, -7), new WorldPos(3, -2, 7),
           new WorldPos(-7, -3, 3), new WorldPos(-3, -3, -7), new WorldPos(-3, -3, 7), new WorldPos(3, -3, -7), new WorldPos(3, -3, 7),
           new WorldPos(-7, -4, 3), new WorldPos(-3, -4, -7), new WorldPos(-3, -4, 7), new WorldPos(3, -4, -7), new WorldPos(3, -4, 7),
           new WorldPos(-7, -5, 3), new WorldPos(-3, -5, -7), new WorldPos(-3, -5, 7), new WorldPos(3, -5, -7), new WorldPos(3, -5, 7),
           new WorldPos(-7, -6, 3), new WorldPos(-3, -6, -7), new WorldPos(-3, -6, 7), new WorldPos(3, -6, -7), new WorldPos(3, -6, 7),
           new WorldPos(-7, -7, 3), new WorldPos(-3, -7, -7), new WorldPos(-3, -7, 7), new WorldPos(3, -7, -7), new WorldPos(3, -7, 7),
           new WorldPos(7, 0, -3), new WorldPos(7, 0, 3), new WorldPos(-6, 0, -5), new WorldPos(-6, 0, 5), new WorldPos(-5, 0, -6),
           new WorldPos(7, 1, -3), new WorldPos(7, 1, 3), new WorldPos(-6, 1, -5), new WorldPos(-6, 1, 5), new WorldPos(-5, 1, -6),
           new WorldPos(7, 2, -3), new WorldPos(7, 2, 3), new WorldPos(-6, 2, -5), new WorldPos(-6, 2, 5), new WorldPos(-5, 2, -6),
           new WorldPos(7, 3, -3), new WorldPos(7, 3, 3), new WorldPos(-6, 3, -5), new WorldPos(-6, 3, 5), new WorldPos(-5, 3, -6),
           new WorldPos(7, 4, -3), new WorldPos(7, 4, 3), new WorldPos(-6, 4, -5), new WorldPos(-6, 4, 5), new WorldPos(-5, 4, -6),
           new WorldPos(7, 5, -3), new WorldPos(7, 5, 3), new WorldPos(-6, 5, -5), new WorldPos(-6, 5, 5), new WorldPos(-5, 5, -6),
           new WorldPos(7, 6, -3), new WorldPos(7, 6, 3), new WorldPos(-6, 6, -5), new WorldPos(-6, 6, 5), new WorldPos(-5, 6, -6),
           new WorldPos(7, 7, -3), new WorldPos(7, 7, 3), new WorldPos(-6, 7, -5), new WorldPos(-6, 7, 5), new WorldPos(-5, 7, -6),
           new WorldPos(7, -1, -3), new WorldPos(7, -1, 3), new WorldPos(-6, -1, -5), new WorldPos(-6, -1, 5), new WorldPos(-5, -1, -6),
           new WorldPos(7, -2, -3), new WorldPos(7, -2, 3), new WorldPos(-6, -2, -5), new WorldPos(-6, -2, 5), new WorldPos(-5, -2, -6),
           new WorldPos(7, -3, -3), new WorldPos(7, -3, 3), new WorldPos(-6, -3, -5), new WorldPos(-6, -3, 5), new WorldPos(-5, -3, -6),
           new WorldPos(7, -4, -3), new WorldPos(7, -4, 3), new WorldPos(-6, -4, -5), new WorldPos(-6, -4, 5), new WorldPos(-5, -4, -6),
           new WorldPos(7, -5, -3), new WorldPos(7, -5, 3), new WorldPos(-6, -5, -5), new WorldPos(-6, -5, 5), new WorldPos(-5, -5, -6),
           new WorldPos(7, -6, -3), new WorldPos(7, -6, 3), new WorldPos(-6, -6, -5), new WorldPos(-6, -6, 5), new WorldPos(-5, -6, -6),
           new WorldPos(7, -7, -3), new WorldPos(7, -7, 3), new WorldPos(-6, -7, -5), new WorldPos(-6, -7, 5), new WorldPos(-5, -7, -6),
           new WorldPos(-5, 0, 6), new WorldPos(5, 0, -6), new WorldPos(5, 0, 6), new WorldPos(6, 0, -5), new WorldPos(6, 0, 5),
           new WorldPos(-5, 1, 6), new WorldPos(5, 1, -6), new WorldPos(5, 1, 6), new WorldPos(6, 1, -5), new WorldPos(6, 1, 5),
           new WorldPos(-5, 2, 6), new WorldPos(5, 2, -6), new WorldPos(5, 2, 6), new WorldPos(6, 2, -5), new WorldPos(6, 2, 5),
           new WorldPos(-5, 3, 6), new WorldPos(5, 3, -6), new WorldPos(5, 3, 6), new WorldPos(6, 3, -5), new WorldPos(6, 3, 5),
           new WorldPos(-5, 4, 6), new WorldPos(5, 4, -6), new WorldPos(5, 4, 6), new WorldPos(6, 4, -5), new WorldPos(6, 4, 5),
           new WorldPos(-5, 5, 6), new WorldPos(5, 5, -6), new WorldPos(5, 5, 6), new WorldPos(6, 5, -5), new WorldPos(6, 5, 5),
           new WorldPos(-5, 6, 6), new WorldPos(5, 6, -6), new WorldPos(5, 6, 6), new WorldPos(6, 6, -5), new WorldPos(6, 6, 5),
           new WorldPos(-5, 7, 6), new WorldPos(5, 7, -6), new WorldPos(5, 7, 6), new WorldPos(6, 7, -5), new WorldPos(6, 7, 5),
           new WorldPos(-5, -1, 6), new WorldPos(5, -1, -6), new WorldPos(5, -1, 6), new WorldPos(6, -1, -5), new WorldPos(6, -1, 5),
           new WorldPos(-5, -2, 6), new WorldPos(5, -2, -6), new WorldPos(5, -2, 6), new WorldPos(6, -2, -5), new WorldPos(6, -2, 5),
           new WorldPos(-5, -3, 6), new WorldPos(5, -3, -6), new WorldPos(5, -3, 6), new WorldPos(6, -3, -5), new WorldPos(6, -3, 5),
           new WorldPos(-5, -4, 6), new WorldPos(5, -4, -6), new WorldPos(5, -4, 6), new WorldPos(6, -4, -5), new WorldPos(6, -4, 5),
           new WorldPos(-5, -5, 6), new WorldPos(5, -5, -6), new WorldPos(5, -5, 6), new WorldPos(6, -5, -5), new WorldPos(6, -5, 5),
           new WorldPos(-5, -6, 6), new WorldPos(5, -6, -6), new WorldPos(5, -6, 6), new WorldPos(6, -6, -5), new WorldPos(6, -6, 5),
           new WorldPos(-5, -7, 6), new WorldPos(5, -7, -6), new WorldPos(5, -7, 6), new WorldPos(6, -7, -5), new WorldPos(6, -7, 5)
       };
        
        //private void Awake()
        //{
        //    CreatChunkPositions();
        //}

        //private void CreatChunkPositions()
        //{
        //    chunkPositions = new WorldPos[(int)Mathf.Pow(perimeter * 2, 3)];
        //    
        //    int i = 0;
        //    for (int x = -perimeter; x < perimeter; x++)
        //    {
        //        for (int y = -perimeter; y < perimeter; y++)
        //        {
        //            for (int z = -perimeter; z < perimeter; z++)
        //            {
        //                chunkPositions[i] = new WorldPos(x, y, z);
        //                print("x: " + chunkPositions[i].x + " y: " + chunkPositions[i].y + " z: " +
        //                      chunkPositions[i].z);
        //                i++;
        //            }
        //        }
        //    }
        //}

        void FindChunksToLoad()
        {
            // Get the position of this gameobject to generate around
            WorldPos playerPos = new WorldPos(
                Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
                Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize);

            // If there aren't already chunks to generate
            if (updateList.Count == 0)
            {
                // Cycle through the array of positions
                for (int i = 0; i < chunkPositions.Length; i++)
                {
                    // Translate the player position and array position into chunk position
                    WorldPos newChunkPos = new WorldPos(chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                        chunkPositions[i].y * Chunk.chunkSize + playerPos.y,
                        chunkPositions[i].z * Chunk.chunkSize + playerPos.z);
                    // Get the chunk in the defined position
                    Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);
                    // If the chunk already exists and it's already
                    // rendered or in queue to be rendered continue
                    if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    {
                        continue;
                    }

                    // Load a column of chunks in this position
                    for (int x = newChunkPos.x - Chunk.chunkSize;
                        x <= newChunkPos.x + Chunk.chunkSize;
                        x += Chunk.chunkSize)
                    {
                        for (int y = newChunkPos.y - Chunk.chunkSize;
                            y <= newChunkPos.y + Chunk.chunkSize;
                            y += Chunk.chunkSize)
                        {
                            for (int z = newChunkPos.z - Chunk.chunkSize;
                                z <= newChunkPos.z + Chunk.chunkSize;
                                z += Chunk.chunkSize)
                            {
                                buildList.Add(new WorldPos(x, y, z));
                            }
                        }

                        updateList.Add(new WorldPos(newChunkPos.x, newChunkPos.y, newChunkPos.z));
                    }

                    return;
                }
            }
        }

        void BuildChunk(WorldPos pos)
        {
            if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            {
                world.CreateChunk(pos.x, pos.y, pos.z);
            }
        }

        void LoadAndRenderChunks()
        {
            if (buildList.Count != 0)
            {
                for (int i = 0; i < buildList.Count && i < 8; i++)
                {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }

                // If chunks were built return early
                return;
            }

            if (updateList.Count != 0)
            {
                Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk != null)
                {
                    chunk.update = true;
                }

                updateList.RemoveAt(0);
            }
        }

        bool DeleteChunks()
        {
            if (timer == 10)
            {
                var chunksToDelete = new List<WorldPos>();
                foreach (var chunk in world.chunks)
                {
                    float distance = Vector3.Distance(
                        new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, chunk.Value.pos.z),
                        new Vector3(transform.position.x, transform.position.y, transform.position.z));
                    if (distance > 256)
                    {
                        chunksToDelete.Add(chunk.Key);
                    }
                }

                foreach (var chunk in chunksToDelete)
                {
                    world.DestroyChunk(chunk.x, chunk.y, chunk.z);
                }

                timer = 0;
                return true;
            }

            timer++;
            return false;
        }

        private void Update()
        {
            if (DeleteChunks())
            {
                return;
            }

            FindChunksToLoad();
            LoadAndRenderChunks();
        }
    }
}