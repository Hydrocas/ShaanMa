///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 02/03/2020 20:57
///-----------------------------------------------------------------

using Com.IsartDigital.PlatFormer.Platformer.Utils;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Utils {
    [RequireComponent(typeof(CurveLine))]
    public class ForceCurvePoint : MonoBehaviour {

        private CurveLine curveLine;
        private Vector3[] path;
        private bool isInvert;
        private void Start() {
            curveLine = GetComponent<CurveLine>();
            path = curveLine.PathPoints;
            path[path.Length - 1] = transform.InverseTransformPoint(Camera.main.ViewportToWorldPoint(new Vector3(0, 1)));

            isInvert = Random.Range(0, 4) > 1;
        }

        private void Update() {
            path[path.Length - 1] = transform.InverseTransformPoint(Camera.main.ViewportToWorldPoint(new Vector3(0, 1)));

            if (isInvert) {
                path[1] = new Vector3(path[0].x, path[3].y);
                path[2] = new Vector3(path[3].x, path[0].y);
            } else {
                path[2] = new Vector3(path[0].x, path[3].y);
                path[1] = new Vector3(path[3].x, path[0].y);
            }
        }
    }
}