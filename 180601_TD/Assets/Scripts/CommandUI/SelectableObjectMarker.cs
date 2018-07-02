using System;

using UnityEngine;

namespace Game.CommandUI
{
	/// <summary>
	/// Данный компонент позволяет выделенному объекту получить маркер выделения
	/// либо предустановленный префаб - MarkerPrefab, либо дефолтный - красную сферу
	/// </summary>
	public class SelectableObjectMarker : Singleton<SelectableObjectMarker>
	{
		/// <summary>
		/// префаб маркера, отобажающий что объект выделен
		/// если не задан - полупрозрачная красная сфера
		/// </summary>
		[SerializeField] private GameObject _marker = null;

		/// <summary>
		/// Цвет дефолтного маркера выделения
		/// </summary>
		private Color _color = new Color(1f, 0.3f, 0.3f, 0.2f);

		/// <summary>
		/// Увеличение размера сферы относительно объекта
		/// </summary>
		private float _scale = 2f;


		#region Стандартный функционал MonoBehaviour

		void Awake()
		{
			if (this._marker == null)
				this.BuildDefaultMarker ();
        }

        private void OnEnable()
        {
            SelectedObjectManager.Instance.OnSelectedObjectChanged += this.OnSelectableObjectChanged;
        }

        #endregion


        #region Реализация слушателя (listners) для CommandUI

        /// <summary>
        /// Вызывается в момент, когда на сцене меняется выбранный объект
        /// Либо отображает, либо прячет маркер выбора объекта
        /// </summary>
        /// <param name="obj">Объект, для которого отображаем маркер, либо null</param>
        private void OnSelectableObjectChanged(GameObject obj)
        {
            if (obj)
                this.ShowMarker(obj);
            else
                this.HideMarker();
        }

        #endregion


        #region Приватные методы компонента

		/// <summary>
		/// Выполняет построение дефолтного селектора
		/// </summary>
		private void BuildDefaultMarker()
		{
			this._marker = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			this._marker.name = "Selection sphere";
			this._marker.transform.parent = this.transform;
			this._marker.SetActive (false);
			this._marker.GetComponent<SphereCollider> ().enabled = false;
			this._marker.GetComponent<MeshRenderer> ().material =
				new Material (Shader.Find ("Transparent/Diffuse")) {
					color = this._color
				};
        }

        /// <summary>
        /// Отображает объект-маркер выбранного объекта 
        /// </summary>
        /// <param name="forObject">Объект, для которого отображаем маркер</param>
        private void ShowMarker(GameObject forObject)
        {
            this._marker.transform.position = forObject.transform.position;
            this._marker.transform.localScale = forObject.transform.localScale * this._scale;
            this._marker.SetActive(true);
        }

        /// <summary>
        /// Прячет маркер
        /// </summary>
        private void HideMarker()
        {
            this._marker.SetActive(false);
        }

        #endregion

	}
}
