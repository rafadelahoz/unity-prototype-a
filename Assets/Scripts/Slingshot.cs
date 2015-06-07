using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	// Singleton
	static Slingshot SlingshotInstance;

	// Fields set in the Inspector Pane
	public GameObject prefabProjectile;
	public float velocityMult = 4f;

	public bool _____________________;

	// Fields set dinamically
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	
	void Awake()
	{
		SlingshotInstance = this;

		Transform launchPointTransform = transform.Find ("LaunchPoint");
		launchPoint = launchPointTransform.gameObject;
		launchPoint.SetActive (false);
		launchPos = launchPointTransform.position;
	}

	void OnMouseEnter()
	{
		launchPoint.SetActive (true);
	}

	void OnMouseExit()
	{
		launchPoint.SetActive (false);
	}

	public void OnMouseDown() 
	{
		// Pressed the mouse button over the Slinghsot
		aimingMode = true;
		// Instantiate a projectile
		projectile = Instantiate (prefabProjectile) as GameObject;
		// Place it at LaunchPoint
		projectile.transform.position = launchPos;
		// Mark it as Kinematic (for what?)
		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}

	void Update()
	{
		if (!aimingMode)
			return;

		// Fetch mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		// Convert it to 3D world coordinates
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);
		// Find the delta between the mouse and the launch point
		Vector3 launchDelta = mousePos3D - launchPos;
		// Limit the launch delta to the radius of the Slingshot SphereCollider
		float maxLength = this.GetComponent<SphereCollider> ().radius;
		if (launchDelta.magnitude > maxLength) 
		{
			launchDelta.Normalize();
			launchDelta *= maxLength;
		}
		// Move the projectile to the new launch position
		Vector3 projectilePos = launchPos + launchDelta;
		projectile.transform.position = projectilePos;

		// Check for mouse release (launch!)
		if (Input.GetMouseButtonUp (0)) 
		{
			aimingMode = false;
			projectile.GetComponent<Rigidbody>().isKinematic = false;
			projectile.GetComponent<Rigidbody>().velocity = - launchDelta * velocityMult;
			FollowCam.SetTarget(projectile);
			projectile = null;
		}
	}

	public static Vector3 getLaunchPoint() {
		return SlingshotInstance.launchPoint.transform.position;
	}
}
