using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("All about Enemy Config")]
    [SerializeField] private float followSpeed;

    [SerializeField] private Transform playerTarget;
    [SerializeField] private Health health;

    private Animator enemyAnim;
    private Rigidbody2D rb2;
    private EnemyBossAttackManager attackManager;


    [SerializeField] private bool allowMove = false;
    public bool AllowMove
    {
        get { return allowMove; }
        set { allowMove = value; }
    }

    public delegate bool CheckBossMModeReadyEventHandler();
    public event CheckBossMModeReadyEventHandler CheckBossModeReady = () => { return true; };

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        health = GetComponent<Health>();
        rb2 = GetComponent<Rigidbody2D>();
        attackManager = GetComponent<EnemyBossAttackManager>();

        health.OnDied += OnDie;
    }

    private void OnDestroy()
    {
        health.OnDied -= OnDie;
        CheckBossModeReady = null;
    }

    void Update()
    {
        if (playerTarget)
        {
            if (GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowEnemyMove
            && AllowMove)
            {
                if (CheckBossModeReady == null || CheckBossModeReady.Invoke()) {
                    MoveController();

                    attackManager.Attack(playerTarget);
                }
            }
        }
    }

    private void MoveController()
    {
        //transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * followSpeed);
        rb2.MovePosition(Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * followSpeed));
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            if (collision.TryGetComponent(out WeaponProjectile projectile))
            {
                health.CurrentHealth -= projectile.Damage;
                Destroy(collision.gameObject);
            }
        }
    }
}
