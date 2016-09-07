using UnityEngine;
using System.Collections;


public class DayNightCycle : MonoBehaviour {
    public PointLight pointLight;

	/** Gets the Directional light to always point at the point light
     * gives a day/night cycle
     */
	void Start () {
        Vector3 relativePos = this.transform.localPosition - pointLight.GetWorldPosition();
        this.transform.rotation = Quaternion.LookRotation(relativePos);
	}
	
	void Update () {
        Vector3 relativePos = this.transform.localPosition - pointLight.GetWorldPosition();
        this.transform.rotation = Quaternion.LookRotation(relativePos);
    }
}
