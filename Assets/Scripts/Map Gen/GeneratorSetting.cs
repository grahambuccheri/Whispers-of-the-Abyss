using UnityEngine;

namespace Map_Gen
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/GeneratorSetting")]

    public class GeneratorSetting : ScriptableObject
    {
        public int width = 256;
        public int height = 256;

        public int depth = 10;

        public float scale = 5;
        
        // fractal parameters
        public int octaves = 4;
        public float persistence = 0.5f; // should usually be 0 to 1, but maybe you want to be fancy
        public float lacunarity = 2f;

        public Vector2 offset = Vector2.zero;

        // zero seed = random.
        public int seed = 0;

        // Todo do we want the default to be ease in out or just linear? hmm.
        public AnimationCurve depthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}
