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
            if (viaButton.Meta == "BuildRocketTower")
            {
                var tower = Main.Instance.RocketTowers[0];
                var rocketTower = Instantiate(tower, forObject.transform.position, Quaternion.identity);
                Destroy(forObject);
            }
            if (viaButton.Meta == "Upgrade")
            {
                
                forObject.GetComponent<BaseTower>().UpdateTower();
                Destroy(forObject);
            }
            SelectedObjectManager.Instance.SelectedObject = null;
            Debug.Log(string.Format("Executing command [{0}] on object [{1}] via button [{2}]", ofType, forObject, viaButton));
        }

        #endregion
    }
}