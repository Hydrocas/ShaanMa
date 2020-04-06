using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentPlatform : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Collider2D col2D;
    private float playerSizeY;
    // Start is called before the first frame update
    void Start()
    {
        col2D = GetComponent<Collider2D>();

        playerSizeY = player.transform.localScale.y;
        Debug.Log(playerSizeY);

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y - playerSizeY / 2 < transform.position.y || Input.GetAxis("Vertical") < 0) col2D.enabled = false;
        else col2D.enabled = true;
    }
}
