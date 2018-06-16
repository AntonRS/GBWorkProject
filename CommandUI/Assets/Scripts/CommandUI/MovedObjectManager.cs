using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CommandUI
{
	/// <summary>
	/// Синглтон.
	/// Отображает нужный интерфейс для выделенного в игре объекта SelectedObject
	/// </summary>
    public class MovedObjectManager : Singleton<MovedObjectManager>
	{
        /// Перемещаемый в игре объект, либо <c>null</c>
        public GameObject MovedObject
        {
            get
            {
                return this._movedObject;
            }
            set
            {
                this._movedObject = (value == this._movedObject ? null : value);
            }
        }

        /// <summary>
        /// Максимальный радиус сферы перемещения объекта MovableObject по
        /// TerrainLayerMask в поле зрения игровой камеры. Используется в 
        /// MovableObject (см. код)
        /// </summary>
        public float MaxMoveSphereRadius = 50f;

        /// <summary>
        /// Высота зависания объекта MovableObject при его перемещении 
        /// </summary>
        public float HoverHeight = 3f;

        /// <summary>
        /// см. свойство
        /// </summary>
        private GameObject _movedObject = null;
	}
}
