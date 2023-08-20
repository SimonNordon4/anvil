using System.Diagnostics;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Anvil.Benchmarks
{
    public class EveryUpdateBenchmark : BenchmarkBase
    {
        public int number;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public Stopwatch stopwatch = new Stopwatch();
        
        private int incrementalNumber = 0;
        private void Start()
        {
            
        }



        private void RegisterEveryUpdate()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (incrementalNumber == 0)
                {
                    stopwatch.Start();
                }
                incrementalNumber++;
                Debug.Log(incrementalNumber);
                if (incrementalNumber >= number)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log($"EveryUpdate Benchmark: {stopwatch.ElapsedMilliseconds}ms");
                }

            }).AddTo(_disposable);   
        }

        public override void Benchmark()
        {
            incrementalNumber = 0;
            // start stop watch
            stopwatch = new Stopwatch();
            _disposable?.Dispose();
            RegisterEveryUpdate();
        }

        public void OnFinished()
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log($"EveryUpdate Benchmark: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}