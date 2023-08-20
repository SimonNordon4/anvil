using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    [Serializable]
    public class ReactiveSystem<T,T1> where T1 : IHandler<T> where T : struct
    {
        [SerializeReference]
        private List<T1> functions;

        private ReactiveProperty<T> _buffer = new();

        public void Bind(IReactiveProperty<T> property, Component component)
        {
            foreach (var function in functions)
                function.Initialize(_buffer);

            _buffer.BatchFrame(0,FrameCountType.Update)
                .Select(v => v.Last())
                .Subscribe(v => property.Value = v)
                .AddTo(component);
        }
    }

    public interface IHandler<T> where T : struct
    {
        public void Initialize(ReactiveProperty<T> bufferProp);
    }
}