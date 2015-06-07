using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	// Fields to be set manually
	public int numClouds = 40;
	public GameObject[] cloudPrefabs;
	public Vector3 cloudPosMin;
	public Vector3 cloudPosMax;
	public float cloudScaleMin = 1f;
	public float cloudScaleMax = 5f;
	public float cloudSpeedMult = 0.5f;

	public bool ______________________________;

	public GameObject[] cloudInstances;

	void Awake()
	{
		// Allocate the cloud storage
		cloudInstances = new GameObject[numClouds];
		// Fetch the CloudAnchor
		GameObject anchor = GameObject.Find ("CloudAnchor");
		// Create the clouds
		GameObject cloud;
		for (int i = 0; i < numClouds; i++) 
		{
			// Choose a random cloud
			int chosen = Random.Range (0, cloudPrefabs.Length);
			// Instantiate
			cloud = Instantiate (cloudPrefabs[chosen]) as GameObject;
			// Position it
			Vector3 cloudPos = new Vector3(Random.Range (cloudPosMin.x, cloudPosMax.x), Random.Range (cloudPosMin.y, cloudPosMax.y));
			// Scale it
			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp (cloudScaleMin, cloudScaleMax, scaleU);
			// Replace smaller clouds near the ground
			cloudPos.y = Mathf.Lerp (cloudPosMin.y, cloudPos.y, scaleU);
			// Smaller clouds should be farther away
			cloudPos.z = 100 - 90*scaleU;
			// Apply!
			cloud.transform.position = cloudPos;
			cloud.transform.localScale = Vector3.one * scaleVal;
			// Make the new cloud a child of the anchor
			cloud.transform.parent = anchor.transform;
			// Store it
			cloudInstances[i] = cloud;
		}
	}

	void Update()
	{
		foreach (GameObject cloud in cloudInstances) {
			// Fetch the cloud scale and position
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cloudPos = cloud.transform.position;
			// Larger (nearer) clouds move faster
			cloudPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
			// Reuse clouds that leave from the right
			if (cloudPos.x < cloudPosMin.x)
				cloudPos.x = cloudPosMax.x;
			// Apply
			cloud.transform.position = cloudPos;
		}
	}
}
