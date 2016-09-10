using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    public LandScape landScape;
    public float speedSetting = 400;          
    public float rotation = 80;         
    public float lookSensitivity = 40;
    public Rigidbody rb;
    private float speed;

    void Start() {
        speed = speedSetting * landScape.spacing;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        
        Vector3 nextPos = this.transform.localPosition;

        Vector3 totalVel = Vector3.zero;

        // Keyboard translation
        // Forward
        if (Input.GetKey(KeyCode.W)) {
            totalVel += this.transform.forward * speed * Time.deltaTime;
        }
        // Backward
        if (Input.GetKey(KeyCode.S)) {
            totalVel -= this.transform.forward  * speed* Time.deltaTime;
        }

        // Right
        if (Input.GetKey(KeyCode.D)) {
            totalVel += this.transform.right * speed * Time.deltaTime;
        }

        // Left
        if (Input.GetKey(KeyCode.A)) {
            totalVel -= this.transform.right * speed * Time.deltaTime;
        }

        // Up (not in the specs but it's a nice quality of life improvement)
        if (Input.GetKey(KeyCode.LeftShift)) {
            totalVel += this.transform.up * speed * Time.deltaTime;
        }
        
        // Down (not in specs either but whatever)
        if (Input.GetKey(KeyCode.LeftControl)) {
            totalVel -= this.transform.up * speed * Time.deltaTime;
        }

        // Forcing it back into bounds if it tried to escape
        if (nextPos.x > landScape.getMaxBounds()) {
            nextPos = new Vector3(landScape.getMaxBounds(), nextPos.y, nextPos.z);
        }

        if (nextPos.x < landScape.getMinBounds()) {
            nextPos = new Vector3(landScape.getMinBounds(), nextPos.y, nextPos.z);
        }

        if (nextPos.z > landScape.getMaxBounds()) {
            nextPos = new Vector3(nextPos.x, nextPos.y, landScape.getMaxBounds());
        }

        if (nextPos.z < landScape.getMinBounds()) {
            nextPos = new Vector3(nextPos.x, nextPos.y, landScape.getMinBounds());
        }

        // Only has effect if object tries to move out of bounds
        this.transform.localPosition = nextPos;

        // Set velocity vector for collision with movement
        rb.velocity = totalVel;
        
        
        // Rotate Counter Clockwise
        if (Input.GetKey(KeyCode.Q)) {
        this.transform.Rotate(new Vector3(0, 0, rotation * Time.deltaTime));
        }
        // Rotate Clockwise
        if (Input.GetKey(KeyCode.E)) {
            this.transform.Rotate(new Vector3(0, 0, -rotation * Time.deltaTime));
        }

        // mouse rotation
        this.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * lookSensitivity * Time.deltaTime);
    }
}