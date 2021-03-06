using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PhotonView pv;

    public float timeAttack;
    public float timeIdle;
    public float enemySpeed;

    public GameObject playerTarget;
    public Health health;

    private bool isAttack = false;
    private float startTimeAttack, startTimeIdle;
    private Animator enemyAnim;

    public GameObject dieSpawn;

    private Rigidbody2D rb;

    [SerializeField] private bool allowMove = false;
    public bool AllowMove
    {
        get { return allowMove; }
        set { allowMove = value; }
    }

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        startTimeIdle = timeIdle;
        startTimeAttack = timeAttack;

        health.OnDied += OnDie;

        if (pv.isMine)
        {

        }
        else
        {
            rb.isKinematic = true;
        }
    }

    private void OnDestroy()
    {
        health.OnDied -= OnDie;
    }

    void Update()
    {
        if (playerTarget)
        {
            if (!GameManager.Instance.IsPaused
                && GameManager.Instance.AllowEntityMove
                && GameManager.Instance.AllowEnemyMove
                && !GameManager.Instance.WinnerWasAnnounced
                && AllowMove)
            {
                // transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * enemySpeed);

                rb.MovePosition(Vector2.MoveTowards(transform.position, playerTarget.transform.position, Time.deltaTime * enemySpeed));

                if (transform.position.x < playerTarget.transform.position.x)
                {
                    transform.eulerAngles = Vector3.down * 180f;
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                }
                if (startTimeAttack <= 0 && !isAttack)
                {
                    enemyAnim.SetTrigger("StartAttack");
                    isAttack = true;
                    startTimeAttack = timeAttack;
                }
                else if (startTimeAttack > 0 && !isAttack)
                {
                    startTimeAttack -= Time.deltaTime;
                }
                if (startTimeIdle <= 0 && isAttack)
                {
                    enemyAnim.SetTrigger("StopAttack");
                    isAttack = false;
                    startTimeIdle = timeIdle;
                }
                else if (startTimeIdle > 0 && isAttack)
                {
                    startTimeIdle -= Time.deltaTime;
                }
            }
        } else
        {
            if (startTimeIdle <= 0 && isAttack)
            {
                enemyAnim.SetTrigger("StopAttack");
                isAttack = false;
                startTimeIdle = timeIdle;
            }
        }
    }

    private void OnDie()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("OnDieRPC", PhotonTargets.AllBuffered);

                PhotonNetwork.Destroy(gameObject);
            }
        } else
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    private void OnDieRPC()
    {
        Instantiate(dieSpawn, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            if (collision.TryGetComponent(out WeaponProjectile projectile))
            {
                if (PhotonNetwork.connected)
                {
                    if (PhotonNetwork.player.IsMasterClient)
                    {
                        if (projectile.IsTrigger)
                        {
                            health.CurrentHealth -= projectile.Damage;

                            if (health.IsDie)
                            {
                                // killed by red team
                                if (projectile.LaunchedBy == 0)
                                {
                                    ScoreManager.Instance.AddScore(TeamType.Red, 1);
                                }
                                // killed by blue team
                                else if (projectile.LaunchedBy == 1)
                                {
                                    ScoreManager.Instance.AddScore(TeamType.Blue, 1);
                                }
                            }

                            projectile.DisableTrigger();
                        }
                    }
                }
                projectile.TerminateProjectile();
            }
        }
    }
}
