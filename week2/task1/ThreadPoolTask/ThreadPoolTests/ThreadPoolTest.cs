using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using ThreadPoolTask;

namespace ThreadPoolTests
{
    [TestClass]
    public class ThreadPoolTest
    {
        private int SuperLongFunction()
        {
            var result = 0;
            for (int i = 0; i < 1000000000; ++i)
            {
                result += 1;
            }

            Thread.Sleep(10000);
            return result;
        }

        private int SuperShortFunction()
        {
            var result = 0;
            for (int i = 0; i < 100; ++i)
            {
                result += 1;
            }
            return result;
        }

        private int ModifyPreviousResultFunction(int x)
        {
            for (int i = 0; i < 1000000000; ++i)
            {
                x += 1;
            }

            return x;
        }

        [TestMethod]
        public void CheckThatValuesAreCalculated()
        {
            var threadPool = new MyThreadPool(3);

            var expectedResult = SuperLongFunction();

            var task1 = threadPool.AddTask(SuperLongFunction);
            var task2 = threadPool.AddTask(SuperLongFunction);
            var task3 = threadPool.AddTask(SuperLongFunction);

            Assert.AreEqual(expectedResult, task1.Result);
            Assert.AreEqual(expectedResult, task2.Result);
            Assert.AreEqual(expectedResult, task3.Result);
        }

        [TestMethod]
        public void CheckThatEmptyPoolDoesNotComputeTask()
        {
            var threadPool = new MyThreadPool(0);

            var task = threadPool.AddTask(SuperLongFunction);

            Assert.AreEqual(1, threadPool.WaitingTasksAmount());
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void CheckThatExactlyGivenAmountOfThreadsRun()
        {
            var threadPool = new MyThreadPool(3);
            var longTasks = new List<IMyTask<int>>();
            for (int i = 0; i < 3; ++i)
            {
                longTasks.Add(threadPool.AddTask(SuperLongFunction));
            }

            var shortTasks = new List<IMyTask<int>>();
            for (int i = 0; i < 3; ++i)
            {
                shortTasks.Add(threadPool.AddTask(SuperShortFunction));
            }

            shortTasks.ForEach(x => Assert.IsFalse(x.IsCompleted));
        }

        [TestMethod]
        public void CheckThatResultThrowsException()
        {
            var threadPool = new MyThreadPool(1);

            var task = threadPool.AddTask<object>(() => throw new TestException());

            while (!task.IsCompleted) { }
            
            Assert.ThrowsException<AggregateException>(() => task.Result);
        }

        [TestMethod]
        public void CheckThatAllTasksAddedToQueueWillBeExecuted()
        {
            var threadPool = new MyThreadPool(2);
            var expectedResult = SuperShortFunction();

            var executedTasks = new List<IMyTask<int>>();
            for (int i = 0; i < 2; ++i)
            {
                executedTasks.Add(threadPool.AddTask(SuperShortFunction));
            }

            threadPool.Shutdown();

            Assert.AreEqual(expectedResult, executedTasks[0].Result);
            Assert.AreEqual(expectedResult, executedTasks[1].Result);

            Assert.IsTrue(executedTasks[0].IsCompleted);
            Assert.IsTrue(executedTasks[1].IsCompleted);

            Assert.ThrowsException<InvalidOperationException>(() => threadPool.AddTask(SuperLongFunction));
        }

        [TestMethod]
        public void CheckContinueWith()
        {
            var threadPool = new MyThreadPool(2);

            var task1 = threadPool.AddTask(SuperLongFunction);
            var task2 = task1.ContinueWith(ModifyPreviousResultFunction);
            var task3 = task2.ContinueWith(ModifyPreviousResultFunction);

            while (!task1.IsCompleted)
            {
                Assert.IsFalse(task2.IsCompleted);
            }

            while (!task2.IsCompleted)
            {
                Assert.IsFalse(task3.IsCompleted);
            }
        }
    }
}
