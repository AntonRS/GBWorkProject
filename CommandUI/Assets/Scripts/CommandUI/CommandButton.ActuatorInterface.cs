using System;
using UnityEngine;

namespace Game.CommandUI
{
	/// <summary>
	/// Интерфейс объекта, реализующего функцию коммандной кнопки при ее нажатии
	/// </summary>
	public interface ICommandButtonActuator
	{
		/// <summary>
		/// Функция получает указание, какую команду и над каким объектом надо выполнить
		/// </summary>
		/// <param name="ofType">Тип исполняемой команды</param>
		/// <param name="forObject">Объект, вызвавший данную комнаду</param>
		/// <param name="viaButton">Ссылка на командную кнопку, вызвавшую данное действие</param>
		void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton);
	}
}

