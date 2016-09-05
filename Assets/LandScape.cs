using UnityEngine;
using System.Collections;

public class LandScape : MonoBehaviour {

    public Shader shader;
    public PointLight lightSource;
    public Camera mainCamera;

    // "Settings"
    public int sizeFactor = 7;          // The n in 2^n + 1 for determining the number of points to calculate heights for
                                        // results in a 
    public float startRange = 20.0f;    // Pssible height range of the corners upon initialising a new terrain
    public float spacing = 5.0f;        // Horizontal distancec between each vertex
    public float roughFactor = 2.0f;    // Effect of randomness when calculating each point. 
    private int max;                    // Calculated maximum number of rows or columns
    public float globalMax;

    // Geometry
    private float[,] height;

    // Use this for initialization
    void Start() {

        max = IntPow(2, sizeFactor) + 1;

        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        terrainMesh.mesh = this.CreateTerrainMesh();

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;

        mainCamera.transform.localPosition = new Vector3(max * spacing / 2 - 0.5f, globalMax, -max * spacing / 2 + 0.5f);
        mainCamera.transform.localEulerAngles = new Vector3(20, -45, 0);
        mainCamera.farClipPlane = max * spacing * 1.414f;
        lightSource.transform.localPosition = new Vector3(max * spacing + globalMax, 0, 0);
    }

    // Update is called once per frame
    void Update() {
         
        if (Input.GetKeyDown(KeyCode.C)) {
            max = IntPow(2, sizeFactor) + 1;
            this.gameObject.GetComponent<MeshFilter>().mesh = this.CreateTerrainMesh();
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.RecalculateNormals();
            mainCamera.transform.localPosition = new Vector3(max * spacing / 2 - 0.5f, globalMax, -max * spacing / 2 + 0.5f);
            mainCamera.transform.localEulerAngles = new Vector3(20, -45, 0);
            mainCamera.farClipPlane = max * spacing * 1.414f;
            lightSource.transform.localPosition = new Vector3(max * spacing + globalMax, 0, 0);
        }
        

        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.lightSource.color);
        renderer.material.SetVector("_PointLightPosition", this.lightSource.GetWorldPosition());

    }

    /* Main functions */
    // create terrain mesh
    private Mesh CreateTerrainMesh() {
        globalMax = 0;
        Mesh mesh = new Mesh();
        mesh.name = "Terrain";

        Vector3[] newVertices = new Vector3[max * max];                 // max number of vertices per row/col, sizefactor = 8 gives 66,049 vertices
                                                                        // unity doesn't like more than 65,000. 
        Vector3[] newNormals = new Vector3[max * max];                  // and corresponding number of normals
        int[] newTriangles = new int[(max - 1) * (max - 1) * 2 * 3];    // max-1 squares per row/col, 2 triangles per square, 3 vertices per triangle
                                                                        
        Color[] newColors = new Color[max * max];

        int row, col, point = 0;

        // set height of corners
        height = new float[max, max];
        SetCorners();

        // perform diamond square algorithm
        DiamondSquare(max);

        // vertices and normals
        for (row = 0; row < max; row++) {
            for (col = 0; col < max; col++) {
                newVertices[max * row + col] = new Vector3(spacing * row - max * spacing / 2, height[row, col], spacing * col - max * spacing / 2);
                newColors[max * row + col] = Color.white;
                newNormals[max * row + col] = Vector3.forward;
            }
        }

        // triangles
        for (row = 1; row < max; row++) {
            for (col = 0; col < max - 1; col++) {
                // taking the 2 triangles of each square

                // triangle 1
                newTriangles[point] = max * row + col;                 // bottom left
                newTriangles[point + 1] = max * (row - 1) + col;       // top left
                newTriangles[point + 2] = max * (row - 1) + (col + 1); // top right

                // triangle 2
                newTriangles[point + 3] = max * row + col;             // bottom left
                newTriangles[point + 4] = max * (row - 1) + (col + 1); // top right
                newTriangles[point + 5] = max * row + (col + 1);       // bottom right
                point += 6;
            }
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        //mesh.normals = newNormals;
        mesh.colors = newColors;

        return mesh;
    }

    // sets the 4 corners of the array
    private void SetCorners() {
        height[0, 0] = Random.Range(-startRange, startRange);
        height[0, max - 1] = Random.Range(-startRange, startRange);
        height[max - 1, 0] = Random.Range(-startRange, startRange);
        height[max - 1, max - 1] = Random.Range(-startRange, startRange);
    }

    // Generate height map 
    private void DiamondSquare(int size) {
        int row, col;
        int step = size / 2;
        if (step < 1) return;

        for (row = step; row < max; row += size) {
            for (col = step; col < max; col += size) {
                Square(row, col, step, Random.Range(-size, size) * roughFactor);
            }
        }

        for (row = 0; row < max; row += step) {
            for (col = (row + step) % size; col < max; col += size) {
                Diamond(row, col, step, Random.Range(-size, size) * roughFactor);
            }
        }

        DiamondSquare(size / 2);
    }

    /* Assisting functions */
    // Generate centre of square
    private void Square(int row, int col, int step, float rand) {
        height[row, col] = average(
            retrieve(row - step, col - step),  // top left
            retrieve(row + step, col - step),  // top right
            retrieve(row - step, col + step),  // bottom left
            retrieve(row + step, col + step))  // bottom right
            + rand;
        if (height[row, col] > globalMax)
            globalMax = height[row, col];
    }

    // Generate centre of diamond 
    private void Diamond(int row, int col, int step, float rand) {
        height[row, col] = average(
            retrieve(row, col - step),  // up
            retrieve(row + step, col),  // right
            retrieve(row, col + step),  // down
            retrieve(row - step, col))  //left
            + rand;

        if (height[row, col] > globalMax)
            globalMax = height[row, col];
    }

    // gets average of not out of bound indices
    private float average(float point1, float point2, float point3, float point4) {
        float total = 0;
        int divisor;

        // an invalid can only ever happen at one point at a time
        if (point1 == -1 || point2 == -1 || point3 == -1 || point4 == -1)
            divisor = 3;
        else
            divisor = 4;

        //       if             then     else
        total += point1 != -1 ? point1 : 0;
        total += point2 != -1 ? point2 : 0;
        total += point3 != -1 ? point3 : 0;
        total += point4 != -1 ? point4 : 0;

        return total / divisor;
    }

    // "safe" retrieve i.e. returns an invalid value if accessing out of range
    private float retrieve(int row, int col) {

        if (row < 0 || col < 0 || row > max - 1 || col > max - 1) {
            return -1;
        }

        else return height[row, col];
    }

    // integer exponent, taken from stackoverflow question no. 383587
    private int IntPow(int x, int pow) {
        if (pow > 7) {
            pow = 7;
            sizeFactor = 7;
            print("Maximum size factor is 7, and has been set as such.\nAdjust spacing for a larger map.");
        }

        int ret = 1;
        while (pow != 0) {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }
    public float getMaxBounds() {
        return max * spacing / 2;
    }
    public float getMinBounds() {
        return -max * spacing / 2;
    }

}
