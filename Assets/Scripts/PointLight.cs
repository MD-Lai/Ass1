using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;
    public float orbitSpeed;

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }

    void Update() {
        this.transform.RotateAround(Vector3.zero, Vector3.forward, orbitSpeed * Time.deltaTime);
    }
}
