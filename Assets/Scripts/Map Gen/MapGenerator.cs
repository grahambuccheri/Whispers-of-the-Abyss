using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using Random = System.Random;

namespace Map_Gen
{
    // What is this class?
    // This class is the wrapper that does all things related to map generation.
    // It uses an underlying fractal perlin noise function to produce a heightmap,
    // which it then assigns to a terrain. (Side note, this script must be attached to a terrain to properly function)
    // It also handles random and distributed spawning of a variety of gameobjects.
    // Terrain generation settings as well as object instantiation settings and selection can be changed via the given 
    // 'GeneratorSetting' scriptable object. This allows for multiple map generation presets.
    //
    // As a grading note I think this script and its associated dependencies are worthy of adding algorithmic points.
    // The use of fractal perlin generation is non-trivial, and this script in general represents a fairly scalable approach
    // to procedural terrain generation and object placement. It is imperfect in many ways but as a first attempt at this system,
    // it provides a lot of utility.
    public class MapGenerator : MonoBehaviour
    {
        public bool debugMode = false;
        public bool animateTerrain = false;
        
        [SerializeField] private GameObject debugMarker;
        public GeneratorSetting settings;
        
        private int seed;
        private Terrain terrain;
        private GameObject mapObjectParent;

        private List<Vector3> spawnLocations;
        private Vector3 globalOffset;
        private int numSpawns = 0;

        private int numRequiredSpawns;
        
        private Random rng;

        private GameObject approachWarningColliderHolder;
        private GameObject levelExitColliderHolder;
        private CapsuleCollider approachWarningCollider;
        private CapsuleCollider levelExitCollider;

        void Start()
        {
            LoadMap();
        }
        
        // this does all the map gen when called. produces a terrain out of the given terrain object.
        // populates map objects via given scriptable setting object.
        public void LoadMap() // should some of this still happen on start?
        {
            // most of this is just basic initialization.
            terrain = GetComponent<Terrain>();

            // hierarchy cleanup
            mapObjectParent = new GameObject("Map Objects");
            mapObjectParent.transform.SetParent(this.transform);
            mapObjectParent.transform.localPosition = new Vector3(mapObjectParent.transform.localPosition.x, 0,
                mapObjectParent.transform.localPosition.z);
            
            spawnLocations = new List<Vector3>();
            
            seed = settings.seed == 0 ? new Random().Next() : settings.seed;
            rng = new Random(seed);

            numRequiredSpawns = settings.spawnLimits.Sum();

            // center our terrain around the origin
            globalOffset = new Vector3(settings.width / 2f, 0, settings.height / 2f);
            transform.localPosition -= globalOffset;
            
            // this is the real processing, the below function calls create the terrain and populate it.
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            terrain.terrainData = GenerateStartingArea(terrain.terrainData);
            GenerateSpawnLocations(); 
            //IllustrateSpawnLocationsDebug();
            if (!debugMode)
            {
                PopulateSpawnLocations();
            }
            
            // we need to set the boundary triggers. We will have two. One for the warning, and one for the level leave.
            // create holders
            approachWarningColliderHolder = new GameObject("approach warning boundary");
            levelExitColliderHolder = new GameObject("level boundary");
            // set appropriate tags
            approachWarningColliderHolder.tag = "LevelExitWarn";
            levelExitColliderHolder.tag = "LevelExit";
            // add colliders, and set radius dynamically
            approachWarningCollider = approachWarningColliderHolder.AddComponent<CapsuleCollider>();
            levelExitCollider = levelExitColliderHolder.AddComponent<CapsuleCollider>();

            approachWarningCollider.isTrigger = true;
            levelExitCollider.isTrigger = true;
            
            approachWarningCollider.radius = settings.width / 2f - 30;
            levelExitCollider.radius = settings.width / 2f - 15;

            approachWarningCollider.height = 400;
            levelExitCollider.height = 400;
            
        }

        // Update is called once per frame
        // TODO move all here in this from update to start. It's only in update to debug parameters.
        void Update()
        {
            if (debugMode)
            {
                terrain.terrainData = GenerateTerrain(terrain.terrainData);
                terrain.terrainData = GenerateStartingArea(terrain.terrainData);
                if (animateTerrain)
                {
                    settings.offset += new Vector2(2 * Time.deltaTime, 2 * Time.deltaTime); //this animates the terrain, Just for fun lol.
                }
            }
        }

        // returns a TerrainData object with the procedurally generated terrain.
        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = settings.width;
            terrainData.size = new Vector3(settings.width, settings.depth, settings.height);

            float[,] heights = GenerateAndTuneHeights();
            
            terrainData.SetHeights(0, 0, heights);
            return terrainData;
        }

        // finds a set of valid spawn locations as per the settings given.
        void GenerateSpawnLocations()
        {
            List<Vector3> potentialSpawnLocations = new List<Vector3>();
            Vector2 circleCenter = new Vector2(settings.width / 2f, settings.height / 2f);

            // loop through each terrain point and check if it could be used as a spawn location
            // grab the gradient map:
            var gradients = NoiseGenerators.GradientMap(
                terrain.terrainData.GetHeights(0, 0, settings.width, settings.height), settings.depth, settings.width,
                settings.height, 3);
            
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    var point = new Vector2(x, y);
                    var distance = Vector2.Distance(point, circleCenter);

                    //Debug.Log("Gradient at X:" + x + "  at Y:" + y + "  gradient: " + gradients[x, y]);
                    
                    //var gradientAtPoint =
                    //    terrain.terrainData.GetSteepness((float)settings.width / x, (float)settings.height / y);
                    // select only points outside the spawn circle, inside the max range circle, and with satisfactory gradient.
                    if (distance > settings.startRadius && distance <= settings.maximumSpawnRadius &&
                        gradients[x, y] <= settings.maximumTolerableGradient)
                    {
                        var newLocation = new Vector3(x, 0, y) - globalOffset;
                        newLocation.y = terrain.SampleHeight(newLocation);
                        potentialSpawnLocations.Add(newLocation);
                    }
                }
            }
            // now we have all potential spawn locations based on our restrictions. Time to pick randomly.
            while (numSpawns < numRequiredSpawns && potentialSpawnLocations.Count != 0)
            {
                // repeat until desired number of spawns or potential locations is empty
                var randomIndex = rng.Next(0, potentialSpawnLocations.Count);
                var potentialLocation = potentialSpawnLocations[randomIndex];
                potentialSpawnLocations.RemoveAt(randomIndex);

                var locationValid = spawnLocations.Count == 0;
                if (!locationValid)
                {
                    locationValid = true;
                    var potentialLocation2d = new Vector2(potentialLocation.x, potentialLocation.z);
                    foreach (Vector3 location in spawnLocations)
                    {
                        var location2d = new Vector2(location.x, location.z);
                        var proximity = Vector2.Distance(location2d, potentialLocation2d);
                        if (proximity < settings.exclusionRadius)
                        {
                            locationValid = false;
                            break;
                        }
                    }
                }
                if (locationValid)
                {
                    // we have chosen to accept this location. add it to the real list.
                    spawnLocations.Add(potentialLocation);
                    numSpawns += 1;
                }
            }
        }

        // fills spawn locations with objects as per settings.
        void PopulateSpawnLocations()
        {
            var typeCounts = new int[settings.objectSelection.Length];

            while (spawnLocations.Count > 0)
            {
                // pick a random location from the remaining set
                var locationIndex = rng.Next(0, spawnLocations.Count);
                var location = spawnLocations[locationIndex];
                
                // pick a random type to populate with
                var type = rng.Next(0, settings.objectSelection.Length);
                
                // if this type is available, populate the location and remove it from the set.
                if (typeCounts[type] < settings.spawnLimits[type])
                {
                    var spawnable = settings.objectSelection[type];
                    // we still have availability on this type, spawn one!
                    typeCounts[type] += 1;
                    var orientation = new Vector3(rng.Next(0, spawnable.xzVariance), rng.Next(0, 360), rng.Next(0, spawnable.xzVariance));

                    spawnable.Spawn(location, orientation, mapObjectParent);
                    
                    spawnLocations.RemoveAt(locationIndex);
                }
            }
        }

        // places a mostly flat circle in the center of the terrain for the submarine to start in.
        TerrainData GenerateStartingArea(TerrainData terrainData)
        {
            Vector2 circleCenter = new Vector2(settings.width / 2f, settings.height / 2f);
            float[,] heights = terrainData.GetHeights(0, 0, settings.width, settings.height);
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    var point = new Vector2(x, y);
                    var distance = Vector2.Distance(point, circleCenter);
                    var radiusProportion = distance / settings.startRadius;
                    if (distance <= settings.startRadius)
                    {
                        heights[x, y] *= Mathf.Pow(radiusProportion, settings.startPow);
                    }
                }
            }
            terrainData.SetHeights(0, 0, heights);
            return terrainData;
        }

        // gets a noise map from NoiseGenerators
        // maps it to the tuning curve.
        float[,] GenerateAndTuneHeights()
        {
            // TODO figure out why I suffer floating point loss!
            float[,] heights = NoiseGenerators.GenerateFractalPerlinMap(seed,
                settings.width,
                settings.height,
                settings.scale,
                settings.offset,
                settings.octaves,
                settings.lacunarity,
                settings.persistence);
            
            // this method for some reason does not suffer floating point loss. TODO.
            // however, it produces much less interesting maps than the fractal.
            // float[,] heights = NoiseGenerators.GeneratePerlinMap(seed,
            //     settings.width,
            //     settings.height,
            //     settings.scale,
            //     settings.offset);

            for(int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    heights[x, y] = settings.depthCurve.Evaluate(heights[x, y]);
                }
            }

            return heights;
        }
        
        void IllustrateSpawnLocationsDebug()
        {
            foreach (Vector3 spawnPoint in spawnLocations)
            {
                Debug.LogError("Vector: " + spawnPoint);
                Instantiate(debugMarker, spawnPoint, Quaternion.Euler(Vector3.zero));
            }
        }
    }
}
