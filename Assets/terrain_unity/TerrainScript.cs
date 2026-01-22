using System;
using UnityEngine;
using UnityEngine.PlayerLoop;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;
    
    //Varibles 
    [Header("Variables")]
    [Tooltip("Sets the size of the terrain")]
    public Vector2Int size;
    [Tooltip("Sets the amount of triangles in the terrain")]
    public Vector2Int resolution;
    [Tooltip("Sets how the terrains squares are cut into triangles")]
    public bool flip;
    
    //Refrences
    [Header("Refrences")]
    [Tooltip("The terrains hight map")]
    public Texture2D heightmap;
    
    
    
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(size, resolution, flip);
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
