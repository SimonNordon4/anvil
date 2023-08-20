using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class HealthSystem : MonoBehaviour
    {
        public IntReactiveProperty health = new(5);
        public FunctionSystem<int,IIntHandler> system = new();


        private void Start()
        {
            foreach (var func in system.functions)
            {
                func.Init();
            }
            health.Subscribe(_=> Debug.Log($"Health: {health.Value}"));
        }

        private void Update()
        {
            health.Value = system.Process(health.Value);
        }
    }

    public interface IIntHandler : IFunction<int>
    {
        public void Init();
    }

    [Serializable]
    public class SetOne : IIntHandler
    {
        [SerializeField]
        public Button button;
        private bool _isProc;

        public void Init()
        {
            button.OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log("SetOne");
                _isProc = true;
            });
        }

        public int Process(int data)
        {
            if (!_isProc) return data;
            
            _isProc = false;
            return 1;
        }
    }
    
    [Serializable]
    public struct SetZero : IIntHandler
    {
        [SerializeField]
        public Button button;
        private bool _isProc;

        public void Init()
        {
            var one = this;
            button.OnClickAsObservable().Subscribe(_ => one.Proc());
        }

        public void Proc()
        {
            _isProc = true;
        }

        public int Process(int data)
        {
            if (!_isProc) return data;
            
            _isProc = false;
            return 0;
        }
    }
}

