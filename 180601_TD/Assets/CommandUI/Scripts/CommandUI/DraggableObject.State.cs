namespace Game.CommandUI
{
    /// <summary>
    /// Перечисление, состояние перемещаемого объекта в игре
    /// <para>Значения</para>
    /// <para>StartDrag : начало перемещения</para>
    /// <para>Dragging : перемещение по территории</para>
    /// <para>EndDrag : завершение перемещения</para>
    /// <para>None : маркер окончания перечисления</para>
    /// </summary>
    public enum DraggableObjectState : int
    {
        StartDrag,
        Dragging,
        EndDrag,

        // должен быть последним типом в перечислении
        None
    }
}
