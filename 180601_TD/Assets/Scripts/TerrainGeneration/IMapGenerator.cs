using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGeneration
{

    /// <summary>
    /// Генератор карты
    /// </summary>
    public interface IMapGenerator
    {
        /// <summary>
        /// Возаращает объект на сцене, в котором создаётся карта.
        /// </summary>
        GameObject TerrainParent { get; }

        /// <summary>
        /// Загружает тайлсет с префабами для генерации карты.
        /// </summary>
        /// <param name="tileset"></param>
        void LoadTilePrefabs(TerrainTilesetData tileset);

        /// <summary>
        /// Создаёт карту по заданным параметрам.
        /// </summary>
        /// <param name="minRoadTiles"></param>
        /// <param name="maxRoadTiles"></param>
        /// <param name="towerPlatformsCount"></param>
        /// <returns>Список всех элементов карты, где объект с нулевым индексом - стартовый тайл, где мобы спавнятся, а объект с последним индексом - финальный тайл, куда мобы пытаются добежать. </returns>
        List<GameObject> GenerateTerrain(int minRoadTiles, int maxRoadTiles, int towerPlatformsCount);

        /// <summary>
        /// Уничтожает карту.
        /// </summary>
        void DestroyTerrain();

    }
    
}