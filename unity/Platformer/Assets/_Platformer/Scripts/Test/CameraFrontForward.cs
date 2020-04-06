///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 20/01/2020 11:25
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.Test {
	[RequireComponent(typeof(Camera))]
	public class CameraFrontForward : MonoBehaviour {

		[SerializeField] private Transform target;
		[SerializeField] private float viewportDistancePoint;

		private new Camera camera;
		private Vector3 camCenter;

		private void Awake()
		{
			camera = GetComponent<Camera>();
			camCenter = Vector2.one * 0.5f;
		}

		private void Update () {

			Vector3 leftPoint = camera.ViewportToWorldPoint(Vector3.left * viewportDistancePoint + camCenter);
			Vector3 rightPoint = camera.ViewportToWorldPoint(Vector3.right * viewportDistancePoint + camCenter);

			DebugPoint(leftPoint, Color.red);
			DebugPoint(rightPoint, Color.blue);

			Vector3 nextPos = new Vector3(target.position.x, target.position.y, transform.position.z);
			transform.position = nextPos;
		}

		private void DebugPoint(Vector3 direction, Color color)
		{
			//Vector2 origin = camera.ViewportToWorldPoint(Vector2.right * (direction * viewportDistancePoint) + new Vector2()
		}
	}
}