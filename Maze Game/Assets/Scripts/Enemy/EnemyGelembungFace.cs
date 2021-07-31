using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyGelembungFace : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] private SpriteRenderer face;

    [SerializeField] private Sprite frontFace;
    [SerializeField] private Sprite rightFace;

    private Animator animator;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemy && enemy.playerTarget)
        {
            if (face.sprite != rightFace)
            {
                if (enemy.playerTarget.transform.position.x > enemy.transform.position.x)
                {
                    // face.sprite = rightFace;

                    // enemy.pv.RPC("UpdateSprite", PhotonTargets.AllBuffered, 1);

                    animator.SetInteger("FacingState", 1);
                }
            }
        } else
        {
            if (face.sprite != frontFace)
            {
                // face.sprite = frontFace;

                // enemy.pv.RPC("UpdateSprite", PhotonTargets.AllBuffered, 0);

                animator.SetInteger("FacingState", 0);
            }
        }
    }
    
    /*
    [PunRPC]
    private void UpdateSprite(int state)
    {
        if (state == 0)
        {
            face.sprite = frontFace;
        } else
        {
            face.sprite = rightFace;
        }
    }
    */
}
