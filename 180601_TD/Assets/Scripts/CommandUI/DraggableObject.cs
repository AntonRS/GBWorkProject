using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CommandUI
{
    /// <summary>
    /// Объект должен обладать любым типом Collider для возможности 
    /// взаимодействия с пользователем
    /// </summary>
    [RequireComponent(typeof(Collider))]

    /// <summary>
    /// Данный класс необходимо присваивать любому объекту игровой сцены,
    /// который требуется выделить и отобразить игровой интерфейс. Для определения
    /// типа отображаемого интерфейса, необходимо корретно установить тип игрвого объекта
    /// ObjectType. Объект обязан иметь в своих компонентах любой вид Collider для корректной 
    /// работы события OnMouseDown()
    /// </summary>
    /// <param name="ObjectType">Тип объекта, определяющий интерфейс, отображаемый 
    /// в игре при выделении данного объекта</param>
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Маска объекта/объектов по которым можно перемещать объект
        /// Применение к маске "Nothing" (==0) не разрешается. Если слой не задан 
        /// в редакторе, будет попытка найти слой "Terrain". Если и такого слоя нет,
        /// перемещение объекта запрещено
        /// </summary>
        public LayerMask TerrainLayerMask;

        /// <summary>
        /// Маркер состояния объекта во время перемещения
        /// </summary>
        private DraggableObjectState _state = DraggableObjectState.None;

        private Vector3 _lastAllowedDragPosition = Vector3.zero;


        #region Стандартый функционал MonoBehaviour

        void Awake()
        {
            if (this.TerrainLayerMask.value == 0)
                this.TerrainLayerMask = LayerMask.NameToLayer("Terrain");
        }

        #endregion


        #region Имплиментация интерфейса IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            DraggableObjectManager.Instance.DraggedObject = this.gameObject;

            this._state = DraggableObjectState.StartDrag;
            this.DragObjectVia(eventData);
        }

        #endregion


        #region Имплиментация интерфейса IDragHandler
    
        public void OnDrag(PointerEventData eventData)
        {
            this._state = DraggableObjectState.Dragging;
            this.DragObjectVia(eventData);
        }

        #endregion


        #region Имплиментация интерфейса IEndDragHandler

        public void OnEndDrag(PointerEventData eventData)
        {
            this._state = DraggableObjectState.EndDrag;
            this.DragObjectVia(eventData);
            this._state = DraggableObjectState.None;

            DraggableObjectManager.Instance.DraggedObject = null;
        }

        #endregion


        #region приватные методы объекта

        /// <summary>
        /// Выполняет перемещение данного объекта по объекту/объектам,
        /// соответствующим TerrainLayerMask. Если TerrainLayerMask не задано (==-1),
        /// или состояние перемещения не задано (==None) перемещение не происходит
        /// </summary>
        private void DragObjectVia(PointerEventData eventData) 
        {
            if (this.TerrainLayerMask.value == -1)
                return;

            if (this._state == DraggableObjectState.None)
                return;

            Vector3 collisionPoint = Vector3.zero;
            if (this.LookForNewWorldPosition(eventData, out collisionPoint))
                this.SetNewPosition(collisionPoint);
            else if (this._state == DraggableObjectState.EndDrag)
                this.PlantObject(this._lastAllowedDragPosition);
        }

        /// <summary>
        /// Выполняет поиск ближайшего объекта, соответствущего маске TerrainLayerMask,
        /// пересекающегося с лучом зрения из камеры в позицию курсора на экране.
        /// Максимальный радиус области поиска точки пересечения задается в 
        /// DraggableObjectManager.MaxDragSphereRadius (см. код)
        /// </summary>
        /// <returns><c>true</c>, если точка пересечения найдена, <c>false</c> если не найдена.</returns>
        /// <param name="collisionPoint">Расчетная точка коллизии луча из камеры с поверхностью</param>
        private bool LookForNewWorldPosition(PointerEventData eventData, out Vector3 collisionPoint) 
        {
            collisionPoint = Vector3.zero;

            Camera cam = eventData.pressEventCamera;
            Vector3 point = cam.ScreenToViewportPoint(eventData.position);
            Ray ray = cam.ViewportPointToRay(point);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(
                ray, 
                out hit, 
                DraggableObjectManager.Instance.MaxDragSphereRadius, 
                this.TerrainLayerMask 
            )) {
                collisionPoint = hit.point;
                this._lastAllowedDragPosition = collisionPoint;
            }

            return collisionPoint != Vector3.zero;
        }

        /// <summary>
        /// Размещает данный объект в заданную точку с учетом статуса перемещения
        /// </summary>
        /// <param name="collisionPoint">Точка коллизии == новая позиция объекта</param>
        private void SetNewPosition(Vector3 collisionPoint) 
        {
            this.transform.position = collisionPoint;
            this.transform.Translate(0f, DraggableObjectManager.Instance.HoverHeight, 0f);

            if (this._state == DraggableObjectState.EndDrag)
                this.PlantObject(collisionPoint);
        }

        /// <summary>
        /// Усаживает объект обратно на поверхность
        /// </summary>
        private void PlantObject(Vector3 collisionPoint)
        {
            Ray ray = new Ray(collisionPoint, Vector3.up);
            RaycastHit hit = new RaycastHit();

            Physics.Raycast(ray, out hit, 1000f); // <-- здесь можно магическое число

            this.transform.Translate(0f, -hit.distance, 0f);
        }

        #endregion
    }
}