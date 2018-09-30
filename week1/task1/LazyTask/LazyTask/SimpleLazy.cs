using System;
namespace LazyTask
{
    /// <summary>
    /// Простой Lazy
    /// </summary>
    /// <typeparam name="T">Тип вычисляемого значения</typeparam>
    public class SimpleLazy<T> : ILazy<T>
    {
        private T lazyObject;
        private Func<T> supplier;

        private bool isInitialized;

        public T Get()
        {
            if (!isInitialized)
            {
                lazyObject = supplier();

                isInitialized = true;
                supplier = null;
            }

            return lazyObject;
        }

        /// <summary>
        /// Создаёт SimpleLazy
        /// </summary>
        /// <param name="f"></param>
        public SimpleLazy(Func<T> f)
        {
            supplier = f;
            isInitialized = false;
        }
    }
}
