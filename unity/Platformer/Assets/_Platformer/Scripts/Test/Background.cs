using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    
    [SerializeField] private float followRatio0;
    [SerializeField] private float followRatio1;
    [SerializeField] private Transform cameraToFollow;
    private Vector3 lastCameraPosition;
    private Vector3 movementSinceLastFrame;
    private Transform SkyBackground;


    // Start is called before the first frame update
    private void Start()
    {
        lastCameraPosition = cameraToFollow.position;
        SkyBackground = transform.GetChild(0);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        movementSinceLastFrame = cameraToFollow.position - lastCameraPosition;
        SkyBackground.position += movementSinceLastFrame * followRatio0;
        //transform.GetChild(1).transform.position += movementSinceLastFrame * followRatio1;
        lastCameraPosition = cameraToFollow.position;
    }
}
