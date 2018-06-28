using UnityEngine;

namespace Game.CommandUI
{
	/// <summary>
	/// Интерфейс объекта, реализующего функцию коммандной кнопки при ее нажатии
	/// </summary>
	public interface ICommandButtonActuator
	{
        /// <summary>
        /// Данная функция вызывается в момент, когда очередная кнопка контекстное меню готова 
        /// отобразиться, но еще не показано на экране. В этот момент дуобно управлять видимостью 
        /// кнопки. Если функция возвращает <c>false</c> - кнлпка не будет отображена, иначе - будет
        /// </summary>
        /// <param name="ofType">Тип исполняемой команды</param>
        /// <param name="viaButton">Ссылка на командную кнопку, вызвавшую данное действие</param>
        bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton);

        /// <summary>
        /// Данная функция вызывается в момент, когда пользователь наводит укзатель 
        /// мыши на объект, который потенциально будет исполнять заданную комманду
        /// </summary>
        /// <param name="ofType">Тип исполняемой команды</param>
        /// <param name="viaButton">Ссылка на командную кнопку, вызвавшую данное действие</param>
        void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton);

        /// <summary>
        /// Вызывается, когда пользователь убироает указатель мыши с выбранного объекта
        /// </summary>
        /// <param name="ofType">Тип исполняемой команды</param>
        /// <param name="viaButton">Ссылка на командную кнопку, вызвавшую данное действие</param>
        void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton);

		/// <summary>
		/// Функция получает указание, какую команду и над каким объектом надо выполнить
		/// </summary>
		/// <param name="ofType">Тип исполняемой команды</param>
		/// <param name="viaButton">Ссылка на командную кнопку, вызвавшую данное действие</param>
		void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton);
	}
}

