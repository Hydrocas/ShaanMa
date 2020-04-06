using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingLayerManagerHorizontalOnly : MonoBehaviour
{
    [SerializeField] private Transform cameraToFollow;
    [Header("La list follow ratio doit être égale au nombre de children")]
    [SerializeField] List<float> FollowRatioHorizontal;
    [SerializeField] List<float> FollowRatioVertical;
    private List<Transform> scrollingLayers;
    private int childrenNumber;
    private Vector3 lastCameraPosition;
    private Vector3 movementSinceLastFrame;

    public ScrollingLayerManagerHorizontalOnly(List<Transform> scrollingLayers)
    {
        this.scrollingLayers = scrollingLayers;
    }

    private void Start()
    {
        scrollingLayers = new List<Transform>();

        childrenNumber = transform.childCount;
        if (FollowRatioHorizontal.Count != childrenNumber)
        {
            Terminate();
            return;
        }

        for (int i = 0; i <= childrenNumber - 1; i++)
        {
            scrollingLayers.Add(transform.GetChild(i));
        }
        lastCameraPosition = cameraToFollow.position;
    }

    private void FixedUpdate()
    {
        movementSinceLastFrame = cameraToFollow.position - lastCameraPosition;
        for (int i = 0; i <= childrenNumber - 1; i++)
        {
            scrollingLayers[i].transform.position = new Vector3(scrollingLayers[i].transform.position.x + movementSinceLastFrame.x * FollowRatioHorizontal[i], scrollingLayers[i].transform.position.y + movementSinceLastFrame.y * FollowRatioVertical[i], scrollingLayers[i].transform.position.z);
        }
        lastCameraPosition = cameraToFollow.position;
    }

    private void Terminate()
    {
        Debug.LogError("Number of children different from number of followratio given. They must be the same number.");

        this.gameObject.SetActive(false);

    }
}
