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
    [Tooltip("The size of the heightmap")]
    public Vector3 heightMapSize;
    [Tooltip("The size of the texture")]
    public Vector3 texureSize;
    [Tooltip("If toggled on, ignores heightMapTexture and generates own.")]
    public bool generateHeightMap;
    public float noiseScale =1f;
    public Vector2 org;
    
    [Space(10)]
    
    //Refrences
    [Header("Refrences")]
    [Tooltip("The terrains hight map")]
    public Texture2D heightmap;

    public Color32[] Colors;

    public MeshRenderer ren;
    
    
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(size, resolution, flip, heightmap, heightMapSize, generateHeightMap, ren, noiseScale, org);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();
    }
    
    
    void Start()
    {
        Regenerate();
    }

    
    void Update()
    {
    }
}
