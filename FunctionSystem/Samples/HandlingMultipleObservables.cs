using System.Linq;
using UniRx;
using UnityEngine;

namespace Anvil
{
    public class HandlingMultipleObservables : MonoBehaviour
    {
        public ReactiveProperty<int> value1 = new();
        public ReactiveProperty<int> value2 = new();
        public ReactiveProperty<int> value3 = new();
        public ReactiveProperty<int> value4 = new();

        public ReactiveProperty<int> result = new();
        private void Start()
        {
            // I want it such that result is only ever set the last value sent of the 4 observables.
            // The 4 observables are not necessarily sent at the same time.
            // The 4 observables are not necessarily sent in the same order.
            // The 4 observables are not necessarily sent at the same frequency.
            
            // I will use the syntax of V1:true means a value was for value1 this frame, false means no value was sent for value1 this frame.
            
            // Case 1: v1:true, v2:false, v3:false, v4:false -> result = v1
            // Case 2: v1:false, v2:true, v3:false, v4:false -> result = v2
            // Case 3: v1:true, v2:false, v3:true, v4:false -> result = v3
            // Case 4: v1:true, v2:true, v3:true, v4:true -> result = v4
            
            Observable.Merge(
                    value1.AsObservable(),
                    value2.AsObservable(),
                    value3.AsObservable(),
                    value4.AsObservable())
                .BatchFrame(0, FrameCountType.Update)
                .Subscribe(v =>
                {
                    Debug.Log($"Received {v.Count} values on frame {Time.frameCount}");
                    result.Value = v.Last();
                }); 
                result.Subscribe(v => Debug.Log($"Result: {v} on frame {Time.frameCount}"));
        }

        public void SetRandom()
        {
            var set = Random.Range(0, 4);
            // switch statement
            switch (set)
            {
                case 0:
                    value1.Value = Random.Range(0, 100);
                    break;
                case 1:
                    value2.Value = Random.Range(0, 100);
                    break;
                case 2:
                    value3.Value = Random.Range(0, 100);
                    break;
                case 3:
                    value4.Value = Random.Range(0, 100);
                    break;
            }
        }

        public void SetTwo()
        {
            Debug.Log("Setting two values on frame " + Time.frameCount);
            value1.Value = Random.Range(0, 100);
            value3.Value = Random.Range(0, 100);
        }
    }
    
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(HandlingMultipleObservables))]
    public class HandlingMultipleObservablesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = (HandlingMultipleObservables) target;
            if (GUILayout.Button("Set Random"))
                t.SetRandom();
            if (GUILayout.Button("Set Two"))
                t.SetTwo();
        }
    }
    #endif
}