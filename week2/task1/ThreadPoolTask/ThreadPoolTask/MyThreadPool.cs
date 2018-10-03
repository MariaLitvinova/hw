using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPoolTask
{
    /// <summary>
    /// Пул потоков
    /// </summary>
    public class MyThreadPool
    {
        private BlockingCollection<Action> waitingTasks = new BlockingCollection<Action>();

        private List<Thread> threads = new List<Thread>();

        private readonly object locker = new object();

        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private CancellationToken token;
        
        /// <summary>
        /// Конструктор класса MyThreadPool
        /// </summary>
        /// <param name="numberOfTasks">Количество потоков</param>
        public MyThreadPool(int numberOfTasks)
        {
            if (numberOfTasks < 0)
            {
                throw new InvalidOperationException("Number of tasks has to be non-negative number!");
            }
            
            token = cancelTokenSource.Token;
            
            for (int i = 0; i < numberOfTasks; ++i)
            {
                var thread = new Thread(ThreadRoutine);

                threads.Add(thread);
            }
            
            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Логика работы одного потока
        /// </summary>
        private void ThreadRoutine()
        {
            while (true)
            {
                Action task = null;
                if (waitingTasks.Any())
                {
                    task = waitingTasks.Take();
                }
                
                task?.Invoke();

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// Возвращает количество тасков в очереди
        /// </summary>
        /// <returns></returns>
        public int WaitingTasksAmount()
        {
            return waitingTasks.Count;
        }

        /// <summary>
        /// Завершает все потоки
        /// </summary>
        public void Shutdown()
        {
            cancelTokenSource.Cancel();
        }

        /// <summary>
        /// Добавляет новое задание, если есть место; если нет, ждёт
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newTask"></param>
        public IMyTask<T> AddTask<T>(Func<T> supplier)
        {
            if (token.IsCancellationRequested)
            {
                throw new InvalidOperationException("Cannot add new task – thread pool already shut down");
            }

            var task = new MyTask<T>(this);
            waitingTasks.Add(() => task.SetResult(supplier));

            return task;
        }
    }
}
