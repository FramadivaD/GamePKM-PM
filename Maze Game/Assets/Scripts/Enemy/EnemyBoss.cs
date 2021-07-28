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

    [SerializeField] private GameObject dieSpawn;
    [SerializeField] private int spawnExplosionCount = 10;

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

    public PhotonView pv;

    public void Initialize(TeamType teamType, Gate mainGate)
    {
        TeamType = teamType;
        MainGate = mainGate;

        if (PhotonNetwork.connected) {
            pv.RPC("InitializeRPC", PhotonTargets.Others, (int) teamType);
        }
    }

    [PunRPC]
    private void InitializeRPC(int teamTypeInt)
    {
        TeamType = (TeamType)teamTypeInt;
    }

    [SerializeField] private float changeRoomTimeInterval = 5;
    private float changeRoomTime = 0;

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
        if (pv.isMine)
        {
            if (
                !GameManager.Instance.IsPaused
                && GameManager.Instance.AllowEntityMove
                && GameManager.Instance.AllowEnemyMove
                && !GameManager.Instance.WinnerWasAnnounced
                && AllowMove)
            {
                if (MainGate && MainGate.CheckIsOpened())
                {
                    CheckBossModeStarted();

                    if (playerTarget == null)
                    {
                        attackManager.ChangePlayerTarget();
                    }

                    if (isFollowPlayerInsteadOfMove)
                    {
                        MoveController(playerTarget);
                    }
                    else
                    {
                        MoveController(moveTarget);
                    }

                    attackManager.Attack(playerTarget);
                }
            }
            else
            {
                rb2.velocity = Vector2.zero;
            }
        }
    }

    private void CheckBossModeStarted()
    {
        if (BossModeStarted == false)
        {
            if (PhotonNetwork.connected)
            {
                if (PhotonNetwork.player.IsMasterClient)
                {
                    pv.RPC("CheckBossModeStartedRPC", PhotonTargets.AllBuffered);
                }
            }
            else
            {
                CheckBossModeStartedRPC();
            }
        }
    }

    [PunRPC]
    private void CheckBossModeStartedRPC()
    {
        if (BossModeStarted == false)
        {
            Debug.Log("Starting following player..");
            BossModeStarted = true;
        }
    }

    private void MoveController(Transform target)
    {
        Vector3 distance = (target.transform.position - transform.position);
        rb2.velocity = distance.normalized * followSpeed;

        CheckUntilChangeRoom();
    }

    private void CheckUntilChangeRoom()
    {
        Transform target = moveTarget;
        if (changeRoomTime <= 0)
        {
            // Change move Target
            if (Vector3.Distance(target.transform.position, transform.position) < 0.5f)
            {
                // isFollowPlayerInsteadOfMove = !isFollowPlayerInsteadOfMove;

                Debug.Log("Proceed Change Target Room..");
                ChangeTargetRoom();
                changeRoomTime = changeRoomTimeInterval;
            }
        } else {
            changeRoomTime -= Time.deltaTime;
        }
    }

    private void OnDie()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            if (PhotonNetwork.connected)
            {
                pv.RPC("OnDieRPC", PhotonTargets.AllBuffered);
            }
            else
            {
                OnDieRPC();
            }
        }
    }

    [PunRPC]
    private void OnDieRPC()
    {
        PlayDieAnimation();
        attackManager.enabled = false;

        SpectatorController.Instance.Track(transform);
        GameManager.Instance.AnnounceWinner(TeamType);
    }

    private void PlayDieAnimation()
    {
        Debug.Log("Boss is defeated.");

        StartCoroutine(SpawnExplosion(spawnExplosionCount));
    }

    private IEnumerator SpawnExplosion(int count)
    {
        if (count > 0) {
            yield return new WaitForSecondsRealtime(0.2f);

            Vector3 randPos = new Vector3(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));

            Instantiate(dieSpawn, randPos, Quaternion.identity);

            StartCoroutine(SpawnExplosion(count - 1));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BossModeStarted)
        {
            if (collision.tag == "PlayerProjectile")
            {
                if (collision.TryGetComponent(out WeaponProjectile projectile))
                {
                    if (PhotonNetwork.connected)
                    {
                        if (PhotonNetwork.player.IsMasterClient)
                        {
                            DamagedByProjectile(projectile.Damage);

                            projectile.TerminateProjectile();
                        } else
                        {
                            projectile.TerminateProjectile();
                        }
                    } else
                    {
                        DamagedByProjectile(projectile.Damage);

                        projectile.TerminateProjectile();
                    }
                }
            }
        }
    }

    private void ChangeTargetRoom()
    {
        if (moveTarget.TryGetComponent(out RoomGeneratorGrid room))
        {
            if (room)
            {
                Debug.Log("Actually Change move target to current room neighbor.");
                RoomGeneratorGrid neighbor = room.GetRandomNeighborRoom();
                if (neighbor) {
                    ChangeMoveTarget(neighbor.transform);
                }
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

    private void DamagedByProjectile(float damage)
    {

        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                health.CurrentHealth -= damage;
                pv.RPC("SyncEnemyBossHealthRPC", PhotonTargets.AllBuffered, health.CurrentHealth);
            }
        }
        else
        {
            health.CurrentHealth -= damage;
            SyncEnemyBossHealthRPC(damage);
        }
    }

    [PunRPC]
    private void SyncEnemyBossHealthRPC(float currentHealth)
    {
        health.CurrentHealth = currentHealth;
    }
}
