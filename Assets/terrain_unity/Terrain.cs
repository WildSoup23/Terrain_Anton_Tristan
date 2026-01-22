using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class Terrain
{
    public Mesh Regenerate(Vector2Int size)
    {
        // Create mesh
        Mesh mesh = new Mesh();
        NativeArray<Vector3> vertices = new NativeArray<Vector3>((size.x + 1)*(size.y + 1), Allocator.Temp);
        int[] triangles = new int[(size.x * size.y) *6];
        List<Vector3> verticesList = new List<Vector3>();
        
        
        int count = 0;
        for (int x = 0; x < size.x + 1; x++)
        {
            for (int y = 0; y < size.y + 1; y++)
            {
                vertices[count] = new Vector3(x, 0, y);
                Debug.Log(vertices[count]);
                count++;
            }
        }
        
        /*
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
        */
        
        count = 0;
        int check = size.y + 1; 
        for (int x = 0; x < (size.x)*(size.y)*6; x++)
        {
            if (x + 1 == size.y * check)
            {
                break;
            }
            
            else if ((x + 1) % check == 0)
            {
                continue;
            }
            
            triangles[count] = x;
            triangles[count + 1] = x + 1;
            triangles[count + 2] = x + size.y + 1;
            count += 3;
        }
        
        
        
        // Populate the mesh with data
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        
        // Return 
        return mesh;
    }
    
    
}
