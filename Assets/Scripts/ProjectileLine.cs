using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileLine : MonoBehaviour {

	// Singleton
	static ProjectileLine ProjectileLineEntity;

	public float minDist = 0.1f;

	protected LineRenderer line;
	private GameObject _target;
	public List<Vector3> points;

	void Awake() {
		// Set the Singleton
		ProjectileLineEntity = this;

		// Fetch the line renderer
		line = GetComponent<LineRenderer> ();
		// And disable it until it's required
		line.enabled = false;
		// Initialize the point list
		points = new List<Vector3> ();
	}

	public GameObject target {
		get {
			return _target;
		}

		set {
			_target = value;

			if (_target != null) {
				// When a new target is set, restart everything
				line.enabled = false;
				points = new List<Vector3>();
				AddPoint();
			}
		}
	}

	public void Clear() {
		_target = null;
		line.enabled = false;
		points = new List<Vector3> ();
	}

	public void AddPoint() {

		if (target == null)
			return;

		Vector3 point = _target.transform.position;

		// Don't draw the points too close to each other
		if (points.Count > 0 && (point - LastPoint).magnitude < minDist) {
			return;
		}

		// If this is the launch point...
		if (points.Count == 0) {
			Vector3 launchPos = Slingshot.getLaunchPoint();
			Vector3 launchPosDiff = point - launchPos;
			// Update points array
			points.Add(point + launchPosDiff);
			points.Add(point);
			// Update line
			line.SetVertexCount(2);
			line.SetPosition(0, points[0]);
			line.SetPosition(1, points[1]);
			// Enable the line renderer
			line.enabled = true;
		} else {
			// Just add a point
			points.Add (point);
			line.SetVertexCount(points.Count);
			line.SetPosition(points.Count-1, point);
			line.enabled = true;
		}
	}

	public Vector3 LastPoint {
		get {
			if (points == null || points.Count == 0)
				return Vector3.zero;
			else
				return points[points.Count-1];
		}
	}

	void FixedUpdate() {
		if (target == null) {
			if (FollowCam.getTarget() != null) {
				if (FollowCam.getTarget().tag == "Projectile") {
					target = FollowCam.getTarget();
				} else {
					return;
				}
			} else {
				return;
			}
		}

		AddPoint ();
		if (target.GetComponent<Rigidbody> ().IsSleeping () || target.GetComponent<Rigidbody> ().velocity.magnitude < 0.05f) {
			target = null;
		}
	}
}
