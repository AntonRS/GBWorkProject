using System;

using UnityEngine;

namespace Game.CommandUI
{
    /// <summary>
    /// Данный класс является ПРИМЕРОМ обработчика событий IRangeMarkerAssignee
    /// и ICommandButtonActuator в связке с меню апгрейда
    /// и не является частью CommandUI и/или игры
    /// </summary>
    public class TowerUpgrade_Example : MonoBehaviour, IRangeMarkerAssignee, ICommandButtonActuator
    {
        /// <summary>
        /// текущий уровень апргрейда объекта
        /// </summary>
        private string _rangeUpgrade = "";
        /// <summary>
        /// текущий радиус действия объекта
        /// </summary>
        private float _range = 3f;
        /// <summary>
        /// фейковый радиус, возвращаемый объектом, если работаем с превью команды Upgrade
        /// </summary>
        private Nullable<float> _fakeRange = null;

        private const string RangeUpgrade1 = "RangeUpgrade1";
        private const float Range1 = 5f;
        private const string RangeUpgrade2 = "RangeUpgrade2";
        private const float Range2 = 9f;


        #region Пример имплиментации интерфейса ICommandButtonActuator

        public bool TestCommandButtonShouldShow(CommandType ofType, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
                return this._rangeUpgrade != TowerUpgrade_Example.RangeUpgrade2;

            return true;
        }

        public void PreviewCommandBegan(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {
                viaButton.gameObject.SetActive(true);

                switch (this._rangeUpgrade)
                {
                    default:
                        this._fakeRange = TowerUpgrade_Example.Range1;
                        viaButton.Meta = TowerUpgrade_Example.RangeUpgrade1;
                        break;
                    case TowerUpgrade_Example.RangeUpgrade1:
                        this._fakeRange = TowerUpgrade_Example.Range2;
                        viaButton.Meta = TowerUpgrade_Example.RangeUpgrade2;
                        break;
                    case TowerUpgrade_Example.RangeUpgrade2:
                        this._fakeRange = null;
                        viaButton.Meta = null;
                        break;
                }
            }
        }

        public void PreviewCommandEnd(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            this._fakeRange = null;
            viaButton.Meta = null;
        }

        public void ExecuteCommand(CommandType ofType, GameObject forObject, CommandButton viaButton)
        {
            if (ofType == CommandType.Upgrade)
            {
                this._fakeRange = null;

                switch (viaButton.Meta)
                {
                    case TowerUpgrade_Example.RangeUpgrade1:
                        this._range = TowerUpgrade_Example.Range1;
                        this._rangeUpgrade = TowerUpgrade_Example.RangeUpgrade1;
                        break;
                    case TowerUpgrade_Example.RangeUpgrade2:
                        this._range = TowerUpgrade_Example.Range2;
                        this._rangeUpgrade = TowerUpgrade_Example.RangeUpgrade2;
                        break;
                }
            }
        }

        #endregion


        #region Пример имплиментации интерфейса IRangeMarkerAssignee

        public float OnRangeRequested()
        {
            return this._fakeRange ?? this._range;
        }

        #endregion
    }
}