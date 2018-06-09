using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CommandUI
{
	// у менеджера должна быть ссылка на объект-маркер
	[RequireComponent(typeof(SelectableObjectMarker))] 

	// требуется ссылка на компонент фабрики меню
	[RequireComponent(typeof(MenuFactory))]

	/// <summary>
	/// Синглтон - фабрика.
	/// Отображает нужный интерфейс для выделенного в игре объекта SelectedObject
	/// </summary>
	public class SelectedObjectManager : Singleton<SelectedObjectManager> 
	{
		/// Выбранный в игре объект, либо <c>null</c>
		/// если пытаемся выделить тот же объект - просто снимем выделение и 
		/// забудем объект, наче выделим новый объект
		public GameObject SelectedObject
		{
			get { 
				return this._selectedObject; 
			}
			set { 
				this._selectedObject = (value == this._selectedObject ? null : value);
				this._selectedObjectType = this._selectedObject ? value.GetComponent<SelectableObject> ().ObjectType : SelectableObjectType.None;
				this._marker.DrawMarkerOver (this._selectedObject);
				this._menuFactory.ShowMenuFor (this._selectedObject, this._selectedObjectType);
			}
		}

		private SelectableObjectMarker _marker = null;
		private MenuFactory _menuFactory = null;

		private GameObject _selectedObject = null;
		private SelectableObjectType _selectedObjectType = SelectableObjectType.None;

		#region Стандартный функционал MonoBehaviour

		void Awake()
		{
			if (Camera.main.GetComponent<Physics2DRaycaster>() == null)
				throw new MissingReferenceException ("Игровая камера должна содержать в себе компонент RaycastHit2D");

			this._marker = this.GetComponent<SelectableObjectMarker> ();
			this._menuFactory = this.GetComponent<MenuFactory> ();

			if (this._marker == null)
				throw new MissingReferenceException (
					"Не задан компонент объект-маркер SelectableObjectMarker для выделения объекта"
				);
		}

		#endregion
	}
}