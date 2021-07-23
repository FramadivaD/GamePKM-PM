using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("All about Enemy Config")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float touchDamage;

    [SerializeField] private bool isFollowPlayerInsteadOfMove = false;
    [SerializeField] private Transform moveTarget;
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

                if (isFollowPlayerInsteadOfMove) {
                    MoveController(playerTarget);
                } else
                {
                    MoveController(moveTarget);
                }

                attackManager.Attack(playerTarget);
            }
        } else
        {
            rb2.velocity = Vector2.zero;
        }
    }

    bool changeTargetRoomflag = false;
    private void MoveController(Transform target)
    {
        Vector3 distance = (target.transform.position - transform.position);
        rb2.velocity = distance.normalized * followSpeed;

        // Change move Target

        if (Vector3.Distance(target.transform.position, transform.position) < 3)
        {
            if (changeTargetRoomflag == false)
            {
                isFollowPlayerInsteadOfMove = !isFollowPlayerInsteadOfMove;

                ChangeTargetRoom();
                changeTargetRoomflag = true;
            }
        } else
        {
            changeTargetRoomflag = false;
        }
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

    private void ChangeTargetRoom()
    {
        if (moveTarget.TryGetComponent(out Room room))
        {
            if (room)
            {
                ChangeMoveTarget(room.transform);
            }
        }
    }

    public void ChangePlayerTarget(Transform target)
    {
        playerTarget = target;
    }

    public void ChangeMoveTarget(Transform target)
    {
        moveTarget = target;
    }
}
