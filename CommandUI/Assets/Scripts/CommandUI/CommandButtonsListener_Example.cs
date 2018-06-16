using UnityEngine;

using Game.CommandUI;

namespace Game
{
    /// <summary>
    /// Данный класс является ПРИМЕРОМ обработчика событий ICommandButtonActuator
    /// и не является частью CommandUI и/или игры
    /// </summary>
    public class CommandButtonsListener_Example : MonoBehaviour, ICommandButtonActuator
    {
        #region Пример имплиментации интерфейса ICommandButtonActuator

        public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            Debug.Log(string.Format("Executing command [{0}] on object [{1}] via button [{2}]", ofType, forObject, viaButton));
        }

        #endregion
    }
}