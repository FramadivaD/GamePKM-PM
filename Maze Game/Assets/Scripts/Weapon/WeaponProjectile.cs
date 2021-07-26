﻿using UnityEngine;
using System.Collections;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] protected PhotonView pv;

    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float destroyAfter = 5;

    public float Damage { get { return damage; } }
    public float Speed { get { return damage; } }

    [SerializeField] protected GameObject explosiveEffect;

    Rigidbody2D rb2;

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(DestroyProjectile(destroyAfter));
    }

    public void Initialize(Quaternion rotation)
    {
        if (PhotonNetwork.connected)
        {
            pv.RPC("InitializeRPC", PhotonTargets.AllBuffered, rotation);
        }
        else
        {
            InitializeRPC(rotation);
        }
    }

    [PunRPC]
    private void InitializeRPC(Quaternion rotation)
    {
        transform.rotation = rotation;

        Launch();
    }

    private void Launch()
    {
        Vector3 dir = transform.right * speed;
        rb2.AddForce(dir, ForceMode2D.Impulse);
    }

    public void TerminateProjectile()
    {
        SpawnExplosiveEffect();

        if (PhotonNetwork.connected)
        {
            if (pv.isMine)
            {
                pv.RPC("TerminateProjectileRPC", PhotonTargets.AllBuffered);
            }
        }
        else
        {
            TerminateProjectileRPC();
        }
    }

    [PunRPC]
    private void TerminateProjectileRPC()
    {
        SpawnExplosiveEffect();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (pv.isMine) {
            if (collider.CompareTag("RoomWall"))
            {
                TerminateProjectile();
            }
            else if (collider.CompareTag("RoomDoor"))
            {
                TerminateProjectile();
            }
            else if (collider.CompareTag("RoomGate"))
            {
                TerminateProjectile();
            }
        }
    }

    private void SpawnExplosiveEffect()
    {
        if (explosiveEffect)
        {
            Instantiate(explosiveEffect, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DestroyProjectile(float destroyAfter)
    {
        yield return new WaitForSecondsRealtime(destroyAfter);

        TerminateProjectile();
    }
}
