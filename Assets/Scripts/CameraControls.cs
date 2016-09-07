using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    public LandScape landScape;
    public float speed = 200;    // units per second
    public float rotation = 20; // degrees per second
    void Update() {
        Vector3 nextPos = this.transform.localPosition;

        if (Input.GetKey(KeyCode.W)) {
            nextPos += this.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S)) {
            nextPos -= this.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            nextPos += this.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A)) {
            nextPos -= this.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            nextPos += this.transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftControl)) {
            nextPos -= this.transform.up * speed * Time.deltaTime;
        }

        if(nextPos.x > landScape.getMaxBounds()) {
            nextPos = new Vector3(landScape.getMaxBounds(), nextPos.y, nextPos.z);
        }
        
        if(nextPos.x < landScape.getMinBounds()) {
            nextPos = new Vector3(landScape.getMinBounds(), nextPos.y, nextPos.z);
        }

        if(nextPos.z > landScape.getMaxBounds()) {
            nextPos = new Vector3(nextPos.x, nextPos.y, landScape.getMaxBounds());
        }

        if(nextPos.z < landScape.getMinBounds()) {
            nextPos = new Vector3(nextPos.x, nextPos.y, landScape.getMinBounds());
        }

        this.transform.localPosition = nextPos;
        
        // Rotate Counter Clockwise
        if (Input.GetKey(KeyCode.Q)) {
            this.transform.Rotate(new Vector3(0, 0, rotation * Time.deltaTime));
        }
        // Rotate Clockwise
        if (Input.GetKey(KeyCode.E)) {
            this.transform.Rotate(new Vector3(0, 0, -rotation * Time.deltaTime));
        }

        this.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotation * Time.deltaTime);
    }
}