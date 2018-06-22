using System.Linq;
using System.Collections;

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
    public class CommandButton : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Ссылка на объект, реализующий интерфейс ICommandButtonActuator для этой кнопки
        /// По умолчанию, присваивается к значению _glbalActuator объекта MenuFactory
        /// Если не задан (==null), при нажатии данной кнопки будут оповещены
        /// ВСЕ объекты, содержащие компонент <c>ICommandButtonActuator</c>
        /// на игровой сцене
        /// </summary>
        [HideInInspector] public ICommandButtonActuator Actuator = null;

        /// <summary>
        /// Тип команды, генерируемая при нажатии данной кнопки
        /// </summary>
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
        /// Тип команды, выполняемой данной кнопкой
        /// </summary>
        [SerializeField] private CommandType _commandType = CommandType.None;


        #region Стандартный функционал MonoBehaviour

        void Start()
        {
            this.StartCoroutine(this.LookAtCamera());
        }

        #endregion


        #region Имплиментация интерфейса IPointerClickHandler

        /// <summary>
        /// Отправляет сообщение о выполняемой операции заданному <c>Actuator</c>
        /// либо бродкастит всем объектам, реализующим интерфейс <c>ICommandButtonActuator</c>
        /// </summary>
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (this.TargetObject != null)
                if (this.Actuator == null)
                    GameObject.FindObjectsOfType<GameObject>()
                                .OfType<ICommandButtonActuator>()
                                .ToList()
                                .ForEach(this.ExecuteCommandOn);
                else
                    this.ExecuteCommandOn(this.Actuator);
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

        #endregion

    }
}

