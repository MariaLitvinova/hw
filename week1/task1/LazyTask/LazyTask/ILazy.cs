namespace LazyTask
{
    /// <summary>
    /// Интерфейс, представляющий вычисление, совершаемое один раз
    /// </summary>
    /// <typeparam name="T">Тип вычисляемого значения</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Возвращает вычисляемое значение
        /// </summary>
        /// <returns>Вычисляемое значение</returns>
        T Get();
    }
}
