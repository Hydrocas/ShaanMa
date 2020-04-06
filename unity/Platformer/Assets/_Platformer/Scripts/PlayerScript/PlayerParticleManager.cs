using Com.Isartdigital.Platformer.PlayerScript;
using Com.Isartdigital.Platformer.PlayerScript.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [Header("Constant particles")]
    [SerializeField] private ParticleSystem particleSprint;
    [SerializeField] private ParticleSystem particleWallSlide;
    [SerializeField] private ParticleSystem particleFireMovement;
    [SerializeField] private ParticleSystem particleFireIdle;

    private List<ParticleSystem> ParticleList;
    private PlayerPhysique player;

    private void Start()
    {
        ParticleList = new List<ParticleSystem>();
        player = transform.parent.transform.parent.GetComponent<PlayerPhysique>();
        player.eventParticles += activateParticle;

        int childrenNumber = transform.childCount;
        for (int i = 0; i <= childrenNumber - 1; i++)
        {
            ParticleList.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    private void Update()
    {
        if (player.isGrounded && player.getVelocity().magnitude > 1)
        {
            if (!particleSprint.isPlaying) particleSprint.Play();
        }
        else particleSprint.Stop();


        if (player._isWalledLeft || player._isWalledRight)
        {
            if (!particleWallSlide.isPlaying) particleWallSlide.Play();
        }
        else particleWallSlide.Stop();

        if (player.isGrounded && player.getVelocity().magnitude < 1)
        {
            if (!particleFireIdle.isPlaying)
            {
                particleFireMovement.Stop();
                particleFireIdle.Play();
            }
        }
        else
        {
            particleFireIdle.Stop();
            if (!particleFireMovement.isPlaying)
            {
                particleFireMovement.Play();
            }
        }
    }

    ///FireHead : 0 
    ///SprintDirt : 1
    ///WallSlide : 2 
    ///GroundJump : 3
    ///AirJump : 4
    ///LandLight : 5
    ///LandHeavy : 6
    ///Death : 7
    ///WallJumpRight : 8
    ///WallJumpLeft : 9
    private void activateParticle(int id)
    {
        ParticleList[id].Play();
    }
}
