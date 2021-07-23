using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private BossAttackType attackType;

    [Header("All about time interval")]
    [SerializeField] private float timeChangeTargetInterval;
    [SerializeField] private float timeLaunchProjectileLaunch;
    [SerializeField] private float timeLaunchProjectileBulletHell;
    [SerializeField] private float timeAttackPunch;

    private bool attackDone = false;
    private float timeToAttack = 0;

    [Header("All about Enemy Config")]
    [SerializeField] private float followSpeed;

    [SerializeField] private Transform playerTarget;
    [SerializeField] private Health health;

    private Animator enemyAnim;
    private Rigidbody2D rb2;

    [SerializeField] private bool allowMove = false;
    public bool AllowMove
    {
        get { return allowMove; }
        set { allowMove = value; }
    }

    [Header("All about Objects and Prefab")]
    [SerializeField] private GameObject projectileLaunch;
    [SerializeField] private GameObject projectileBullet;

    public delegate bool CheckBossMModeReadyEventHandler();
    public event CheckBossMModeReadyEventHandler CheckBossModeReady = () => { return true; };

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        health = GetComponent<Health>();
        rb2 = GetComponent<Rigidbody2D>();

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
                    transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * followSpeed);

                    if (timeToAttack <= 0)
                    {
                        RandomizeAttackType();
                    } else
                    {
                        AttackTarget();
                    }
                }
            }
        }
    }

    private void RandomizeAttackType()
    {
        attackType = (BossAttackType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(BossAttackType)).Length);

        if (attackType == BossAttackType.None)
        {

        }
        else if (attackType == BossAttackType.Punch)
        {
            attackDone = false;
            timeToAttack = timeAttackPunch;
        }
        else if (attackType == BossAttackType.ProjectileLaunch)
        {
            attackDone = false;
            timeToAttack = timeLaunchProjectileLaunch;
        }
        else if (attackType == BossAttackType.ProjectileBulletHell)
        {
            attackDone = false;
            timeToAttack = timeLaunchProjectileBulletHell;
        }
    }

    private void AttackTarget()
    {
        if (attackType == BossAttackType.None)
        {

        }
        else if (attackType == BossAttackType.ProjectileLaunch)
        {
            timeToAttack = timeLaunchProjectileLaunch;
            attackDone = true;

            // GameObject ne = Instantiate(projectile, );
        }
        else if (attackType == BossAttackType.ProjectileBulletHell)
        {
            timeToAttack = timeLaunchProjectileBulletHell;
            attackDone = true;
        }
        else if (attackType == BossAttackType.Punch)
        {
            timeToAttack = timeAttackPunch;
            attackDone = true;
        }
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
