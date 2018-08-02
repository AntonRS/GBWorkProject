using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    public class FinalLabyrinthData
    {

        public int[][] Squares;
        public int[][] EndSquares;

        public FinalLabyrinthData(int[][] squaresCoordinatesArray, int[][] endSquaresCoordinatesArray)
        {
            Squares = squaresCoordinatesArray;
            EndSquares = endSquaresCoordinatesArray;
        }

    }

}