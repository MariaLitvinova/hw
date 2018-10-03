using System;

namespace LazyTask
{
    /// <summary>
    /// Многопоточный Lazy
    /// </summary>
    /// <typeparam name="T">тип вычисляемого значения</typeparam>
    public class MultithreadSafeLazy<T> : ILazy<T>
    {
        private T lazyObject;
        private Func<T> supplier;

        private volatile bool isInitialized;

        private readonly object locker = new object();

        public T Get()
        {
            if (!isInitialized)
            {
                lock (locker)
                {
                    if (!isInitialized)
                    {
                        lazyObject = supplier();

                        supplier = null;
                        isInitialized = true;
                    }
                }
            }

            return lazyObject;
        }

        /// <summary>
        /// Создаёт многопоточный Lazy
        /// </summary>
        /// <param name="f">Функция для вычисления значения</param>
        public MultithreadSafeLazy(Func<T> f)
        {
            supplier = f;
            isInitialized = false;
        }
    }
}
