using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]private float cameraSpeed = 1;
    private Vector3 TargetedPosition = Vector3.zero;
    private Vector3 StartPosition;

    private void FixedUpdate()
    {
        Vector3 vectorToTarget = TargetedPosition - transform.localPosition;
        transform.localPosition += vectorToTarget * cameraSpeed * Time.deltaTime;
    }

    public void setTargetedPosition(Vector3 GivenPosition)
    {
        TargetedPosition = GivenPosition;
    }

    public void ResetTargetedPosition(Vector3 GivenPosition)
    {
        if (GivenPosition == TargetedPosition) TargetedPosition = Vector3.zero;
    }


}
