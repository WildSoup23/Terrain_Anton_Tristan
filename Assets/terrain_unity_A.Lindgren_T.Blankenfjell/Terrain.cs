using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class Terrain
{
    //References 
    private Texture2D noiseTex;
    private Color[] pix;
    private Color32[] pix2;

    //Function to generate mesh
    public Mesh Regenerate(Vector2Int size, Vector2Int resolution, bool flip, Texture2D heightmap, Vector2 heightMapSize,float heightMapheight, bool generateHeightMap, MeshRenderer ren, float noiseScale, Vector2 org
    , Color32[] colors, float[] heights, Vector2 noiseMapSize, Material material1, Material material2, Vector2 textureSize)
    {
        //If selected to generate height map, do this.
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
        
        //Create mesh
        Mesh mesh = new Mesh();
        
        //Creates an array for all the vertices colors
        pix2 = new Color32[(resolution.x + 1)*(resolution.y + 1)];
        
        //Creates vertices
        NativeArray<Vector3> Vertices = CalculateVertices(size, resolution, heightmap, heightMapSize,heightMapheight,generateHeightMap
        ,colors, heights, noiseMapSize);
        
        //Creates triangles
        int[] Triangles = CalculateTriangles(size, resolution,flip);
        
        // Populate the mesh with data
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        
        //Set the color for all the vertices
        mesh.SetColors(pix2);
        
        //Fixes all the UVs
        NativeArray<Vector2> UVs = new NativeArray<Vector2>((resolution.x + 1) * (resolution.y + 1), Allocator.Temp);
        for (int i = 0; i < UVs.Length; i++)
        {
            UVs[i] = new Vector2((Vertices[i].x-(textureSize.x/2) )/ textureSize.x, (Vertices[i].z-(textureSize.y/2) ) / textureSize.y);
        }
        mesh.SetUVs(0,UVs);
        
        //Fixes normals for lighting
        mesh.RecalculateNormals();
        
        return mesh;
    }

    //Function to calculate the vertices
    private NativeArray<Vector3> CalculateVertices(Vector2Int size, Vector2Int resolution,Texture2D heightmap ,Vector2 heightMapSize,float heightMapheight, bool generateHeightMap
    ,Color32[] colors, float[] heights, Vector2 noiseMapSize)
    {
        //Creates an array, which size is based on resolution
        NativeArray<Vector3> vertices = new NativeArray<Vector3>((resolution.x + 1)*(resolution.y + 1), Allocator.Temp);
        
        //Gets map size based on if its generated height map or a preset one.
        Vector2 mapSize;
        if (generateHeightMap) mapSize = noiseMapSize;
        else mapSize = heightMapSize;
        
        //Values to store the lowest height
        float lowestHight = heightMapheight;
        float newHight;
        
        //Creates vertices based on resolution, mapSize and heightmap.
        int count = 0;
        for (int x = 0; x < resolution.x + 1; x++)
        {
            for (int y = 0; y < resolution.y + 1; y++)
            {
                int x1 = Mathf.FloorToInt(((x-(resolution.x))*size.x/resolution.x) / mapSize.x * heightmap.width);
                int z1 = Mathf.FloorToInt(((y-(resolution.y))*size.y/resolution.y) /mapSize.y * heightmap.height);
                vertices[count] = new Vector3((x-(resolution.x/2f))*size.x/resolution.x, 
                    heightmap.GetPixel(x1, z1).grayscale * heightMapheight , 
                    (y-(resolution.y/2f))*size.y/resolution.y);

                //Gets lowest vertice
                newHight = vertices[count].y;
                if (lowestHight > newHight)
                {
                    lowestHight = newHight;
                }
                
                //Checks for vertices height and assigns color based on said height
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

        //Makes the lowest point in mesh be at origo(0,0)
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x,Mathf.Clamp(vertices[i].y-lowestHight,0,heightMapheight), vertices[i].z);
        }
        
        return vertices;
    }
    
    //Function to calculate triangles
    private int[] CalculateTriangles(Vector2Int size, Vector2Int resolution, bool flip)
    {
        //An array to store triangles, which size is based on resolution
        int[] triangles = new int[(resolution.x * resolution.y) *6];
        
        //Creates the first sets of triangles 
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
        //Creates second sets of triangles
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

    //Function to create a noise map
    private void CalcNoise(float noiseScale, Vector2 org)
    {
        //Creates noise for each pixel in noise texture
        for (float y = 0.0f; y< noiseTex.height; y++)
        {
            for (float x = 0.0f; x < noiseTex.width; x++)
            {
                float xCoord = org.x + x / noiseTex.width * noiseScale;
                float yCoord = org.y + y / noiseTex.height * noiseScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }
        
        //Applies pixels to texture
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
