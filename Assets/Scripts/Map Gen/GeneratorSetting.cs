using UnityEngine;

namespace Map_Gen
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/GeneratorSetting")]

    public class GeneratorSetting : ScriptableObject
    {
        // size of terrain
        public int width = 256;
        public int height = 256;
        
        // starting area parameters
        public int startRadius = 10;
        public int startPow = 3; // aggressiveness of flattening

        // terrain parameters
        public int depth = 10;

        public double scale = 5;
        
        // fractal parameters
        public int octaves = 4;
        public float persistence = 0.5f; // should usually be 0 to 1, but maybe you want to be fancy
        public float lacunarity = 2f;

        public Vector2 offset = Vector2.zero;

        // zero seed = random.
        public int seed = 0;

        // Todo do we want the default to be ease in out or just linear? hmm.
        public AnimationCurve depthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        // Here are the settings for spawnpoints
        public int maximumSpawnRadius = 16;

        //public int spawnPoints = 16;
        public float exclusionRadius = 3; // distance from edge of grid squares spawns will occur in

        public float maximumTolerableGradient = 0.5f; // maximum surface steepness a spawn point can occur on
        
        // Settings for objects to spawn
        public SpawnableObject[] objectSelection; // list of objects which can spawn
        //public float[] spawnProbabilities; // chance of a type spawning TODO do we need this?
        public int[] spawnLimits; // max instances of a type
    }
}
