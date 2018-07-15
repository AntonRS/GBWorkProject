using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CommandUI
{
    // запускаем данный менеджер первым
    [DefaultExecutionOrder(-1)]

	/// <summary>
	/// Синглтон.
	/// Отображает нужный интерфейс для выделенного в игре объекта SelectedObject
	/// </summary>
	public class SelectedObjectManager : Singleton<SelectedObjectManager>
	{
        void HandleAction(Delegate obj)
        {
        }


        /// Выбранный в игре объект, либо <c>null</c>
		/// если пытаемся выделить тот же объект - просто снимем выделение и
		/// забудем объект, наче выделим новый объект. При изменении выбранного 
        /// объекта, оповещаются все подписчики события <c>OnSelectedObjectChanged</c>
		public GameObject SelectedObject
		{
			get {
				return this._selectedObject;
			}
			set {
				this._selectedObject = (value == this._selectedObject ? null : value);
				this._selectedObjectType = this._selectedObject ? value.GetComponent<SelectableObject> ().ObjectType : SelectableObjectType.None;

                if (this.OnSelectedObjectChanged != null)
                    this.OnSelectedObjectChanged.Invoke(this._selectedObject);
			}
		}


        #region Делегация

        /// <summary>
        /// Делегат метода оповещения об изменении выбранного на сцене объекта
        /// </summary>
        public delegate void SelectedObjectChanged(GameObject obj);

        /// <summary>
        /// Событие об изменении выбранного объекта на сцене для подписки
        /// </summary>
        public event SelectedObjectChanged OnSelectedObjectChanged;

        #endregion


        #region Некоторые приватные поля класса для внутреннего управления

		private GameObject _selectedObject = null;
		private SelectableObjectType _selectedObjectType = SelectableObjectType.None;

        #endregion


        #region Стандартный функционал MonoBehaviour

        void Awake()
		{
			if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
                throw new MissingReferenceException ("Игровая камера должна содержать в себе компонент PhysicsRaycaster");
		}

		#endregion
	}
}
