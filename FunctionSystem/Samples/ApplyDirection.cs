using System;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class ApplyDirection : MonoBehaviour
    {
        //public ReactiveProperty<Vector3> wishDirection  = new();
        public FunctionSystem<Vector3,IWishDirectionFunction> functionSystem  = new();

        private void Update()
        {
            var direction = functionSystem.Process(Vector3.zero);
            transform.position += direction;
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
        public Vector3 Process(Vector3 data) => data * moveSpeed;
    }

    [Serializable]
    public struct ApplyDeltaTime : IWishDirectionFunction
    {
        [SerializeField] private float timeScale;
        public Vector3 Process(Vector3 data) => data * (Time.deltaTime * timeScale);
    }

    [Serializable]
    public struct SpeedBoost : IWishDirectionFunction
    {
        [SerializeField] private float boostSpeed;
        public Vector3 Process(Vector3 data) => Input.GetKey(KeyCode.Space) ? data * boostSpeed : data;
    }
}