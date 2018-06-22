namespace Game.CommandUI
{
	/// <summary>
	/// Перечисление, определяющие тип выполняемой функции
	/// <para>Значения</para>
	/// <para>Build : выполнить постройку</para>
	/// <para>Sell : продать объект</para>
	/// <para>Upgrade : Выполнить апгрейд объекта</para>
	/// <para>Trade : открыть меню торговли с объектом</para>
	/// <para>Speak : открыть диалог общения с объектом</para>
	/// <para>Info : получить информацию о выбранной территории</para>
	/// <para>None : маркер окончания перечисления</para>
	/// </summary>
	public enum CommandType : int {
		Build,
		Attack,
		Sell,
		Upgrade,
		Trade,
		Speak,
		Info,

		// должен быть последним типом в перечислении
		None
	}
}

