using System;

namespace LazyTask
{
    /// <summary>
    /// Многопоточный Lazy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultithreadSafeLazy<T> : ILazy<T>
    {
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

        public MultithreadSafeLazy(Func<T> f)
        {
            supplier = f;
            isInitialized = false;
        }

        private T lazyObject;
        private Func<T> supplier;

        private volatile bool isInitialized;

        private readonly object locker = new object();
    }
}
