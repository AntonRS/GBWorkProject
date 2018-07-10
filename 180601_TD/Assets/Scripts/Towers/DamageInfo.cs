namespace Game.Towers
{
    /// <summary>
    /// Параметры урона.
    /// </summary>
    public struct DamageInfo
    {
        /// <summary>
        /// Количество урона.
        /// </summary>
        public float Damage;
        /// <summary>
        /// Тип урона.
        /// </summary>
        public AttackType AttackType;
        /// <summary>
        /// Уменьшение скорости.
        /// </summary>
        public int SpeedReduction;
        /// <summary>
        /// Башня, которая наносит урон.
        /// </summary>
        public BaseTower AttackingTower;
    }
}

