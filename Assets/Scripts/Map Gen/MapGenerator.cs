using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map_Gen
{
    public class MapGenerator : MonoBehaviour
    {
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
        void Start()
        {
            terrain = GetComponent<Terrain>();

            mapObjectParent = new GameObject("Map Objects");
            
            spawnLocations = new List<Vector3>();
            
            seed = settings.seed == 0 ? new Random().Next() : settings.seed;
            rng = new Random(seed);

            numRequiredSpawns = settings.spawnLimits.Sum();

            globalOffset = new Vector3(settings.width / 2f, 0, settings.height / 2f);
            transform.localPosition -= globalOffset;
            
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            terrain.terrainData = GenerateStartingArea(terrain.terrainData);
            GenerateSpawnLocations();
            //IllustrateSpawnLocationsDebug();
            PopulateSpawnLocations();
        }

        // Update is called once per frame
        // TODO move all here in this from update to start. It's only in update to debug parameters.
        void Update()
        {
            
        }

        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            //terrainData.heightmapResolution = settings.width;
            terrainData.size = new Vector3(settings.width, settings.depth, settings.height);

            float[,] heights = GenerateAndTuneHeights();
            
            terrainData.SetHeights(0, 0, heights);
            return terrainData;
        }

        void GenerateSpawnLocations()
        {
            List<Vector3> potentialSpawnLocations = new List<Vector3>();
            Vector2 circleCenter = new Vector2(settings.width / 2f, settings.height / 2f);

            // loop through each terrain point and check if it could be used as a spawn location
            // grab the gradient map:
            var gradients = NoiseGenerators.GradientMap(
                terrain.terrainData.GetHeights(0, 0, settings.width, settings.height), settings.depth, settings.width,
                settings.height, 1);
            
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    var point = new Vector2(x, y);
                    var distance = Vector2.Distance(point, circleCenter);

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
