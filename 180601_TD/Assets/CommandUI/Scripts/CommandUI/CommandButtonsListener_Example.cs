using UnityEngine;
using Game.Towers;
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
        // пока здесь все криво, логика будет вынесена в конкретные классы и мeтоды, здесь будет лишь обращение типа build()
        public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (viaButton.Meta == "BuildRocketTower")
            {
                
                var tower = TowersManager.Instance.RocketTowers[0];
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