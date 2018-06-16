namespace Game.CommandUI
{
    /// <summary>
    /// Перечисление, состояние перемещаемого объекта в игре
    /// <para>Значения</para>
    /// <para>StartMove : начало перемещения</para>
    /// <para>Moving : перемещение по территории</para>
    /// <para>EndMove : завершение перемещения</para>
    /// <para>None : маркер окончания перечисления</para>
    /// </summary>
    public enum MovableObjectState : int
    {
        StartMove,
        Moving,
        EndMove,

        // должен быть последним типом в перечислении
        None
    }
}
