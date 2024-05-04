using System;
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
        // TODO i am uncertain why, but this method results in jagged terrain maps, whereas standard perlin does not. even with one octave!
        public static float[,] GenerateFractalPerlinMap(int seed, int width, int height, double scale, Vector2 offset,
            int octaves, float lacunarity, float persistence)
        {
            float[,] heights = new float[width, height];

            double maxHeight = 0;
            double minHeight = double.MaxValue;
            double amplitude = 1;
            double frequency = 1;

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
                        heights[i, j] += (float)(mapIteration[i, j] * amplitude);
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
                    heights[i, j] = (float)((heights[i, j] - minHeight) / (maxHeight - minHeight)); // why is this step so lossy :(
                }
            }

            return heights;
        }
        
        // produces a basic perlin noise map of the given size and parameters.
        public static float[,] GeneratePerlinMap(int seed, int width, int height, double scale, Vector2 offset)
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

        static float GetPerlinPoint(int x, int y, int width, int height, float offsetX, float offsetY, double scale)
        {
            float xNorm = (float) (x / (float) width * scale + offsetX);
            float yNorm = (float) (y / (float) height * scale + offsetY);

            return Mathf.PerlinNoise(xNorm, yNorm);
        }

        // takes in a heightmap and a depth to scale by, and returns a mapping of gradient values per point.
        public static float[,] GradientMap(float[,] originalMap, float depthScalar, int width, int height, int sampleWidth)
        {
            float[,] newMap = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float maxRise;
                    float riseX = x + sampleWidth < width ? Mathf.Abs(originalMap[x + sampleWidth, y] - originalMap[x, y]) : Mathf.Abs(originalMap[x - sampleWidth, y] - originalMap[x, y]);
                    float riseY = y + sampleWidth < width ? Mathf.Abs(originalMap[x, y + sampleWidth] - originalMap[x, y]) : Mathf.Abs(originalMap[x, y - sampleWidth] - originalMap[x, y]);
                    maxRise = Mathf.Max(riseX, riseY) * depthScalar;
                    // run is 1, so maxRise * depth scalar is the gradient approx at the point
                    newMap[x, y] = maxRise;
                }
            }

            return newMap;
        }
        
        // takes in a float heightmap, returns a new map where each index is the 3x3 cubic approximation of the points around it.
        // WIP not working.
        public static float[,] CubicApproximation(float[,] originalMap)
        {
            var width = originalMap.Rank;
            var height = originalMap.Length;
            float[,] newMap = new float[width, height];
            // iterate through each point
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // take the cubic approx
                    float total = 0;
                    int count = 0;
                    for (int dx = -2; dx <= 2; dx++)
                    {
                        for (int dy = -2; dy <= 2; dy++)
                        {
                            // check the bounds are valid for this access
                            if (x + dx >= 0 && x + dx < width && y + dy >= 0 && y + dy < height)
                            {
                                // add this point to the approx since it is valid
                                total += originalMap[x + dx, y + dy];
                                count++;
                            }
                        }
                    }
                    
                    // make the corresponding point in the new array the average.
                    newMap[x, y] = total / count;
                }
            }
            return newMap;
        }
    }
}
