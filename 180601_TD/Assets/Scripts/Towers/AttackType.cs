namespace Game.Towers
{
    /// <summary>
    /// Тип урона
    /// </summary>
    public enum AttackType
    {
        /// <summary>
        /// переодический урон. dmg/sec
        /// </summary>
        lazer,
        /// <summary>
        /// переодический урон. dmg/sec
        /// </summary>
        bullets,
        /// <summary>
        /// урон наносится главной цели и распространяется на окружающих врагов
        /// </summary>
        rocket
    }
}

