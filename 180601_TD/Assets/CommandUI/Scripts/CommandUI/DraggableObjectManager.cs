using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CommandUI
{
	/// <summary>
	/// Синглтон.
    /// Отвечает за информацию о выбранном DraggeableObject
	/// </summary>
    public class DraggableObjectManager : Singleton<DraggableObjectManager>
	{
        /// Перемещаемый в игре объект, либо <c>null</c>
        public GameObject DraggedObject
        {
            get
            {
                return this._draggedObject;
            }
            set
            {
                this._draggedObject = (value == this._draggedObject ? null : value);
            }
        }

        /// <summary>
        /// Максимальный радиус сферы перемещения объекта DraggeableObject по
        /// TerrainLayerMask в поле зрения игровой камеры. Используется в 
        /// DraggeableObject (см. код)
        /// </summary>
        public float MaxDragSphereRadius = 50f;

        /// <summary>
        /// Высота зависания объекта DraggeableObject при его перемещении 
        /// </summary>
        public float HoverHeight = 3f;

        /// <summary>
        /// см. свойство
        /// </summary>
        private GameObject _draggedObject = null;
	}
}
