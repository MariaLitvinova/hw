namespace LazyTask
{
    /// <summary>
    /// Интерфейс Lazy
    /// </summary>
    /// <typeparam name="T">Тип вычисляемого значения</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Возвращает вычисляемое значение
        /// </summary>
        /// <returns></returns>
        T Get();
    }
}
