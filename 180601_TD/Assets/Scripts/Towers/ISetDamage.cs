namespace Game.Towers
{
    /// <summary>
    /// Отвечает за получение урона.
    /// </summary>
    public interface ISetDamage
    {
        /// <summary>
        /// Получение урона.
        /// </summary>
        /// <param name="damageinfo">Параметры урона</param>
        void ApplyDamage(DamageInfo damageinfo);
    }
}
    

