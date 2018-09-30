using System;
namespace LazyTask
{
    /// <summary>
    /// Простой Lazy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleLazy<T> : ILazy<T>
    {
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

        public SimpleLazy(Func<T> f)
        {
            supplier = f;
            isInitialized = false;
        }

        private T lazyObject;
        private Func<T> supplier;

        private bool isInitialized;
    }
}
