using System.Linq;
using System.Collections;
using Game.Towers;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.EventSystems;

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
    public class CommandButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMesh _costText;

        public void SetValues()
        {
            
            if (_commandType == CommandType.Upgrade)
            {
                var cost = TargetObject.GetComponent<BaseTower>().GetUpgradeCost().ToString();
                if (cost != null)
                {
                    _costText.text = cost.ToString();
                }
                else
                {
                    _costText.text = string.Empty;
                }
            }
            if (_commandType == CommandType.Build)
            {
                if (Meta == "BuildRocketTower")
                {
                    _costText.text = GameManager.Instance.GetTowersManager.rocketTowers[0].Cost.ToString();
                }
                if (Meta == "BuildLazerTower")
                {
                    _costText.text = GameManager.Instance.GetTowersManager.lazerTowers[0].Cost.ToString();
                }
                if (Meta == "BuildGunTower")
                {
                    _costText.text = GameManager.Instance.GetTowersManager.gunTowers[0].Cost.ToString();
                }
            }
            if (_commandType == CommandType.Sell)
            {
                _costText.text = TargetObject.GetComponent<BaseTower>().SellCost.ToString();
            }
        }
        



        /// <summary>
        /// Ссылка на объект, реализующий интерфейс ICommandButtonActuator для этой кнопки
        /// на игровой сцене
        /// </summary>
        private ICommandButtonActuator _actuator = null;

        [HideInInspector] public ICommandButtonActuator Actuator 
        {
            get 
            {
                return this._actuator;
            }
            set 
            {
                this._actuator = value;

                if (this._actuator != null)
                    this._sprite.enabled =
                        this._actuator.TestCommandButtonShouldShow(this._commandType, this);
            }
        }

        /// <summary>
        /// Тип команды, выполняемой данной кнопкой
        /// </summary>
        [SerializeField] private CommandType _commandType = CommandType.None;

        public CommandType Command
        {
            get
            {
                return _commandType;
            }
        }

        /// <summary>
        /// Мета-дата кнопки, позволяет задать необходимые доп данные кнопки.
        /// Например, в случае нескольких кннопок апргрейда, высылается одинаковый
        /// тип комманды Upgrade. Используя мета, можно указать о каком апгрейде
        /// именно идет речь. Например так: "Upgrade #1 Encrease strength"
        /// </summary>
        public string Meta;

        /// <summary>
        /// Объект, на который данная кнопка воздействует, передается из
        /// <c>SelectedObjectManager</c> в момент отображения меню
        /// </summary>
        [HideInInspector] public GameObject TargetObject = null;

        /// <summary>
        /// Ссылка на компонент-картинку кнопки
        /// </summary>
        private SpriteRenderer _sprite = null;


        #region Стандартный функционал MonoBehaviour

        void Awake()
        {
            this.StartCoroutine(this.LookAtCamera());
            this._sprite = this.GetComponent<SpriteRenderer>();
        }

        #endregion


        #region Имплиментация интерфейса IPointerEnterHandler

        /// <summary>
        /// Вызывает метод <c>PreviewCommandBegan</c> на заданном <c>Actuator</c>
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.TargetObject != null && this.Actuator != null)
                this.Actuator.PreviewCommandBegan(
                    this.Command,
                    this.TargetObject,
                    this
                );
        }

        #endregion


        #region Имплиментация интерфейса IPointerExitHandler

        /// <summary>
        /// Вызывает метод <c>PreviewCommandEnd</c> на заданном <c>Actuator</c>
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.TargetObject != null && this.Actuator != null)
                this.Actuator.PreviewCommandEnd(
                    this.Command,
                    this.TargetObject,
                    this
                );
        }

        #endregion


        #region Имплиментация интерфейса IPointerClickHandler

        /// <summary>
        /// Отправляет сообщение о выполняемой операции заданному <c>Actuator</c>
        /// либо бродкастит всем объектам, реализующим интерфейс <c>ICommandButtonActuator</c>
        /// </summary>
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (this.TargetObject != null && this.Actuator != null)
                this.Actuator.ExecuteCommand(
                    this.Command,
                    this.TargetObject,
                    this
                );

            SelectedObjectManager.Instance.SelectedObject = null;
        }

        #endregion


        #region Приватные методы компонента

        /// <summary>
        /// Принуждает кнопку "смотреть" в камеру
        /// </summary>
        private IEnumerator LookAtCamera()
        {
            yield return new WaitForSeconds(0.5f);

            Vector3 target = Camera.main.transform.position;
            target.y = this.transform.position.y;

            this.gameObject.transform.LookAt(target);

            this.StartCoroutine(this.LookAtCamera());
        }

        #endregion

    }
}

