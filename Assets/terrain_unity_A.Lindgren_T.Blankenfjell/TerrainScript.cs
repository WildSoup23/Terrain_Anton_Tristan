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
    
    //Texture
    [Header("Texture")]
    [Tooltip("The texture to used for terrain")]
    public Texture2D terrainTexture;
    [Tooltip("Size of texture")] 
    public Vector2 textureSize;
    
    [Space(10)]
    
    // Heightmap generation
    [Header("Heightmap generation")]
    [Tooltip("If toggled on, ignores heightMapTexture and generates own")]
    public bool generateHeightMap;
    [Tooltip("The strength of the noise")]
    public float noiseScale =1f;
    [Tooltip("The position in the noise map")]
    public Vector2 org;
    [Tooltip("Size of the noise map")]
    public Vector2 noiseMapSize;
    [Tooltip("Noise terrain material")]
    public Material noiseMaterial;
    
    [Space(10)]
    
    //Refrences
    [Header("References")]
    [Tooltip("The terrains height map")]
    public Texture2D heightmap;
    
    //Height colors
    [Header("Height colors")]
    [Tooltip("The order of which colors change based on height")]
    public Color32[] Colors;
    [Tooltip("At which heights colors change")]
    public float[] heights;
    
    public void Regenerate()
    {
        //If no terrain, then create new terrain
        if (terrain == null) terrain = new Terrain();

        //Looks for MeshRenderer
        if(ren == null) ren = GetComponent<MeshRenderer>();
        
        //Creates mesh
        Mesh mesh = terrain.Regenerate(
            size, resolution, flip, 
            heightmap, heightMapSize, heightMapHeight, generateHeightMap, 
            ren, noiseScale, org, Colors, heights, 
            noiseMapSize, heightMapMaterial, noiseMaterial, textureSize);
        
        //Sets mesh name
        mesh.name = "TerrainMesh";
        
        //Changes materials main texture and texture scale
        ren.sharedMaterial.mainTexture = terrainTexture;
        ren.sharedMaterial.mainTextureScale = textureSize;
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    
    void Start()
    {
        //On start create mesh
        Regenerate();
    }
    
    //Draws line origo(0,0)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);

        Gizmos.DrawLine(new Vector3(0,0,0), new Vector3(0,99,0));
    }

}
