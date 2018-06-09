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
		/// Дефолтный масштаб сферы относительно выделенного объекта
		/// </summary>
		private float _scale = 3f;


		#region Стандартный функционал MonoBehaviour

		void Awake()
		{
			if (this._marker == null)
				this.BuildDefaultMarker ();

			this.DrawMarkerOver (null);
		}

		#endregion


		/// <summary>
		/// Выполняет построение дефолтного селектора
		/// </summary>
		private void BuildDefaultMarker()
		{
			this._marker = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			this._marker.GetComponent<SphereCollider> ().enabled = false;
			this._marker.GetComponent<MeshRenderer> ().material = 
				new Material (Shader.Find ("Transparent/Diffuse")) {
					color = this._color
				};
		}

		/// <summary>
		/// перемещает и показывает маркер выделения объекта
		/// </summary>
		public void DrawMarkerOver(GameObject selected) 
		{
			if (selected == null) { 
				this._marker.SetActive (false);	
			} else {
				this._marker.transform.position = selected.transform.position;
				this._marker.transform.localScale = selected.transform.localScale * this._scale;
				this._marker.SetActive (true);	
			}
		}
	}
}

