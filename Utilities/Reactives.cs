using System;
using System.Collections;
using System.Collections.Generic;

namespace NorskaLib.Utilities
{
    public class ReactiveReference<C> where C : class
    {
        private C value;

        /// <summary>
        /// NOTE: Values are compared using '==' operator.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public C Value
        {
            get => value;

            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    onChanged?.Invoke(value);
                }
                else
                {
                    this.value = value;
                }
            }
        }

        public Action<C> onChanged;

        public ReactiveReference()
        {
            value = null;
        }

        public ReactiveReference(C initial)
        {
            value = initial;
        }
    }

    public class ReactiveValue<V>
    {
        private V value;

        /// <summary>
        /// NOTE: Values are compared using 'EqualityComparer<V>.Default.Equals()' method.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public V Value
        {
            get => value;

            set
            {
                if (!EqualityComparer<V>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    onChanged?.Invoke(value);
                }
                else
                {
                    this.value = value;
                }
            }
        }

        public Action<V> onChanged;

        public ReactiveValue()
        {
            value = default(V);
        }

        public ReactiveValue(V initial)
        {
            value = initial;
        }
    }
}