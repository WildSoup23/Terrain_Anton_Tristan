using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class Terrain
{
    public Mesh Regenerate(Vector2Int size)
    {
        // Create mesh
        Mesh mesh = new Mesh();
        NativeArray<Vector3> vertices = new NativeArray<Vector3>((size.x+1)*(size.y+1), Allocator.Temp);
        NativeArray<int> triangles = new NativeArray<int>((size.x * size.y) *6, Allocator.Temp);
        
        int count = 0;
        for (int i = 0; i < size.x+1; i++)
        {
            for (int j = 0; j < size.y+1; j++)
            {
                vertices[count] = new Vector3(i, 0, j);
                count++;
            }
        }

        count = 0;
        for (int i = 0; i < size.x + 1; i++)
        {
            for (int j = 0; j < size.y + 1; j++)
            {
                triangles[count] = (i)+j;
                count++;
            }
        }
        
        /*
         i=0;
         triangels[count]
         
        012
        132
        
        143
        453
        
        
        */
        
        // Populate the mesh with data
        mesh.SetVertices(vertices);
        mesh.SetIndices(triangles, MeshTopology.Triangles, 0);
        
        // Empty native arrays
        vertices.Dispose();
        triangles.Dispose();
        
        // Mesh state for render
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        // Return 
        return mesh;
    }
    
    
}
