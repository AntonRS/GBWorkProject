using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    public static class LabyrinthCommonMethodsAndExtensions
    {

        /// <summary>
        /// Перекидывает содержимое списка координат в массив.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int[][] ListOfCoordinatesToArray(this List<int[]> list)
        {            
            int[][] arrayOfCoordinates = new int[list.Count][];
            for (int i = 0; i < arrayOfCoordinates.Length; i++)
                arrayOfCoordinates[i] = list[i];

            return arrayOfCoordinates;
        }
    }


}