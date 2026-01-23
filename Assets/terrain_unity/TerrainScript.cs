using System;
using UnityEngine;
using UnityEngine.PlayerLoop;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;
    private MeshRenderer ren;
    
    //Varibles 
    [Header("Variables")]
    [Tooltip("Sets the size of the terrain")]
    public Vector2Int size;
    [Tooltip("Sets the amount of triangles in the terrain")]
    public Vector2Int resolution;
    [Tooltip("Sets how the terrains squares are cut into triangles")]
    public bool flip;
    [Tooltip("The size of the heightmap")]
    public Vector2 heightMapSize;
    [Tooltip("The height of the heightmap")]
    public float heightMapHeight;
    [Tooltip("Height map material")]
    public Material heightMapMaterial;
    
    [Space(10)]
    
    // Heightmap generation
    [Header("Heightmap generation")]
    [Tooltip("If toggled on, ignores heightMapTexture and generates own")]
    public bool generateHeightMap;
    [Tooltip("The strenght of the noise")]
    public float noiseScale =1f;
    [Tooltip("The postion in the noisemap")]
    public Vector2 org;
    [Tooltip("Size of the noisemap")]
    public Vector2 noiseMapSize;
    [Tooltip("Noise terrain material")]
    public Material noiseMaterial;
    
    [Space(10)]
    
    //Refrences
    [Header("Refrences")]
    [Tooltip("The terrains hight map")]
    public Texture2D heightmap;
    [Tooltip("An array of colors assigned to diffrent heights")]
    
    //Height colors
    [Header("Height colors")]
    public Color32[] Colors;

    public float[] heights;
    
    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        if(ren == null) ren = GetComponent<MeshRenderer>();
        
        Mesh mesh = terrain.Regenerate(size, resolution, flip, heightmap, heightMapSize, heightMapHeight, generateHeightMap, ren, noiseScale, org
        ,Colors, heights, noiseMapSize, heightMapMaterial, noiseMaterial);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();
       // mesh.SetColors(Colors);
    }
    
    
    void Start()
    {
        Regenerate();
    }

    
    void Update()
    {
    }
}
