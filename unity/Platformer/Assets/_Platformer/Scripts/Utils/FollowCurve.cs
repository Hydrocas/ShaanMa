///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 02/03/2020 18:43
///-----------------------------------------------------------------

using Com.IsartDigital.PlatFormer.Platformer.Utils;
using UnityEngine;

namespace Com.MaximilienGalea.Test {
    public class FollowCurve : MonoBehaviour {

        [SerializeField] private CurveLine curveToFollow;
        [SerializeField] private float time;
        [SerializeField] bool isTwoWay;

        private bool reverseWay = false;

        private float elapsedTime;

        private Vector3[] pathToFollow;

        private void Start() {
            transform.position = curveToFollow.PathPoints[0];
            pathToFollow = curveToFollow.PathPoints;

        }

        public void setCurve(CurveLine curveLine) {
            if (curveLine != null) {
                curveToFollow = curveLine;
            }
        }

        private void FixedUpdate() {
            MoveToNextPoint();
        }

        private void MoveToNextPoint() {

            elapsedTime += Time.fixedDeltaTime;

            transform.position = curveToFollow.GetPoint(elapsedTime / time);
            
        }
    }
}