using System;

namespace ThreadPoolTask
{
    /// <summary>
    /// Интерфейс, представляющий задачу
    /// </summary>
    /// <typeparam name="TResult">Тип возвращаемого функцией-вычислителем значения</typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Вычисляемое значение
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Завершён ли таск
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Принимает объект типа Func, который может быть применен к результату данной задачи X и возвращает новую задачу Y, принятую к исполнению
        /// </summary>
        /// <typeparam name="TNewResult">Тип нового результата</typeparam>
        /// <param name="newTask">Новая задача</param>
        /// <returns></returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask);
    }
}
