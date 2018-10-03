using System;

namespace LazyTask
{
    /// <summary>
    /// Фабрика для создания Lazy
    /// </summary>
    public static class LazyFactory
    {
        /// <summary>
        /// Создаёт простой Lazy
        /// </summary>
        /// <typeparam name="T">Тип хранимого значения</typeparam>
        /// <param name="supplier">Функция для вычисления значения</param>
        /// <returns>простой Lazy</returns>
        public static SimpleLazy<T> CreateSimpleLazy<T>(Func<T> supplier) 
            => new SimpleLazy<T>(supplier);

        /// <summary>
        /// Создаёт многопоточный Lazy
        /// </summary>
        /// <typeparam name="T">Тип хранимого значения</typeparam>
        /// <param name="supplier">Функция для вычисления значения</param>
        /// <returns>многопоточный Lazy</returns>
        public static MultithreadSafeLazy<T> CreateMultithreadLazy<T>(Func<T> supplier)
            => new MultithreadSafeLazy<T>(supplier);
    }
}
