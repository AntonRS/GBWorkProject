using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    /// <summary>
    /// Пока очень временно, просто кубики ставим туда где должы быть коридоры
    /// </summary>
    public class TilePlacer
    {

        public void PlaceTiles(int[][] arrayOfCoordinates, GameObject objectToPlace, Transform terrainParent)
        {
            foreach (var point in arrayOfCoordinates)
                MonoBehaviour.Instantiate(objectToPlace, new Vector3(point[0], 0, point[1]),Quaternion.identity);
        }


    }

}