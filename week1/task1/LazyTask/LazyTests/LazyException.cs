using System;

namespace LazyTests
{
    /// <summary>
    /// Исключение для тестирования Lazy
    /// </summary>
    [Serializable]
    public class LazyException : Exception
    {
        public LazyException()
        {
        }

        public LazyException(string message) : base(message)
        {
        }

        public LazyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LazyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
