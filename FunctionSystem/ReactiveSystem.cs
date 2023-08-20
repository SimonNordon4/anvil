using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Anvil
{
    [Serializable]
    public class ReactiveSystem<T,T1> where T1 : IHandler<T> where T : struct
    {
        private T _buffer;
        
        private IReactiveProperty<T> _property;

        [SerializeReference]
        private List<T1> functions;
        
        private IHandler<T> EndFunction { get; set; }

        public void Bind(IReactiveProperty<T> property)
        {
            _property = property;
            var safeCastSystem = this as ReactiveSystem<T, IHandler<T>>;
            foreach (var function in functions)
                function.Initialize(safeCastSystem);
        }

        private void Evaluate()
        {
            _buffer = default;
            foreach (var function in functions)
                _buffer = function.Process(_buffer);
            _property.Value = _buffer;
        }

        public void AddFunction(T1 func)
        {
            functions ??= new List<T1>();
            functions.Add(func);
        }
        
        public void Notify(T1 func)
        {
            
        }
    }

    public interface IHandler<T> where T : struct
    {
        public void Initialize(ReactiveSystem<T,IHandler<T>> system);
        public T Process(T data);
    }
    
    // Class A Frame 1 -> Evaluate
    // Class B Frame 1 -> Evaluate
    // This will set the value twice, very bad!
    
    // Instead we somehow need to wait until all classes have called Evaluate, then we can set the value.
    // However it's unknown if any classes will ever call Evaluate() on that frame.


}