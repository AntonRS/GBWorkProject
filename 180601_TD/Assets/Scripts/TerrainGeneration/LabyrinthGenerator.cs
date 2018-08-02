using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    /// <summary>
    /// Модуль для создания квадратного лабиринта. На вход принимает длину стороны лабиринта. Возвращает координаты всех вошедших в сгенерированные коридоры клеток.
    /// Генерирует коридоры до тех пор, пока не будет построено минимальное поданное на вход число коридоров И построенные коридоры хотя бы по одному разу не упрутся в каждый из краёв лабиринта.
    /// Таким образом, сделав длину лабиринта больше или равной длине дороги, нужной на выходе террейн-генератора, можно будет ГАРАНТИРОВАННО проложить по коридорам дорогу необходимой длины, например, сделав, поиск кратчайшего пути между крайними клетками лабиринта.
    /// </summary>
    public class LabyrinthGenerator
    {

        /// <summary>
        /// Генерирует лабиринт в соответствии с заданными параметрами. Возвращает массив вида int[][], где координаты каждой клетки хранятся в виде массивов из двух int-чисел (x = клетка[0] и y = клетка[1]).
        /// Метод сначала строит коридоры рандомной длины в заданных пределах.
        /// После того как достигнуто минимальное число нужных коридоров в лабиринте (minCorridorsCount), начинает увеличивать пределы длины каждого последующего коридора на число corridorLengthincrement.
        /// Когда в каждую из границ коридора метод упёрся коридором хотя бы по одному разу, генерация заканчивается и происходит возврат получившегося массива клеток. 
        /// Подбирайте параметры на вход метода исходя из этого и в соответствии с нужным вам видом лабиринта.
        /// </summary>
        /// <param name="width">Общая ширина лабиринта (лабиринт имеет квадратную форму). </param>
        /// <param name="minFirstCorridorsLength">Минимальный предел длины коридоров, до того как построено минимальное количество коридоров.</param>
        /// <param name="maxFirstCorridorsLength">Максимальный предел длины коридоров, до того как построено минимальное количество коридоров.</param>
        /// <param name="minCorridorsCount">Минимальное количество коридоров</param>
        /// <param name="corridorLengthincrement">Значение, на которое будут увеличиваться пределы построения каждого коридора после того, как было построено минимальное заданное количество коридоров.</param>
        /// <param name="startingCoordinatesXY">Координаты клетки, из которой будет строиться лабиринт.</param>
        /// <returns></returns>
        public int[][] GenerateLabyrinth(int width, int minFirstCorridorsLength, int maxFirstCorridorsLength, int minCorridorsCount, int corridorLengthincrement, int[] startingCoordinatesXY)
        {
            //if (width < minFirstCorridorLength)
            //    throw new UnityException("Установленная длина первого коридора лабиринта меньше обшей ширины лабиринта. В поданных на вход LabyrinthGenerator.GenerateLabyrinth данных width должен быть больше или равен minFirstCorridorLength.");

            CurrentLabyrinthData labyrinth = new CurrentLabyrinthData
            {
                MinX = (startingCoordinatesXY[0] - width) / 2,
                MinY = (startingCoordinatesXY[1] - width) / 2,
                MaxX = (startingCoordinatesXY[0] + width) / 2,
                MaxY = (startingCoordinatesXY[1] + width) / 2,

                MinTotalCorridorsCount = minCorridorsCount
            };

            GenerateCorridor(minFirstCorridorsLength, maxFirstCorridorsLength, corridorLengthincrement, startingCoordinatesXY, Random.Range(0, 1) > 0, labyrinth);

            return labyrinth.Squares.ListOfCoordinatesToArray();

        }

        private void GenerateCorridor(int minLength, int maxLength, int corridorLengthincrement, int[] originSquare, bool isHorizontal, CurrentLabyrinthData labyrinth)
        {
            int corridorLength = Random.Range(minLength, maxLength);

            if (isHorizontal)
                GenerateHorizontalCorridor(corridorLength, originSquare, labyrinth);
            else
                GenerateVerticalCorridor(corridorLength, originSquare, labyrinth);

            labyrinth.CurrentCorridorsCount += 1;

            //ВНИМАНИЕ!!!! если строить по несколько новых ответвлений от каждого построенного коридора, то эту проверку необходимо ставить в самом начале метода, иначе лучше в конце.
            if (labyrinth.AllFourBordersAreReached() && labyrinth.CurrentCorridorsCount >= labyrinth.MinTotalCorridorsCount)
                return;

            if (labyrinth.CurrentCorridorsCount >= labyrinth.MinTotalCorridorsCount)
            {
                minLength = minLength + corridorLengthincrement;
                maxLength = maxLength + corridorLengthincrement;
            }

            //рекурсивный вызов построения следующего коридора
            //увеличение длины коридора позволяет не нарваться на бесконечный цикл - в крайнем случае получится крестик, вписанный в пределы лабиринта.
            //Вот это - Random.Range(squares.Count - corridorLength + 2, squares.Count - 1) - исключаю из вариантов нового ориджинпойнта крайние точки коридора, чтобы не получилось лабиринта без тупиков.
            GenerateCorridor(minLength, maxLength, corridorLengthincrement, labyrinth.Squares[Random.Range(labyrinth.Squares.Count - corridorLength + 2, labyrinth.Squares.Count - 1)], !isHorizontal, labyrinth);

            
        }


        private void GenerateHorizontalCorridor(int corridorLength, int[] originSquare, CurrentLabyrinthData labyrinth)
        {
            //Debug.Log("Построено " + labyrinth.Squares.Count);

            int corridorStartingSquareX = Random.Range(originSquare[0] - corridorLength + 1, originSquare[0]);

            //если уперлись в (или вылетели за) левую границу лабиринта, меняем X начальной точки на X левой границы, а также отмечаем, что левая граница достигнута.
            if (corridorStartingSquareX <= labyrinth.MinX)
            {
                corridorStartingSquareX = labyrinth.MinX;
                labyrinth.LeftBorderReached = true;
            }

            //если уперлись в (или вылетели за) правую границу лабиринта, укорачиваем длину коридора, а также отмечаем, что правая граница достигнута.
            if (corridorStartingSquareX + corridorLength + 1 >= labyrinth.MaxX)
            {
                corridorLength = labyrinth.MaxX - corridorStartingSquareX + 1;
                labyrinth.RightBorderReached = true;
            }

            for (int i = 0; i < corridorLength; i++)
            {
                int[] squareCoordinates = new int[] { corridorStartingSquareX + i, originSquare[1] };
                if (!ListContainsCoordinates(labyrinth.Squares, squareCoordinates))
                    labyrinth.Squares.Add(squareCoordinates);
            }
        }

        private void GenerateVerticalCorridor(int corridorLength, int[] originSquare, CurrentLabyrinthData labyrinth)
        {
            int corridorStartingSquareY = Random.Range(originSquare[1] - corridorLength + 1, originSquare[1]);

            //если уперлись в (или вылетели за) нижнюю границу лабиринта, меняем Y начальной точки на Y нижней границы, а также отмечаем, что нижняя граница достигнута.
            if (corridorStartingSquareY <= labyrinth.MinY)
            {
                corridorStartingSquareY = labyrinth.MinY;
                labyrinth.BottomBorderReached = true;
            }

            //если уперлись в (или вылетели за) верхнюю границу лабиринта, укорачиваем длину коридора, а также отмечаем, что верхняя граница достигнута.
            if (corridorStartingSquareY + corridorLength + 1 >= labyrinth.MaxY)
            {
                corridorLength = labyrinth.MaxY - corridorStartingSquareY + 1;
                labyrinth.TopBorderReached = true;
            }

            for (int i = 0; i < corridorLength; i++)
            {
                int[] squareCoordinates = new int[] { originSquare[0], corridorStartingSquareY + i };
                if (!ListContainsCoordinates(labyrinth.Squares, squareCoordinates))
                    labyrinth.Squares.Add(squareCoordinates);
            }
        }


        //Метод для проверки, есть ли данные координаты в списке. Обычный List.Contains в данном случае не работает
        //, потому что int[] - ссылочный тип, и проверяется не значение, а ссылка.
        //Если ссылка не совпадает, обычный List.Contains вернёт false, даже если значения одинаковы.
        private bool ListContainsCoordinates(List<int[]> list, int[] squareXY)
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