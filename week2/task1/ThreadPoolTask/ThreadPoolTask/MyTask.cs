using System;
using System.Threading;

namespace ThreadPoolTask
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private TResult result;
        private MyThreadPool threadPool;
        private ManualResetEvent sync;
        private Exception storedException = null;

        public TResult Result
        {
            get
            {
                sync.WaitOne();
                if (storedException != null)
                {
                    throw new AggregateException("Вызов метода завершился исключением", storedException);
                }

                return result;
            }
        }

        public void SetResult(Func<TResult> supplier)
        {
            try
            {
                result = supplier();
            } catch (Exception e)
            {
                storedException = e; 
            }
            
            sync.Set();
            IsCompleted = true;
        }

        public bool IsCompleted { get; private set; }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newSupplier)
        {
            sync.WaitOne();
            TNewResult supplier() => newSupplier(result);
            
            return threadPool.AddTask(supplier);
        }

        public MyTask(MyThreadPool threadPool)
        {
            this.threadPool = threadPool;
            this.IsCompleted = false;
            sync = new ManualResetEvent(false);
        }
    }
}
