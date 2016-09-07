using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public LandScape landScape;
    public Shader shader;

    float width, height, vertPos;
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
        float min = landScape.getMinBounds();
        float max = landScape.getMaxBounds();
        float height = landScape.getMinHeight() + landScape.getRange() * landScape.landBound * 0.9f;

        water.vertices = new[] {
            new Vector3(min,height,min),
            new Vector3(min,height,max),
            new Vector3(max,height,max),

            new Vector3(min,height,min),
            new Vector3(max,height,max),
            new Vector3(max,height,min)
        };

        water.colors = new[] {
            color,
            color,
            color,
            color,
            color,
            color
        };

        water.normals = new[] {
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0),

            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0)
        };


        return water;
    }
}
