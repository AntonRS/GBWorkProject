using System;

namespace Game.CommandUI
{
    /// <summary>
    /// Данный интерфейс должен реализовываться любым объектом сцены, который
    /// хочет отобразить радиус своего действия
    /// </summary>
    public interface IRangeMarkerAssignee
    {
        /// <summary>
        /// Функция должна возвращать радиус действия объекта на сцене
        /// </summary>
        float OnRangeRequested();
    }
}
