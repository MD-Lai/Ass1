using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public LandScape landScape;
    public Shader shader;
    public PointLight lightSource;
    Color color;

    private float waterLevel = 0;
    private MeshFilter waterMeshFilter;
    private Mesh waterMesh;
    private MeshRenderer waterRenderer;
    private MeshCollider waterCollider;
    // Use this for initialization
    void Start () {
        color = new Color(0,0,1,0.5f);

        waterMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        waterMeshFilter.mesh = this.CreateWaterMesh();
        
        waterRenderer = this.gameObject.AddComponent<MeshRenderer>();
        waterRenderer.material.shader = shader;
        waterRenderer.material.color = color;

        waterCollider = this.gameObject.AddComponent<MeshCollider>();
        waterCollider.sharedMesh = this.GetComponent<MeshFilter>().mesh;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (waterLevel != landScape.getWaterLevel()) {
            waterMeshFilter.mesh = this.CreateWaterMesh();
            waterCollider.sharedMesh = this.GetComponent<MeshFilter>().mesh;
        }
        waterRenderer.material.SetColor("_PointLightColor", this.lightSource.color);
        waterRenderer.material.SetVector("_PointLightPosition", this.lightSource.GetWorldPosition());
    }

    private Mesh CreateWaterMesh() {
        Mesh water = new Mesh();
        waterLevel = landScape.getWaterLevel();    // y coord where the mesh should rest
        float min = landScape.getMinBounds();       // rightmost point for x, bottommost point for z
        float max = landScape.getMaxBounds();       // leftmost point for x, topmost point for z

        water.name = "WaterMesh";
        
        water.vertices = new Vector3[] {
            new Vector3(min, waterLevel, min),              // bottom left
            new Vector3(min, waterLevel, max),              // top left
            new Vector3(max, waterLevel, max),              // top right
            new Vector3(max, waterLevel, min)               // bottom right
        };
        
        water.colors = new Color[] {
            color,color,color,color
        };
        

        water.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        water.RecalculateNormals();

        return water;
    }
}
