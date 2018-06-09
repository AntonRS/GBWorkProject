using System.Collections;
using UnityEngine;
using System;
using System.Linq;

namespace Game.CommandUI
{
	/// <summary>
	/// Объект должен обладать людым типом Collider для возможности выбора
	/// </summary>
	[RequireComponent(typeof(Collider))]

	/// <summary>
	/// Класс кнопки контекстного меню объекта
	/// Принуждает кнопку, на которую назначена, всегда смотреть лицом в текущую игровую камеру
	/// Выполняет генерацию события указанного типа при нажатии
	/// </summary>
	public class CommandButton : MonoBehaviour
	{
		/// <summary>
		/// Ссылка на объект-интерфейс, слушающий данную кнопку
		/// Если не задан, при нажатии данной кнопки будут оповещены
		/// ВСЕ объекты, содержащие компонент <c>ICommandButtonActuator</c>
		/// на игровой сцене
		/// </summary>
		public ICommandButtonActuator Actuator = null;

		/// <summary>
		/// Тип команды, генерируемая при нажатии данной кнопки
		/// </summary>
		public CommandType Command 
		{ 
			get {
				return _commandType;
			}
		}

		/// <summary>
		/// Объект, на который даннай кнопка воздействует, передается
		/// <c>SelectedObjectManager</c> в момент отображения меню
		/// </summary>
		[HideInInspector] public GameObject TargetObject = null;

		/// <summary>
		/// Тип команды, выполняемой данной кнопкой
		/// </summary>
		[SerializeField] private CommandType _commandType = CommandType.None;


		#region Стандартный функционал MonoBehaviour

		void Start()
		{
			this.StartCoroutine (this.LookAtCamera ());
		}

		/// <summary>
		/// Отправляет сообщение о выполняемой операции заданному <c>Actuator</c>
		/// либо бродкастит всем объектам, реализующим интерфейс <c>ICommandButtonActuator</c>
		/// </summary>
		void OnMouseDown()
		{
			if (this.TargetObject != null)
				if (this.Actuator == null) 
					GameObject.FindObjectsOfType<GameObject> ()
								.OfType<ICommandButtonActuator> ()
								.ToList ()
								.ForEach(this.ExecuteCommandOn);
				else
					this.ExecuteCommandOn (this.Actuator);
		}

		#endregion

		/// <summary>
		/// Принуждает кнопку "смотреть" в камеру
		/// </summary>
		private IEnumerator LookAtCamera()
		{
			yield return new WaitForSeconds (0.5f);

			Vector3 target = Camera.main.transform.position;
			target.y = this.transform.position.y;

			this.gameObject.transform.LookAt (target);

			this.StartCoroutine (this.LookAtCamera ());
		}

		/// <summary>
		/// Вызывает исполняемый метод <c>Execute</c> с данными данной кнопки
		/// на переданном в функцию объекте <c>ICommandButtonActuator</c>
		/// </summary>
		/// <param name="actuator">Обеъкт, куда передаем информацию о событии</param>
		private void ExecuteCommandOn(ICommandButtonActuator actuator)
		{
			actuator.ExecuteCommand(
				this.Command,
				this.TargetObject,
				this
			);
		}
	}
}

