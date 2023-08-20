using System;
using UniRx;
using UnityEngine;

namespace Anvil
{
    public class ExamplePosition : MonoBehaviour
    {
        public Vector3ReactiveProperty position = new();
        public ReactiveSystem<Vector3, IPositionHandler> system = new();

        private void Start()
        {
            system.Bind(position, this);
        }
    }

    public interface IPositionHandler : IHandler<Vector3>
    {
    }
    
    public class ExamplePositionHandler : IPositionHandler
    {
        public void Initialize(ReactiveProperty<Vector3> bufferProp)
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => bufferProp.Value = Vector3.one);
        }
    }
}