﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    [Serializable]
    public class ReactiveSystem<T,T1> : IDisposable where T1 : IHandler<T> where T : struct
    {
        [SerializeReference]
        private List<T1> functions;
        private T _buffer;
        
        private CompositeDisposable _disposable = new();

        public void Bind(IReactiveProperty<T> property, Component component)
        {
            foreach (var function in functions)
                function.Initialize(_disposable);

            Observable.EveryUpdate().Subscribe(_ =>
            {
                _buffer = property.Value;
                foreach (var function in functions)
                    _buffer = function.Process(_buffer);
                property.Value = _buffer;
            }).AddTo(component);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }

    public interface IHandler<T> where T : struct
    {
        public void Initialize(CompositeDisposable system);
        public T Process(T data);
    }
    
   
}