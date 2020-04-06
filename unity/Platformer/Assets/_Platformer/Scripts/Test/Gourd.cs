using Com.Isartdigital.Platformer.PlayerScript.Physics;
using UnityEngine;

public class Gourd : MonoBehaviour
{
    [SerializeField] private Transform liquid;
    [SerializeField] private GameObject ObjectToFollow;
    [Header("Movement settings")]
    [SerializeField] private float speed = 2;
    [SerializeField] private float distanceFromPlayer = 1;
    private Vector3 vectorToTarget;
    private Vector3 liquidStartPosition;
    [Header("Time settings")]
    [SerializeField] private float timeLimitMax = 15;
    private float timeLeft;

    private void Start()
    {
        liquidStartPosition = liquid.transform.localPosition;
        timeLeft = timeLimitMax;
    }
    void FixedUpdate()
    {
        //mouvement de la gourde
        vectorToTarget = ObjectToFollow.transform.position - transform.position - Vector3.right * ObjectToFollow.GetComponent<PlayerPhysique>().FacingDirection * distanceFromPlayer + Vector3.up;
        transform.position += vectorToTarget * speed * Time.deltaTime;
        if (vectorToTarget.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (vectorToTarget.x < 0) transform.localScale = Vector3.one;
        //Update position de liquid en fonction du temps
        if (timeLeft > 0) timeLeft -= Time.deltaTime;
        else timeLeft = 0;
        liquid.transform.localPosition = liquidStartPosition + (Vector3.up * timeLeft / timeLimitMax) - Vector3.up;

    }
}
