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


        //Метод для проверки, есть ли данные координаты в списке. Обычный List.Contains в данном случае не работает
        //, потому что int[] - ссылочный тип, и проверяется не значение, а ссылка.
        //Если ссылка не совпадает, обычный List.Contains вернёт false, даже если значения одинаковы.
        public static bool ListContainsCoordinates(this List<int[]> list, int[] squareXY)
        {
            if (list == null || squareXY == null)
                return false;

            foreach (var item in list)
            {
                if (item[0] == squareXY[0] && item[1] == squareXY[1])
                    return true;
            }

            return false;
        }
    }


}