using System;

namespace _Events
{
        public class EventRegister
        {
            private event Action CustomAction = delegate { };

            public void Invoke() => CustomAction?.Invoke();

            public void AddListener(Action listener) => CustomAction += listener;

            public void RemoveListener(Action listener) => CustomAction -= listener;
        }

        public class EventRegister<T1>
        {
            private event Action<T1> CustomAction = delegate { };

            public void Invoke(T1 param) => CustomAction?.Invoke(param);

            public void AddListener(Action<T1> listener) => CustomAction += listener;

            public void RemoveListener(Action<T1> listener) => CustomAction -= listener;
        }
    }

