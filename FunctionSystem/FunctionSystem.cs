using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Anvil
{
    [Serializable]
    public struct FunctionSystem<T,T1> where T1 : IFunction<T> where T : struct
    {
        private T _buffer;

        [SerializeReference]
        public List<T1> functions;

        public T Process(T data)
        {
            _buffer = data;
            foreach (var function in functions)
                _buffer = function.Process(_buffer);
            return _buffer;
        }

        public void AddFunction(T1 func)
        {
            functions ??= new List<T1>();
            functions.Add(func);
        }
    }

    public interface IFunction<T> where T : struct
    {
        public T Process(T data);
    }
}



