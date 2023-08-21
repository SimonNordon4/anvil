using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = System.Object;

namespace Anvil.Systems
{
    [Serializable]
    public class ReactiveSystem<T> where T : struct
    {
        private T _buffer;
        private Subject<Unit> _onUpdate;
        
        [SerializeReference]
        private List<IFunction<T>> _functions;
        
        public void Bind(ReactiveProperty<T> property, Component component)
        {
            foreach (var function in _functions)
            {
                if(function is IHandler<T> handler)
                    handler.Initialize(_onUpdate,component);
            }
        }
        
        private T Process(T data = default)
        {
            _buffer = data;
            foreach (var function in _functions)
                _buffer = function.Process(_buffer);
            return _buffer;
        }
    }
    
    public interface IHandler<T> : IFunction<T> where T : struct
    {
        public void Initialize(Subject<Unit> onUpdate,Component component);
    }
}

// How To Check One Value Change Per.
//         public void Bind(IReactiveProperty<T> property, Component component)
//         {
//             foreach (var function in functions)
//                 function.Initialize(_buffer);
//
//             _buffer.BatchFrame(0,FrameCountType.Update)
//                 .Select(v => v.Last())
//                 .Subscribe(v => property.Value = v)
//                 .AddTo(component);
//         }