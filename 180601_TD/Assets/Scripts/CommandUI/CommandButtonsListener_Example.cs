using UnityEngine;
using Game.Towers;
using Game.CommandUI;
using Game.TerrainGeneration;

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
                GameManager.Instance.GetTowersManager.BuildRocketTower(forObject.transform);
            }
            if (viaButton.Meta == "BuildLazerTower")
            {
                GameManager.Instance.GetTowersManager.BuildLazerTower(forObject.transform);
            }
            if (viaButton.Meta == "BuildGunTower")
            {
                GameManager.Instance.GetTowersManager.BuildGunTower(forObject.transform);
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