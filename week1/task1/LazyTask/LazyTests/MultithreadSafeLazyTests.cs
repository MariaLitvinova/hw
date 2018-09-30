using System;
using System.Threading;
using LazyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyTests
{
    [TestClass]
    public class MultithreadSafeLazyTests
    {
        /// <summary>
        /// Проверка, что Lazy в принципе инициализируется
        /// </summary>
        [TestMethod]
        public void CheckThatLazyStoresPassedFunction()
        {
            var simpleLazy = LazyFactory.CreateMultithreadLazy(() => { return 42; });
            var lazyResult = simpleLazy.Get();

            Assert.AreEqual(42, lazyResult);
        }       /// <summary>
                /// Проверка, что Lazy инициализируется лишь один раз
                /// </summary>
        [TestMethod]
        public void CheckThatLazyInitializesOnlyOnce()
        {
            int counter = 0;

            var simpleLazy = LazyFactory.CreateMultithreadLazy(() => { counter++; return 42; });

            for (int i = 0; i < 10; ++i)
            {
                simpleLazy.Get();
            }

            Assert.AreEqual(1, counter);
        }

        /// <summary>
        /// Проверка, что Lazy работает с null
        /// </summary>
        [TestMethod]
        public void CheckThatLazyWorksWithNull()
        {
            var simpleLazy = LazyFactory.CreateMultithreadLazy<object>(() => { return null; });

            var lazyResult = simpleLazy.Get();

            Assert.AreEqual(null, lazyResult);
        }

        /// <summary>
        /// Проверка, что Lazy работает с исключением
        /// </summary>
        [TestMethod]
        public void CheckThatLazyWorksWithException()
        {
            var simpleLazy = LazyFactory.CreateMultithreadLazy<object>(() => throw new LazyException());

            Assert.ThrowsException<LazyException>(() => simpleLazy.Get());
        }

        /// <summary>
        /// Проверка, что Lazy работает для нескольких потоков
        /// </summary>
        [TestMethod]
        public void CheckThatLazyWorksWithMultipleThreads()
        {
            var threads = new Thread[3];
            int counter = 0;

            var lazy = LazyFactory.CreateMultithreadLazy(() => { counter++; return 42; });

            for (var i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    lazy.Get();
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(1, counter);
        }
    }
}
