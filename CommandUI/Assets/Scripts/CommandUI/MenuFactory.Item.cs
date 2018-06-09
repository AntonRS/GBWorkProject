using System;
using UnityEngine;

namespace Game.CommandUI
{
	/// <summary>
	/// Определение спика меню объекта для заполнения массива
	/// в MenuFactory.MenuPrefabs
	/// Содержит указание на тип объекта, для которого это меню должно отобразиться
	/// и ссылку на конкретный префаб меню
	/// </summary>
	[Serializable]
	public struct MenuFactoryItem
	{
		/// <summary>
		/// Тип объекта, для которого это меню действительно
		/// </summary>
		public SelectableObjectType ObjectType;

		/// <summary>
		/// Префаб меню
		/// </summary>
		public GameObject MenuPrefab;
	}
}

