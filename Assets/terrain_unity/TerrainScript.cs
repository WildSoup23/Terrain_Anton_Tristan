using System;
using UnityEngine;
using UnityEngine.PlayerLoop;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;
    
    //Varibles 
    public Vector2Int size;
    
    
    
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(size);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    
    void Start()
    {
        Regenerate();
    }

    
    void Update()
    {
    }
}
