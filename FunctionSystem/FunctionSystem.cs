using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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
    
    [Serializable]
    public struct SimpleFunction : IFunction<int>
    {
        public int Process(int data)
        {
            return data + 1;
        }
    }

    [Serializable]
    public struct PollFunction : IFunction<int>
    {
        public bool canAdd;
        public int Process(int data)
        {
            return canAdd ? data + 1 : data;
        }
    }
    
    [Serializable]
    public class SetToOne : IFunction<int>
    {
        public Button button;
        public bool isClicked;
        
        public SetToOne(Button button)
        {
            this.button = button;
            button.OnClickAsObservable().Subscribe(_ => isClicked = true);
        }
        public int Process(int data)
        {
            if (!isClicked) return data;
            isClicked = false;
            return 1;
        }
    }
}



