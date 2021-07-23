using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("All about Enemy Config")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float touchDamage;

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

    public bool BossModeStarted { get; private set; } = false;

    public float TouchDamage { get { return touchDamage; } }

    public TeamType TeamType { get; private set; }
    public Gate MainGate { get; private set; }

    public void Initialize(TeamType teamType, Gate mainGate)
    {
        TeamType = teamType;
        MainGate = mainGate;
    }

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
    }

    void Update()
    {
        if (GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowEnemyMove
            && !GameManager.Instance.WinnerWasAnnounced
            && AllowMove)
        {
            if (MainGate.CheckIsOpened())
            {
                if (BossModeStarted == false)
                {
                    Debug.Log("Starting following player..");
                    BossModeStarted = true;
                }

                if (playerTarget == null)
                {
                    attackManager.ChangePlayerTarget();
                }

                MoveController();

                attackManager.Attack(playerTarget);
            }
        } else
        {
            rb2.velocity = Vector2.zero;
        }
    }

    private void MoveController()
    {
        //transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * followSpeed);
        //rb2.MovePosition(Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * followSpeed));

        rb2.velocity = (playerTarget.transform.position - transform.position).normalized * followSpeed;
    }

    private void OnDie()
    {
        PlayDieAnimation();
        attackManager.enabled = false;

        GameManager.Instance.AnnounceWinner(TeamType);
    }

    private void PlayDieAnimation()
    {
        Debug.Log("Boss is defeated.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BossModeStarted)
        {
            if (collision.tag == "PlayerProjectile")
            {
                if (collision.TryGetComponent(out WeaponProjectile projectile))
                {
                    health.CurrentHealth -= projectile.Damage;
                    projectile.TerminateProjectile();
                }
            }
        }
    }

    public void ChangePlayerTarget(Transform target)
    {
        playerTarget = target;
    }
}
