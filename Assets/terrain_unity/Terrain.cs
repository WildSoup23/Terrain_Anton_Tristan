using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class Terrain
{
    public Mesh Regenerate(Vector2Int size, Vector2Int resolution, bool flip)
    {
        // Create mesh
        Mesh mesh = new Mesh();
        NativeArray<Vector3> Vertices = CalculateVertices(size, resolution);
        int[] Triangles = CalculateTriangles(size, resolution,flip);
        
        // Populate the mesh with data
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        
        // Return 
        return mesh;
    }

    private NativeArray<Vector3> CalculateVertices(Vector2Int size, Vector2Int resolution)
    {
        NativeArray<Vector3> vertices = new NativeArray<Vector3>((resolution.x + 1)*(resolution.y + 1), Allocator.Temp);
        
        int count = 0;
        for (int x = 0; x < resolution.x + 1; x++)
        {
            for (int y = 0; y < resolution.y + 1; y++)
            {
                vertices[count] = new Vector3((x-(resolution.x/2f))*size.x/resolution.x, 0, (y-(resolution.y/2f))*size.y/resolution.y);
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
    
}
