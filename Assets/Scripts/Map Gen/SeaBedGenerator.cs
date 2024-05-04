using UnityEngine;
using Random = System.Random;

namespace Map_Gen
{
    public class SeaBedGenerator : MonoBehaviour
    {
        public GeneratorSetting settings;
        
        private int seed;
        private Terrain terrain;
        void Start()
        {
            seed = settings.seed == 0 ? new Random().Next() : settings.seed;

            terrain = GetComponent<Terrain>();

            transform.localPosition -= new Vector3(settings.width / 2f, 0, settings.height / 2f);
        }

        // Update is called once per frame
        // TODO move all here in this from update to start. It's only in update to debug parameters.
        void Update()
        {
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            terrain.terrainData = GenerateStartingArea(terrain.terrainData);
        }

        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = settings.width + 1;
            terrainData.size = new Vector3(settings.width, settings.depth, settings.height);

            float[,] heights = GenerateAndTuneHeights();
            
            terrainData.SetHeights(0, 0, heights);
            return terrainData;
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
            float[,] heights = NoiseGenerators.GenerateFractalPerlinMap(seed,
                settings.width,
                settings.height,
                settings.scale,
                settings.offset,
                settings.octaves,
                settings.lacunarity,
                settings.persistence);
            
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
    }
}
