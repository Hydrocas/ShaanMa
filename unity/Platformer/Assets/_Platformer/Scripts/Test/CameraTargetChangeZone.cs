using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetChangeZone : MonoBehaviour
{
    [SerializeField] private Vector3 CameraTargetChange;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Com.Isartdigital.Platformer.PlayerScript.Physics.PlayerPhysique physique = other.GetComponent<Com.Isartdigital.Platformer.PlayerScript.Physics.PlayerPhysique>();
        if (physique != null) physique.setCameraTarget(CameraTargetChange);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Com.Isartdigital.Platformer.PlayerScript.Physics.PlayerPhysique physique = collision.GetComponent<Com.Isartdigital.Platformer.PlayerScript.Physics.PlayerPhysique>();
        if (physique != null) physique.resetCameraTarget(CameraTargetChange);
    }

    
}
