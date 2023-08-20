using UnityEngine;
using System.Diagnostics;

namespace Anvil.Benchmarks
{
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

