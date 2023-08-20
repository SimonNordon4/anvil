using UniRx;
using UnityEngine;

namespace Anvil
{
    public class MoveSystem : MonoBehaviour
    {
        public IntReactiveProperty moveSpeed = new(5);
        public ReactiveSystem<int, IMoveHandler> system = new();

        private void Start()
        {
            system.Bind(moveSpeed,this);
            moveSpeed.Subscribe(_ => Debug.Log($"MoveSpeed: {moveSpeed.Value} occuring on frame {Time.frameCount}"));
        }
    }

    public interface IMoveHandler: IHandler<int>
    {
        
    }
    
    [System.Serializable]
    public struct MoveSystemHandler : IMoveHandler
    {
        [SerializeField] private HealthSystem healthSystem;


        public void Initialize(CompositeDisposable system)
        {
        }

        public int Process(int data)
        {
            return healthSystem.health.Value;
        }
    }
}