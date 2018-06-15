namespace Game.CommandUI
{
	/// <summary>
	/// Перечисление, определяющие тип выделяемого объекта
	/// от выбора типа объекта зависит, какой интерфейс 
	/// будет отображаться в игре 
	/// <para>Значения</para>
	/// <para>EnemyBuilding : вражеская постройка</para>
	/// <para>EnemyUnit : вражеский юнит</para>
	/// <para>PlayerBuilding : постройка игрока</para>
	/// <para>PlayerUnit : юнит игрока</para>
	/// <para>NPCBuilding : нейтральная постройка</para>
	/// <para>NPCUnit : нейтральный юнит</para>
	/// <para>BuildableTerrain : территория, на которой можно строить</para>
	/// <para>SelectableTerrain : территория, которую можно выбрать и посмотреть инфу</para>
	/// <para>None : маркер окончания перечисления</para>
	/// </summary>
	public enum SelectableObjectType : int {
		EnemyBuilding,
		EnemyUnit,
		PlayerBuilding,
		PlayerUnit,
		NPCBuilding,
		NPCUnit,
		BuildableTerrain,
		SelectableTerrain,

		// должен быть последним типом в перечислении
		None
	}
}