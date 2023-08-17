using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    [Serializable]
    public struct FunctionSystem<T,T1> where T1 : IFunction<T> where T : struct
    {
        private T _buffer;

        [SerializeReference]
        private List<T1> _functions;

        public T Process(T data)
        {
            _buffer = data;
            foreach (var function in _functions)
                _buffer = function.Process(_buffer);
            return _buffer;
        }

        public void AddFunction(T1 func)
        {
            _functions ??= new List<T1>();
            _functions.Add(func);
        }
    }

    public interface IFunction<T> where T : struct
    {
        public T Process(T data);
    }
}



