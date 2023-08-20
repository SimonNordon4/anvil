using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class HealthSystem : MonoBehaviour
    {
        public IntReactiveProperty health = new(5);
        public ReactiveSystem<int, IHealthHandler> system = new();

        private void Start()
        {
            system.Bind(health,this);
            health.Subscribe(_ => Debug.Log($"Health: {health.Value} occuring on frame {Time.frameCount}"));
        }
    }
}

