using System;

namespace ItHappened.Bll.Domain
{
    
    //Optional only for domain
    //Use null for infrastructure
    public class Optional<T>
    {
        public static Optional<T> Some(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new Optional<T>( true)
            {
                value = value
            };
        }
        
        public static Optional<T> None()
        {
            return new Optional<T>( false);
        }

        public void Do(Action<T> action)
        {
            if (HasValue)
            {
                action(value);
            }
        }

        public Optional<TOut> Map<TOut>(Func<T, TOut> map)
        {
            if (!HasValue)
            {
                return Optional<TOut>.None();
            }
            
            var output = map(value);
            return output == null ? Optional<TOut>.None() : Optional<TOut>.Some(output);
        }

        public bool HasValue { get; }
        
        private T value;
        
        private Optional(bool hasValue)
        {
            HasValue = hasValue;
        }
    }
}