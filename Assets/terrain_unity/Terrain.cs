using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class Terrain
{
    public Mesh Regenerate(Vector2Int size)
    {
        // Create mesh
        Mesh mesh = new Mesh();
        NativeArray<Vector3> vertices = new NativeArray<Vector3>(999999999, Allocator.Temp);
        NativeArray<int> triangles = new NativeArray<int>((size.x * size.y) *6, Allocator.Temp);
        List<Vector3> verticesList = new List<Vector3>();
        
        
        int count = 0;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                vertices[count] = new Vector3(x, 0, y);
                Debug.Log(vertices[count]);
                count++;
                vertices[count] = new Vector3(x, 0, y + 1);
                Debug.Log(vertices[count]);
                count++;
                vertices[count] = new Vector3(x + 1, 0, y);
                Debug.Log(vertices[count]);
                count++;
            }
        }
        
        for (int x = size.x; x >0; x--)
        {
            for (int y = size.y; y > 0; y--)
            {
                vertices[count] = new Vector3(x, 0, y);
                Debug.Log(vertices[count]);
                count++;
                vertices[count] = new Vector3(x, 0, y - 1);
                Debug.Log(vertices[count]);
                count++;
                vertices[count] = new Vector3(x - 1, 0, y);
                Debug.Log(vertices[count]);
                count++;
            }
        }
        
        count = 0;
        for (int x = 0; x < (size.x)*(size.y)*6; x++)
        {
            triangles[count] = x;
            count++;
        }
        
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
