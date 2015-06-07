using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	static FollowCam FollowCamInstance;

	public float easing = 0.05f;
	public Vector2 minXY;

	public bool ___________;

	// Dynamically set fields
	public GameObject target;
	public float camZ;

	void Awake() 
	{
		FollowCamInstance = this;
		camZ = transform.position.z;
	}

	void FixedUpdate()
	{
		if (target == null)
			return;

		// Follow target
		Vector3 destination = target.transform.position;
		// Correct the position with the Min X Y
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);
		// Interpolate the position for the smoothiness
		destination = Vector3.Lerp (transform.position, destination, easing);
		destination.z = camZ;
		// Go!
		transform.position = destination;
		// Maintain the ground on view (also, zoom)
		Camera.main.orthographicSize = destination.y + 10;
	}

	public static void SetTarget(GameObject newTarget)
	{
		FollowCamInstance.target = newTarget;
	}

	public static GameObject getTarget()
	{
		return FollowCamInstance.target;
	}
}
