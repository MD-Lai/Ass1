using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public LandScape landScape;
    public Shader shader;
    Color color = Color.blue - Color.black * 0.7f; // 30% opacity
	// Use this for initialization
	void Start () {
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        terrainMesh.mesh = this.CreateWaterMesh();
        
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;

    }
	
	// Update is called once per frame
	void Update () {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    private Mesh CreateWaterMesh() {
        Mesh water = new Mesh();
        float level = landScape.getWaterLevel();    // y coord where the mesh should rest
        float min = landScape.getMinBounds();       // rightmost point for x, bottommost point for z
        float max = landScape.getMaxBounds();       // leftmost point for x, topmost point for z
        water.name = "WaterMesh";
        
        water.vertices = new Vector3[] {
            new Vector3(max, level, min),              // bottom left
            new Vector3(max, level, max),              // top left
            new Vector3(min, level, max),              // top right
            new Vector3(min, level, min)               // bottom right
        };
        water.colors = new Color[] {
            color,color,color,color
        };

        water.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        water.RecalculateNormals();

        return water;
    }
}
