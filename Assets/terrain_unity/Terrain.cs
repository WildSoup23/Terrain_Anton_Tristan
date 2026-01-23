using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class Terrain
{
    private Texture2D noiseTex;
    private Color[] pix;
    private Color32[] pix2;

    public Mesh Regenerate(Vector2Int size, Vector2Int resolution, bool flip, Texture2D heightmap, Vector2 heightMapSize,float heightMapheight, bool generateHeightMap, MeshRenderer ren, float noiseScale, Vector2 org
    , Color32[] colors, float[] heights, Vector2 noiseMapSize, Material material1, Material material2)
    {
        if (generateHeightMap)
        {
            noiseTex= new Texture2D((int)noiseMapSize.x, (int)noiseMapSize.y);
            pix = new Color[noiseTex.width * noiseTex.height];
            CalcNoise(noiseScale, org);
            ren.material = material2;
            heightmap = noiseTex;
        }
        else
        {
            ren.material = material1;
        }
        
        // Create mesh
        Mesh mesh = new Mesh();
        pix2 = new Color32[(resolution.x + 1)*(resolution.y + 1)];
        NativeArray<Vector3> Vertices = CalculateVertices(size, resolution, heightmap, heightMapSize,heightMapheight,generateHeightMap
        ,colors, heights, noiseMapSize);
        int[] Triangles = CalculateTriangles(size, resolution,flip);
        
        // Populate the mesh with data
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetColors(pix2);
        //mesh.SetUVs(0,Vertices);
        // Return 
        return mesh;
    }

    private NativeArray<Vector3> CalculateVertices(Vector2Int size, Vector2Int resolution,Texture2D heightmap ,Vector2 heightMapSize,float heightMapheight, bool generateHeightMap
    ,Color32[] colors, float[] heights, Vector2 noiseMapSize)
    {
        NativeArray<Vector3> vertices = new NativeArray<Vector3>((resolution.x + 1)*(resolution.y + 1), Allocator.Temp);
        Vector2 mapSize;
        if (generateHeightMap) mapSize = noiseMapSize;
        else mapSize = heightMapSize;
        
        int count = 0;
        for (int x = 0; x < resolution.x + 1; x++)
        {
            for (int y = 0; y < resolution.y + 1; y++)
            {
                
                int x1 = Mathf.FloorToInt(((x-(resolution.x))*size.x/resolution.x) / mapSize.x * heightmap.width);
                int z1 = Mathf.FloorToInt(((y-(resolution.y))*size.y/resolution.y) /mapSize.y * heightmap.height);
                
                vertices[count] = new Vector3((x-(resolution.x/2f))*size.x/resolution.x, heightmap.GetPixel(x1, z1).grayscale * heightMapheight , (y-(resolution.y/2f))*size.y/resolution.y);
                
                for (int i = 0; i < heights.Length; i++)
                {
                    if (vertices[count].y >= heights[i])
                    {
                        pix2[count] = colors[i];
                    }
                }
                count++;
            }
        }
        return vertices;
    }
    private int[] CalculateTriangles(Vector2Int size, Vector2Int resolution, bool flip)
    {
        int[] triangles = new int[(resolution.x * resolution.y) *6];
        // Forward
        int count = 0;
        int check = resolution.y + 1; 
        int flipNR = flip ? 2 : 1;
        
        for (int x = 0; x < (resolution.x+1)*(resolution.y+1); x++)
        {
            if (x + 1 == resolution.x * check)
            {
                break;
            }
            
            else if ((x + 1) % check == 0)
            {
                continue;
            }
            triangles[count] = x;
            triangles[count + 1] = x + 1;
            triangles[count + 2] = x + resolution.y + flipNR;
            count += 3;
        }
        // Backward
        for (int x = (resolution.x+1)*(resolution.y+1) - 1; x > 0; x--)
        {
            if (x == resolution.y+1)
            {
                break;
            }
            else if (x % check == 0)
            {
                continue;
            }
            triangles[count] = x;
            triangles[count + 1] = x - 1;
            triangles[count + 2] = x - resolution.y - flipNR;
            count += 3;
        }
        
        return triangles; 
    }

    private void CalcNoise(float noiseScale, Vector2 org)
    {
        for (float y = 0.0f; y< noiseTex.height; y++)
        {
            for (float x = 0.0f; x < noiseTex.width; x++)
            {
                float xCoord = org.x + x / noiseTex.width * noiseScale;
                float yCoord = org.y + y / noiseTex.height * noiseScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                Debug.Log(1);
            }

            Debug.Log(2);
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        
    }
}
