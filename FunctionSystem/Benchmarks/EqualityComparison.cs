using UnityEngine;
using System.Diagnostics;

namespace Anvil.Benchmarks
{
    public class EqualityComparison : BenchmarkBase
    {
        public int comparisonCount = 10;
        public override void Benchmark()
        {
            // create a stopwatch
            var stopwatch = new Stopwatch();
            // start the stopwatch
            stopwatch.Start();
            for(int i = 0; i < comparisonCount; i++)
            {
                // do the comparison
                var result = 1 == 2;
            }
            // stop the stopwatch
            stopwatch.Stop();
            // print the elapsed time
            UnityEngine.Debug.Log($"Equality Comparison: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
    public abstract class BenchmarkBase : MonoBehaviour
    {
        public abstract void Benchmark();

    }
    

    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BenchmarkBase), true, isFallback = true)]
    public class EqualityComparisonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (BenchmarkBase) target;
            if (GUILayout.Button("Check"))
            {
                script.Benchmark();
            }
        }
    }
#endif
}

