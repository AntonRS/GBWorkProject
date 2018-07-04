using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace Game.CommandUI
{
    /// <summary>
    /// Класс реализцет фабрику меню для выделенного объекта
    /// Отображает меню над объектом на заданной высоте с учетом типа 
    /// выделенного объекта
    /// </summary>
    public class MenuFactory : MonoBehaviour
    {
        /// <summary>
        /// Высота, на которой отображается меню над выбранным объектом 
        /// </summary>
        public float Height = 2f;

        /// <summary>
        /// Ссылка на GameObject игры/сцены, содержащий в себе компонент ICommandButtonActuator
        /// Этот объект будет основным получателем комманд от коммандных кнопок, если на них не 
        /// будет явно указан другой комопнент, выполняющий роль слушателя команд
        /// </summary>
        public GameObject GlobalActuatorObject = null;

        /// <summary>
        /// список всех вариантов меню для выделяемых объектов SelectableObject
        /// </summary>
        public MenuFactoryItem[] MenuDefinitionItems = new MenuFactoryItem[0];

        /// <summary>
        /// Ссылка на текущее отображаемое меню, либо <c>null</c>, если объект не был выбран
        /// </summary>
        private GameObject _menu = null;

        /// <summary>
        /// Ссылка на комопонент ICommandButtonActuator объекта GlobalActuator
        /// Задается в момент создания этой фабрики. Если GlobalActuator не задан,
        /// командные кнопки будут использовать свой алгоритм обращения (см. документацию к кнопке)
        /// </summary>
        private ICommandButtonActuator _globalActuator = null;

        /// <summary>
        /// Пул меню, которые уже отображались для повторного использования
        /// </summary>
        private Dictionary<int, GameObject> _pool = new Dictionary<int, GameObject>();


        #region Стандартный функционал MonoBehaviour

        void Awake()
        {
            if (this.GlobalActuatorObject)
                this._globalActuator = this.GlobalActuatorObject.GetComponent<ICommandButtonActuator>();
        }

        #endregion


        #region Публичные методы компонента

        /// <summary>
        /// Выполняет отображение меню для заданного выделенного объекта или прячет все 
        /// меню, если выделенный объект отсутствует
        /// </summary>
        /// <param name="selectedObject">Выделенный игровой объект</param>
        /// <param name="ofType">Тип выбранного объекта</param>
        public void ShowMenuFor(GameObject selectedObject, SelectableObjectType ofType)
        {
            this.HideMenu();
            if (selectedObject != null && ofType != SelectableObjectType.None)
                if (!this.SearchCachedMenuFor(selectedObject, ofType))
                    this.BuldAndDisplayMenuFor(selectedObject, ofType);
        }

        /// <summary>
        /// Прячет текущее меню если было отображено ранее
        /// </summary>
        public void HideMenu()
        {
            if (this._menu != null)
                this._menu.SetActive(false);

            this._menu = null;
        }

        #endregion


        #region Приватные методы компонента

        /// <summary>
        /// Выполняет поиск меню для объекта в пуле уже созданных меню
        /// Если находит такое меню - отображает его над новым выделенным объектом и возвращает <c>true</c>
        /// Иначе возвращает <c>false</c>
        /// </summary>
        /// <returns><c>true</c>, если меню найдено, <c>false</c> иначе</returns>
        /// <param name="selectedObject">Выделенный игровой объект</param>
        /// <param name="ofType">Тип выбранного объекта</param>
        private bool SearchCachedMenuFor(GameObject selectedObject, SelectableObjectType ofType)
        {
            GameObject cached = null;
            this._pool.TryGetValue((int)ofType, out cached);

            if (cached)
                this.DisplayMenu(cached, selectedObject);

            return cached != null;
        }

        /// <summary>
        /// Создает новое меню для выделенного объекта, кэширует его в пуле и отображает на экран
        /// </summary>
        /// <param name="selectedObject">Выделенный игровой объект</param>
        /// <param name="ofType">Тип выбранного объекта</param>
        private void BuldAndDisplayMenuFor(GameObject selectedObject, SelectableObjectType ofType)
        {
            GameObject menu = this.InstantiateMenuForObject(ofType);

            this._pool.Add((int)ofType, menu);

            this.DisplayMenu(menu, selectedObject);
        }

        /// <summary>
        /// Создает инстанс игрового меню для заданного типа ofType,
        /// деактивирует его и возвращает ссылку на это меню
        /// </summary>
        /// <returns>Созданное меню</returns>
        /// <param name="ofType">Тип выбранного объекта</param>
        private GameObject InstantiateMenuForObject(SelectableObjectType ofType)
        {
            MenuFactoryItem item = SearchMenuItemDefinitionForObject(ofType);

            GameObject instance = GameObject.Instantiate(item.MenuPrefab, this.transform);

            if (instance == null)
                throw new NullReferenceException("Не удалось создать инстанс меню для объекта типа [" + ofType + "]");

            instance.SetActive(false);

            return instance;
        }

        /// <summary>
        /// Выполняет поиск референса на меню объекта указанного типа
        /// если ссылка не найдена, выбрасывает исключение
        /// </summary>
        /// <returns>Ссылку на взождение MenuFactoryItem в словаре MenuDefinitionItems</returns>
        /// <param name="ofType">Тип выбранного объекта</param>
        private MenuFactoryItem SearchMenuItemDefinitionForObject(SelectableObjectType ofType)
        {
            foreach (MenuFactoryItem item in this.MenuDefinitionItems)
                if (item.ObjectType == ofType)
                    return item;

            throw new NullReferenceException("Не удалось найти ссылку на префаб меню для объекта [" + ofType + "]");
        }

        /// <summary>
        /// Выполняет отображение игрового меню над выделенным объектом
        /// </summary>
        /// <param name="menu">Требуемое к отображению меню</param>
        /// <param name="selectedObject">Выделенный игровой объект</param>
        private void DisplayMenu(GameObject menu, GameObject selectedObject)
        {
            this._menu = menu;
            this._menu.transform.position = selectedObject.transform.position;
            this._menu.transform.Translate(new Vector3(0, this.Height, 0));

            this._menu.GetComponentsInChildren<CommandButton>()
                .ToList()
                .ForEach((button) =>
                {
                    button.Actuator = this._globalActuator;
                    button.TargetObject = selectedObject;
                });

            this._menu.SetActive(true);
        }

        #endregion

    }
}

