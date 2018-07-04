using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.TerrainGeneration
{

    public class TerrainNavMeshBuilder
    {
        private NavMeshSurface _navMeshSurface;

        /// <summary>
        /// Создать новый объект для создания NavMesh-а. На вход принимает находящийся на сцене GameObject, на котором нужно будет строить NavMesh.
        /// </summary>
        /// <param name="terrainParent"></param>
        public TerrainNavMeshBuilder (GameObject terrainParent)
        {
            _navMeshSurface = terrainParent.GetComponent<NavMeshSurface>();
            if (_navMeshSurface == null)
                _navMeshSurface = terrainParent.AddComponent<NavMeshSurface>();
        }
        
        public void BuildNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }

}
