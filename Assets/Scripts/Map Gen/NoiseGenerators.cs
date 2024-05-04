using UnityEngine;
using Random = System.Random;

// This class contains some exercises in noise generation for the purposes of creating satisfying terrains.
// The goal is to use fractal perlin noise for terrain generation of the sea bed. I would like to add a gradient modified version as well,
// but this is quite complex.
namespace Map_Gen
{
    public static class NoiseGenerators
    {
        // produces a fractal perlin noise map  with a given number of octaves
        public static float[,] GenerateFractalPerlinMap(int seed, int width, int height, float scale, Vector2 offset,
            int octaves, float lacunarity, float persistence)
        {
            float[,] heights = new float[width, height];
            
            float maxHeight = 0;
            float minHeight = float.MaxValue;
            float amplitude = 1;
            float frequency = 1;

            Random rng = new Random(seed);
            
            // initialize heights to zero
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    heights[i, j] = 0f;
                }
            }
            
            // ok here we will make our fractal.
            for (int octave = 0; octave < octaves; octave++)
            {
                // create the next height map layer
                Vector2 layerOffset = new Vector2(rng.Next(0, 500000) + offset.x, rng.Next(0, 500000) + offset.y);
                float[,] mapIteration = GeneratePerlinMap(seed, width, height, scale * frequency, layerOffset);
                
                // add this layer to cumulative product
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        heights[i, j] += mapIteration[i, j] * amplitude;
                        if (heights[i, j] > maxHeight)
                        {
                            maxHeight = heights[i, j];
                        }
                        if (heights[i, j] < minHeight)
                        {
                            minHeight = heights[i, j];
                        }
                    }
                }
                
                // prepare for next iteration
                amplitude *= persistence;
                frequency *= lacunarity;
            }
            
            // normalize the heights in the array
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    heights[i, j] = (heights[i, j] - minHeight) / (maxHeight - minHeight);
                }
            }

            return heights;
        }
        
        // produces a basic perlin noise map of the given size and parameters.
        public static float[,] GeneratePerlinMap(int seed, int width, int height, float scale, Vector2 offset)
        {
            float[,] map = new float[width, height];

            Random rng = new Random(seed);

            float offsetX = rng.Next(0, 500000) + offset.x;
            float offsetY = rng.Next(0, 500000) + offset.y;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = GetPerlinPoint(x, y, width, height, offsetX, offsetY, scale);
                }
            }

            return map;
        }

        static float GetPerlinPoint(int x, int y, int width, int height, float offsetX, float offsetY, float scale)
        {
            float xNorm = (float) x / width * scale + offsetX;
            float yNorm = (float) y / height * scale + offsetY;

            return Mathf.PerlinNoise(xNorm, yNorm);
        }
    }
}
