using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	// Fields set in the Inspector Pane
	public GameObject prefabProjectile;

	public bool _____________________;

	// Fields set dinamically
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	
	void Awake()
	{
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
		projectile.rigidbody.isKinematic = true;
	}

}
