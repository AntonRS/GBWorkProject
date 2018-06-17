using UnityEngine;
using GeekBrains;
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

            var newTower = Instantiate(viaButton.buildTower, forObject.transform.position, Quaternion.identity);


            Destroy(forObject);
            Debug.Log(string.Format("Executing command [{0}] on object [{1}] via button [{2}]", ofType, forObject, viaButton));
        }

        #endregion
    }
}