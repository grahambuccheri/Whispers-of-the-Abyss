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
        // TODO move this from update to start. It's only in update to debug parameters.
        void Update()
        {
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
        }

        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = settings.width + 1;
            terrainData.size = new Vector3(settings.width, settings.depth, settings.height);

            float[,] heights = GenerateAndTuneHeights();
            
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
