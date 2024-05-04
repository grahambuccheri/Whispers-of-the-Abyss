using System.Collections.Generic;
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

        private List<Vector3> spawnLocations;
        private Vector3 globalOffset;
        private int numSpawns = 0;
        private Random rng;
        void Start()
        {
            terrain = GetComponent<Terrain>();
            
            spawnLocations = new List<Vector3>();
            
            seed = settings.seed == 0 ? new Random().Next() : settings.seed;
            rng = new Random(seed);

            globalOffset = new Vector3(settings.width / 2f, 0, settings.height / 2f);
            transform.localPosition -= globalOffset;
        }

        // Update is called once per frame
        // TODO move all here in this from update to start. It's only in update to debug parameters.
        void Update()
        {
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            terrain.terrainData = GenerateStartingArea(terrain.terrainData);
            GenerateSpawnLocations();
            IllustrateSpawnLocationsDebug(5f);
            PopulateSpawnLocations();
        }

        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = settings.width + 1;
            terrainData.size = new Vector3(settings.width, settings.depth, settings.height);

            float[,] heights = GenerateAndTuneHeights();
            
            terrainData.SetHeights(0, 0, heights);
            return terrainData;
        }

        void GenerateSpawnLocations()
        {
            // define a grid based on the terrain
            // Eureka, to simplify terrain point checks, we may opt to instead iterate through each TERRAIN DATA point on the heightmap.
            // for each point, check if it is far enough away from the starting radius, and within the spawning radius, AS WELL as satisfying maximal gradient.
            // if it satisfies all that, put it into a list.
            // until we generate the desired number of spawns, or out potential location list is empty,
            // pick a random location from the list. compare its Vector2 distance to all other points in the list (this is slow, I know, but lets just start with naiive for MVP.) (do a pop here, we will consume this value either way)
            // if the distance to all points is AT LEAST the exclusion radius, make it a real spawn point. Otherwise, do nothing (we already popped, so the value is gone)
            // repeat until all locations are selected.
            // OPTIONAL: vary the location to not be precisely grid aligned. This may not be needed to achieve the desired effect.
            // DOUBLE OPTIONAL AND DIFFICULT: incorporate spawn types here and give them different radii of exclusion. This is really probably not needed, but it is a cool feature in theory.
            // I like this approach a lot better because the actual process is very straightforwards. The efficiency is not necessarily the best, but as long as the number of DESIRED locations remains reasonable, which why would it not? 
            // this should be plenty sufficient! I'm gonna go take a shower, and then I will implement this and see how the distribution looks! 
            // quick note, this isnt gonna be as nice as poisson disk sampling, but given our distribution will likely be sparse, and the player will have restricted vision, as long as objects are sufficiently remote the precise distribution shouldn't really matter!
            // our only REAL problem may be occasional sparse areas, or clusters of unguarded treasures, that sort of thing.
            // the first problem can be solved by modifying the spawn point chooser to begin with 1 area, which it then bisects, or quads, or some other division.
            // it then makes sure to fill each of these quadrants, etc. Just something to enforce some amount of even distribution.
            // as for the second problem, this is easily solved in populate spawn locations. just add distance checks to the nearest existent node, or spawn chaining.
            // for example, you could have it such that when we spawn a treasure, we also spawn a monster in the nearest node! Just some ideas. Ok im actually gonna go shower.
            float[,] terrainHeights = terrain.terrainData.GetHeights(0, 0, settings.width, settings.height);
            List<Vector3> potentialSpawnLocations = new List<Vector3>();
            Vector2 circleCenter = new Vector2(settings.width / 2, settings.height / 2);

            // loop through each terrain point and check if it could be used as a spawn location
            // TODO im going to bed but this loop is overrestricting somehow. finds zero potential locations.
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    var point = new Vector2(x, y);
                    var distance = Vector2.Distance(point, circleCenter);

                    // select only points outside the spawn circle, inside the max range circle, and with satisfactory gradient.
                    if (distance > settings.startRadius && distance <= settings.maximumSpawnRadius &&
                        terrain.terrainData.GetSteepness(x, y) <= settings.maximumTolerableGradient)
                    {
                        var newLocation = new Vector3(x, 0, y) - globalOffset;
                        newLocation.y = terrain.SampleHeight(newLocation);
                        potentialSpawnLocations.Add(newLocation);
                    }
                }
            }
            Debug.LogError("Found " + potentialSpawnLocations.Count + " potential spawn locations.");

            // now we have all potential spawn locations based on our restrictions. Time to pick randomly.
            while (numSpawns < settings.spawnPoints && potentialSpawnLocations.Count != 0)
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
            // Iterate through each spawn location
            // roll an object type at random
            // if this object is at maximum capacity, roll again. Try N times.
            // spawn the object at this location
            // DONE
        }

        // places a mostly flat circle in the center of the terrain for the submarine to start in.
        TerrainData GenerateStartingArea(TerrainData terrainData)
        {
            Vector2 circleCenter = new Vector2(settings.width / 2, settings.height / 2);
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
        
        void IllustrateSpawnLocationsDebug(float size)
        {
            foreach (Vector3 spawnPoint in spawnLocations)
            {
                Debug.LogError("Vector: " + spawnPoint);
                Instantiate(debugMarker, spawnPoint, Quaternion.Euler(Vector3.zero));
            }
        }
    }
}
