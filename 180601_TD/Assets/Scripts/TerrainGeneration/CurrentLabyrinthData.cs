using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    public class CurrentLabyrinthData
    {

        public List<int[]> Squares = new List<int[]>();
        public List<int[]> EndSquares = new List<int[]>();

        public int MinX;
        public int MaxX;
        public int MinY;
        public int MaxY;

        public bool LeftBorderReached;
        public bool RightBorderReached;
        public bool TopBorderReached;
        public bool BottomBorderReached;

        public int CurrentCorridorsCount;
        public int MinTotalCorridorsCount;

        public bool AllFourBordersAreReached()
        {
            return (LeftBorderReached && RightBorderReached && TopBorderReached && BottomBorderReached);
        }

    }
}