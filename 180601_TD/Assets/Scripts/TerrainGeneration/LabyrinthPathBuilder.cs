using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Extensions;

namespace Game.TerrainGeneration
{
    public class LabyrinthPathBuilder
    {

        public int[][] BuildPathInLabyrinth(int pathLength, int[][] ArrayOfLabyrinthSquaresCoordinatesXY, int[][] ArrayOfPotentialPathStartsAndFinishes)
        {

            FinalLabyrinthData labyrinth = new FinalLabyrinthData(ArrayOfLabyrinthSquaresCoordinatesXY, ArrayOfPotentialPathStartsAndFinishes);

            List<int[]> path = new List<int[]>();

            labyrinth.EndSquares = labyrinth.EndSquares.RandomizeArray();

            foreach (var startSquare in labyrinth.EndSquares) //тут в принципе можно и общий squares поставить
            {
                foreach (var finishSquare in labyrinth.EndSquares)
                {
                    //строим кратчайший путь от стартовой клетки до конечной
                    path = BuildShortestPath(startSquare, finishSquare, labyrinth);

                    //если построенный путь достиг нужной длины, это и будет наша дорога.
                    if (path != null && path.Count >= pathLength)
                    {
                        //убираем лишнее, если путь слишком длинный
                        while (path.Count > pathLength)
                            path.Remove(path[path.Count - 1]);

                        //перекидываем список в массив и возвращаем его
                        return path.ListOfCoordinatesToArray();
                    }
                }
            }


            return null;
        }

        /// <summary>
        /// Поиск кратчайшего пути от стартовой точки до конечной, волновым методом.
        /// </summary>
        /// <param name="startSquareXY"></param>
        /// <param name="finishSquareXY"></param>
        /// <param name="labyrinth"></param>
        /// <returns></returns>
        private List<int[]> BuildShortestPath (int[] startSquareXY, int[] finishSquareXY, FinalLabyrinthData labyrinth)
        {
            if (startSquareXY == finishSquareXY)
                return null;

            Debug.Log("Пробуем построить путь от " + startSquareXY[0] + "," + startSquareXY[1] + " до " + finishSquareXY[0] + "," + finishSquareXY[1]);


            Dictionary<int[], int> squareValues = new Dictionary<int[], int>();

            //начинаем с финишной клетки, потому что путь строится по оставленным волной значениям в обратную сторону.
            squareValues[finishSquareXY] = 0;
            int currentValue = 0;

            //волна до целевой клетки
            while (currentValue < labyrinth.Squares.Length)
            {
                Debug.Log("Текущее значение равно " + currentValue);
                Debug.Log("Количество значений уже " + squareValues.Count);
                foreach (var square in labyrinth.Squares)
                {

                    if (ValuesDictionaryContainsCoordinates(squareValues, square) && ValuesDictionaryGetValueByCoordinates(squareValues, square) == currentValue)
                    {
                        Debug.Log("Пробуем двигать волну от клетки " + square[0] + "," + square[1] + " со значением " + ValuesDictionaryGetValueByCoordinates(squareValues, square));
                        //попытка сделать шаг волны в соседние клетки
                        for (int i = -1; i < 2; i = i + 2) //таким образом получаю i равное -1 и i равное +1, и, вызывая два раза метод ниже, пробегаюсь по четырем соседним клеткам.
                        {
                            TryAdvanceWaveToSquare(new int[] { square[0] + i, square[1] }, labyrinth, squareValues, currentValue);
                            TryAdvanceWaveToSquare(new int[] { square[0], square[1] + i }, labyrinth, squareValues, currentValue);
                        }
                    }
                }


                currentValue++;

                //если волна докатилась до целевой клетки, выходим из цикла
                if (ValuesDictionaryContainsCoordinates(squareValues, startSquareXY))
                    break;

                //ВОЗМОЖНО, СТОИТ ДОБАВИТЬ КАКОЕ-НИБУДЬ ОГРАНИЧЕНИЕ С ОШИБКОЙ, ЧТОБЫ НЕ БЫЛО БЕСКОНЕЧНОГО ЦИКЛА ПРИ НЕПРАВИЛЬНО ЗАДАННОМ ЛАБИРИНТЕ
                //а так волна не может не докатиться до нужной клетки, потому что изолированных клеток в моём лабиринте нет

                            
                
            }

            Debug.Log("Волна докатилась до искомой клетки, теперь ищем путь обратно");

            //построение пути обратно по оставленным волной значениям. С некоторой рандомизацией, для увеличения количества поворотов.

            List<int[]> pathSquaresList = new List<int[]>
            {
                startSquareXY
            };

            int[] currentPathSquare = startSquareXY;

            int[] squareCheckingDirectionOrder = new int[] { 0, 1, 2, 3 };
            bool nextSquareFound;

            for (int i = currentValue; i > 0; i--)
            {

                Debug.Log("Текущее значение равно " + i);

                Debug.Log("Мы находимся в клетке " + currentPathSquare[0] + "," + currentPathSquare[1]);

                nextSquareFound = false;
                squareCheckingDirectionOrder = squareCheckingDirectionOrder.RandomizeArray();
                for (int j = 0; j < squareCheckingDirectionOrder.Length; j++)
                {
                    switch (squareCheckingDirectionOrder[j])
                    {
                        case (0):
                            nextSquareFound = TryBacktrackWaveToSquare(ref currentPathSquare, new int[] { currentPathSquare[0] - 1, currentPathSquare[1] }, squareValues, i, pathSquaresList);
                            break;

                        case (1):
                            nextSquareFound = TryBacktrackWaveToSquare(ref currentPathSquare, new int[] { currentPathSquare[0] + 1, currentPathSquare[1] }, squareValues, i, pathSquaresList);
                            break;

                        case (2):
                            nextSquareFound = TryBacktrackWaveToSquare(ref currentPathSquare, new int[] { currentPathSquare[0], currentPathSquare[1] - 1 }, squareValues, i, pathSquaresList);
                            break;

                        case (3):
                            nextSquareFound = TryBacktrackWaveToSquare(ref currentPathSquare, new int[] { currentPathSquare[0], currentPathSquare[1] + 1 }, squareValues, i, pathSquaresList);
                            break;

                        default: break;
                    }

                    if (nextSquareFound)
                        break;
                }

            }

            Debug.Log("Итого путь состоит из следующих клеток:");
            foreach (var square in pathSquaresList)
                Debug.Log(square[0] + "," + square[1]);

            return pathSquaresList;
        }

        private void TryAdvanceWaveToSquare(int[] targetSquareXY, FinalLabyrinthData labyrinth, Dictionary<int[], int> squareValues, int currentValue)
        {
            Debug.Log("заглянули в клетку " + targetSquareXY[0] + "," + targetSquareXY[1]);

            foreach (int[] square in labyrinth.Squares)
                if (CoordinatesAreEqual(square, targetSquareXY) && !ValuesDictionaryContainsCoordinates(squareValues, targetSquareXY))//оператор '==' и Array.Equals сравнивают ссылки, поэтому использую свои методы сравнения
                {
                    squareValues[targetSquareXY] = currentValue + 1;
                    Debug.Log("клетке с координатами " + targetSquareXY[0] + "," + targetSquareXY[1] + " присвоили значение " + (currentValue + 1));
                    break;
                }
        }

        private bool TryBacktrackWaveToSquare(ref int[] currentPathSquare, int[] targetSquareXY, Dictionary<int[], int> squareValues, int currentValue, List<int[]> pathSquaresList)
        {
            Debug.Log("заглянули в клетку " + targetSquareXY[0] + "," + targetSquareXY[1]);

            if (ValuesDictionaryContainsCoordinates(squareValues, targetSquareXY) &&  ValuesDictionaryGetValueByCoordinates(squareValues, targetSquareXY) == currentValue - 1)
            {
                currentPathSquare = targetSquareXY;
                pathSquaresList.Add(targetSquareXY);

                Debug.Log("у неё значение " + (currentValue - 1) + " и это то, что нам нужно");
                Debug.Log("Теперь мы должны перейти в клетку " + currentPathSquare[0] + "," + currentPathSquare[1]);

                return true;
            }

            return false;            
        }     

        private bool CoordinatesAreEqual(int[] squareACoordinatesXY, int[] squareBCoordinatesXY)
        {
            if (squareACoordinatesXY == null || squareBCoordinatesXY == null)
                return false;

            return squareACoordinatesXY[0] == squareBCoordinatesXY[0] && squareACoordinatesXY[1] == squareBCoordinatesXY[1];
        }
        
        private bool ValuesDictionaryContainsCoordinates(Dictionary<int[], int> dictionary, int[] squareXY)
        {
            if (dictionary == null || squareXY == null)
                return false;

            foreach (var key in dictionary.Keys)
                if (key[0] == squareXY[0] && key[1] == squareXY[1])
                    return true;

            return false;
        }

        private int ValuesDictionaryGetValueByCoordinates(Dictionary<int[], int> dictionary, int[] squareXY)
        {
            if (dictionary == null || squareXY == null)
                throw new Exception("One or both of the supplied parameters are null.");

            foreach (var key in dictionary.Keys)
                if (key[0] == squareXY[0] && key[1] == squareXY[1])
                    return dictionary[key];

            throw new KeyNotFoundException("В словаре нет такого ключа.");
        }

    }

}