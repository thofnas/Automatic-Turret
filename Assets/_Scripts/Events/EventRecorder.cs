﻿using System;

namespace Events
{
        public class EventRecorder
        {
            private event Action CustomAction = delegate { };

            public void Invoke() => CustomAction?.Invoke();

            public void AddListener(Action listener) => CustomAction += listener;

            public void RemoveListener(Action listener) => CustomAction -= listener;
        }

        public class EventRecorder<T>
        {
            private event Action<T> CustomAction = delegate { };

            public void Invoke(T param) => CustomAction?.Invoke(param);

            public void AddListener(Action<T> listener) => CustomAction += listener;

            public void RemoveListener(Action<T> listener) => CustomAction -= listener;
        }
    }

