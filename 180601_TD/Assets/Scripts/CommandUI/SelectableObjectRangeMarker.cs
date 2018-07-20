using UnityEngine;

namespace Game.CommandUI
{
    /// <summary>
    /// Данный компонент позволяет выделенному объекту отобразить радиус своего 
    /// влияния на сцене (range). Данный объект реализует делегат <c>SelectedObjectChanged</c>
    /// менеджера <c>SelectedObjectManager</c>
    /// </summary>
    public class SelectableObjectRangeMarker : Singleton<SelectableObjectRangeMarker>
    {
        /// <summary>
        /// Ссылка на компонент игрового объекта сцены, выделенного последний раз,
        /// либо null, если объект больше не выделен. Изменение ссылки происходит 
        /// в момент активации события <c>OnSelectedObjectChanged</c> 
        /// менеджера <c>SelectedObjectManager</c>
        /// </summary>
        private GameObject _target = null;

        /// <summary>
        /// Ссылка на компонент <c>IRangeMarkerAssignee</c> игрового объекта сцены, 
        /// ссылка на который хранится в <c>_target</c>
        /// </summary>
        private IRangeMarkerAssignee _component = null;

        /// <summary>
        /// период, с которым будет вызываться метод обновления
        /// для корректного отображения радиуса дейсвтия объекта
        /// </summary>
        private const float _invokeationInterval = 0.5f;

        /// <summary>
        /// Объект-маркер, отображающий радиус действия объекта
        /// Данный объект является дочерним объектом <c>SelectableObjectRangeMarker</c>
        /// и создается при создании данного объекта
        /// </summary>
        private GameObject _range = null;

        /// <summary>
        /// Цвет маркера выделения
        /// </summary>
        public Color Color = new Color(0.3f, 1f, 0.3f, 0.5f);

        /// <summary>
        /// Масштаб сферы-радуса по высоте
        /// </summary>
        private const float _squeezeY = 0.01f;

        #region Стандартный функционал MonoBehaviour

        void Awake()
        {
            if (this._range == null)
                this.BuildRangeMarker();    
        }

        private void OnEnable()
        {
            SelectedObjectManager.Instance.OnSelectedObjectChanged += this.OnSelectableObjectChanged;
        }

        #endregion


        #region Реализация слушателя (listners) для CommandUI

        /// <summary>
        /// Вызывается в момент, когда на сцене меняется выбранный объект
        /// Либо отображает, либо прячет маркер радиуса
        /// Передаваемый объект должен реализовывать интерфейс <c>IRangeMarkerAssignee</c>
        /// для отображения радиуса. Если объект не реализует этот интерфейс,
        /// маркер не отображается
        /// </summary>
        /// <param name="obj">Объект, для которого отображаем маркер, либо null</param>
        private void OnSelectableObjectChanged(GameObject obj)
        {
            this._target = obj;
            this._component = obj ? obj.GetComponent<IRangeMarkerAssignee>() : null;

            if (this._component == null)
                this.CancelInvoke("ShowRange");
            else
                this.InvokeRepeating("ShowRange", 0f, SelectableObjectRangeMarker._invokeationInterval);

            this._range.SetActive(this._component != null);
        }

        #endregion


        #region Приватные методы объекта

        /// <summary>
        /// Создает объект-маркер радиуса влияния объекта
        /// </summary>
        private void BuildRangeMarker()
        {
            this._range = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this._range.name = "Range marker";
            this._range.transform.parent = this.transform;
            this._range.SetActive(false);
            this._range.GetComponent<SphereCollider>().enabled = false;
            this._range.GetComponent<MeshRenderer>().material =
                new Material(Shader.Find("Transparent/Diffuse"))
                {
                    color = this.Color
                };
        }

        /// <summary>
        /// Отображает объект-маркер радуса действия для выбранного объекта, 
        /// если ссылка на него активна и содержится в <c>_target</c>, иначе
        /// маркер прячем
        /// </summary>
        private void ShowRange()
        {
            if (this._component == null)
                return;
            
            this._range.transform.position = this._target.transform.position;
            this._range.transform.localScale = new Vector3(
                this._target.transform.localScale.x,
                SelectableObjectRangeMarker._squeezeY,
                this._target.transform.localScale.z);
            this._range.transform.localScale *= this._component.OnRangeRequested()*2;
            this._range.SetActive(true);
        }

        #endregion
    }
}
