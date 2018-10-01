using System;
using LazyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyTests
{
    [TestClass]
    public class SimpleLazyTests
    {
        /// <summary>
        /// Проверка, что Lazy в принципе инициализируется
        /// </summary>
        [TestMethod]
        public void CheckThatLazyStoresPassedFunction()
        {
            var simpleLazy = LazyFactory.CreateSimpleLazy(() => { return 42; });
            var lazyResult = simpleLazy.Get();

            Assert.AreEqual(42, lazyResult);
        }

        /// <summary>
        /// Проверка, что Lazy инициализируется лишь один раз
        /// </summary>
        [TestMethod]
        public void CheckThatLazyInitializesOnlyOnce()
        {
            int counter = 0;

            var simpleLazy = LazyFactory.CreateSimpleLazy(() => { counter++; return 42; });

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
            var simpleLazy = LazyFactory.CreateSimpleLazy<object>(() => { return null; });

            var lazyResult = simpleLazy.Get();

            Assert.AreEqual(null, lazyResult);
        }

        /// <summary>
        /// Проверка, что Lazy работает с исключением
        /// </summary>
        [TestMethod]
        public void CheckThatLazyWorksWithException()
        {
            var simpleLazy = LazyFactory.CreateSimpleLazy<object>(() => throw new LazyException());

            Assert.ThrowsException<LazyException>(() => simpleLazy.Get());
        }
    }
}
