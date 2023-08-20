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
    
    public interface IHealthHandler : IHandler<int>
    {
        
    }
    
    [Serializable]
    public class ButtonToOne : IHealthHandler
    {
        [SerializeField]private Button button;
        private bool _isPressed;
        
        public void Initialize(CompositeDisposable system)
        {
            button.OnClickAsObservable().Subscribe(_ => _isPressed = true).AddTo(system);
        }

        public void Initialize()
        {
        }

        public int Process(int data)
        {
            if (!_isPressed) return data;
            _isPressed = false;
            return 1;
        }
    }
}

