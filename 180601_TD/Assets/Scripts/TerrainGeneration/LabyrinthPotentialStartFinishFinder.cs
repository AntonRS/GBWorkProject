using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{
    /// <summary>
    /// Класс для поиска потенциальных клеток начала и конца пути в лабиринте.
    /// </summary>
    public class LabyrinthPotentialStartFinishFinder
    {

        /// <summary>
        /// Принимает массив клеток лабиринта. Возвращает потенциальные начальные/конечные клетки для последующего алгоритма поиска пути. В качестве таковых клеток определяет те, которые находятся по краям лабиринта, а также те, которые соседствуют только с одной клеткой каждая.
        /// </summary>
        /// <param name="labyrinthSquares"></param>
        /// <returns></returns>
        public int[][] GetPotentialStartAndFinissSquares(int[][] labyrinthSquares)
        {
            List<int[]> potentialStartFinishSquares = new List<int[]>();

            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;

            //ищем края лабиринта
            foreach (var square in labyrinthSquares)
            {
                if (square[0] < minX)
                    minX = square[0];
                else if (square[0] > maxX)
                    maxX = square[0];

                if (square[1] < minY)
                    minY = square[1];
                else if (square[1] > maxY)
                    maxY = square[1];
            }

            //ищем нужные клетки
            foreach (var square in labyrinthSquares)
            {
                if (square[0] == minX || square[0] == maxX || square[1] == minY || square[1] == maxY)
                    potentialStartFinishSquares.Add(square);
                else
                {
                    int adjacentSquaresFound = 0;

                    for (int i = -1; i < 2; i = i + 2) //таким образом получаю i равное -1 и i равное +1, и, вызывая два раза метод ниже, пробегаюсь по четырем соседним клеткам.
                    {
                        if (ArrayContainsCoordinates(labyrinthSquares, new int[] { square[0] + i, square[1] }))
                            adjacentSquaresFound++;

                        if (ArrayContainsCoordinates(labyrinthSquares, new int[] { square[0], square[1] + i }))
                            adjacentSquaresFound++;
                    }

                    if (adjacentSquaresFound == 1)
                    {
                        potentialStartFinishSquares.Add(square);
                        Debug.Log("В потенциальные клетки концоначала добавили клетку " + square[0] + "," + square[1]);
                    }
                }
            }

            return potentialStartFinishSquares.ListOfCoordinatesToArray();

        }


        private bool ArrayContainsCoordinates(int[][] array, int[] squareXYtoFind)
        {
            if (array == null || squareXYtoFind == null)
                return false;

            foreach (var square in array)
                if (square[0] == squareXYtoFind[0] && square[1] == squareXYtoFind[1])
                    return true;

            return false;
        }


    }

}
