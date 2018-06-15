using UnityEngine;

namespace Game.CommandUI
{
	/// <summary>
	/// Объект должен обладать любым типом Collider для возможности выбора
	/// </summary>
	[RequireComponent(typeof(Collider))]

	/// <summary>
	/// Данный класс необходимо присваивать любому объекту игровой сцены,
	/// который требуется выделить и отобразить игровой интерфейс. Для определения
	/// типа отображаемого интерфейса, необходимо корретно установить тип игрвого объекта
	/// ObjectType. Объект обязан иметь в своих компонентах любой вид Collider для корректной 
	/// работы события OnMouseDown()
	/// </summary>
	/// <param name="ObjectType">Тип объекта, определяющий интерфейс, отображаемый 
	/// в игре при выделении данного объекта</param>
	public class SelectableObject : MonoBehaviour
	{
		public SelectableObjectType ObjectType = SelectableObjectType.None;

		/// <summary>
		/// при клике мышью по объекту, передаем в SelectionManager.Instance 
		/// ссылку на объект, к которому привязан компонент, как на новый 
		/// выделенный объект сцены
		/// </summary>
		public void OnMouseDown ()
		{
			SelectedObjectManager.Instance.SelectedObject = this.gameObject;
		}
	}
}