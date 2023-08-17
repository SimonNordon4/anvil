using System;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class ApplyWishDirection : MonoBehaviour
    {
        //public ReactiveProperty<Vector3> wishDirection  = new();
        public FunctionSystem<Vector3,IWishDirectionFunction> functionSystem  = new();
        private void Start()
        {
            // Observable
            //     .EveryUpdate()
            //     .Subscribe(_ => wishDirection.Value = functionSystem.Process(wishDirection.Value)).AddTo(this);
        }
    }
    
    public interface IWishDirectionFunction : IFunction<Vector3>
    {
    }
    
    [Serializable]
    public struct ApplyWasd : IWishDirectionFunction
    {
        public Vector3 Process(Vector3 data)
        {
            var direction = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                direction += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                direction += Vector3.back;
            if (Input.GetKey(KeyCode.A))
                direction += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                direction += Vector3.right;

            return direction.normalized;
        }
    }

    [Serializable]
    public struct ApplyMoveSpeed : IWishDirectionFunction
    {
        [SerializeField] private int moveSpeed;
        
        public Vector3 Process(Vector3 data)
        {
            return data * moveSpeed;
        }
    }

    [Serializable]
    public struct ApplyDeltaTime : IWishDirectionFunction
    {
        public Vector3 Process(Vector3 data)
        {
            return data * Time.deltaTime;
        }
    }

    [Serializable]
    public struct ResetOnClick : IWishDirectionFunction
    {
        [SerializeField] private Button button;
        private bool _isClicked;

        public void Start()
        {
            _isClicked = false;
            var x = this;
            //button.OnClickAsObservable().Subscribe(_ => x._isClicked = true);
        }
        
        public Vector3 Process(Vector3 data)
        {
            return _isClicked ? Vector3.zero : data;
        }
    }
}