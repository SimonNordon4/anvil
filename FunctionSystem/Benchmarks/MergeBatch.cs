using System;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Anvil.Benchmarks
{
    public class MergeBatch : MonoBehaviour
    {
        
        public int numberOfObservables = 100;
        private Stopwatch _stopwatch = new();
        
        private CompositeDisposable _disposable = new();
        
        public ReactiveProperty<int> firstObservable = new();
        
        public void Start()
        {
            CreateBatched();
        }

        public void CreateBatched()
        {
            firstObservable
                .Merge(CreateObservables(numberOfObservables))
                .BatchFrame(0,FrameCountType.Update)
                .Subscribe(v =>
                {
                    _stopwatch.Stop();
                    Debug.Log($"Elapsed time: {_stopwatch.ElapsedMilliseconds}ms");
                }).AddTo(this);
        }

        public void SetFirst()
        {
            _stopwatch.Restart();
            firstObservable.Value = Random.Range(0, 100);
        }

        private IObservable<int>[] CreateObservables(int number)
        {
            var observables = new IObservable<int>[number];
            var returnRandom = Random.Range(0, 100);
            for (var i = 0; i < number; i++)
            {
                observables[i] = Observable.Return(returnRandom);
            }

            return observables; 
        }
    }
    
    // create custom inspector for this class
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MergeBatch))]
    public class MergeBatchEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (MergeBatch) target;
            if (GUILayout.Button("Set First"))
            {
                script.SetFirst();
            }
            if (GUILayout.Button("Create Batched"))
            {
                script.CreateBatched();
            }
        }
    }
    #endif
}