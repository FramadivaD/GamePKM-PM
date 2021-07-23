using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAttackManager : MonoBehaviour
{
    private BossAttackType attackType;

    [Header("All about time interval")]
    [SerializeField] private float timeChangeTargetInterval;
    [SerializeField] private float timeLaunchProjectileLaunch;
    [SerializeField] private float timeLaunchProjectileBulletHell;
    [SerializeField] private float timeAttackPunch;

    [Header("Variable control")]
    [SerializeField] private float punchForce;
    [SerializeField] private float bulletHellFirePerShootInterval;
    [SerializeField] private float bulletHellAngleSpeed;

    [Header("Weapon Control")]
    [SerializeField] private Transform projectileLaunchController;
    [SerializeField] private Transform projectileLaunchSpawnOrigin;
    [SerializeField] private Transform projectileBulletHellController;
    [SerializeField] private Transform projectileBulletHellSpawnOrigin;

    [Header("All about Objects and Prefab")]
    [SerializeField] private GameObject projectileLaunchPrefab;
    [SerializeField] private GameObject projectileBulletPrefab;

    public bool IsAttacking { get; set; }

    // private
    private bool attackDone = false;
    private float timeToAttack = 0;

    private float bulletHellAngle = 0;
    private float bulletHellTime = 0;

    private Rigidbody2D rb2;

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        BulletHellController();
    }

    public void Attack(Transform target)
    {
        if (timeToAttack <= 0)
        {
            RandomizeAttackType();
        }
        else
        {
            AttackTarget(target);
            timeToAttack -= Time.deltaTime;
        }
    }

    private void RandomizeAttackType()
    {
        attackType = (BossAttackType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(BossAttackType)).Length);

        if (attackType == BossAttackType.None)
        {
            attackDone = false;
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

    private void AttackTarget(Transform target)
    {
        if (attackDone == false)
        {
            if (attackType == BossAttackType.None)
            {
                attackDone = true;
                Debug.Log("Enemy Boss Proceed Doing nothing");
            }
            else if (attackType == BossAttackType.ProjectileLaunch)
            {
                timeToAttack = timeLaunchProjectileLaunch;
                attackDone = true;

                // Calculate rotation to target
                Vector3 direction = target.position - transform.position;
                float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // fire the projectile
                FireProjectile(projectileLaunchPrefab, projectileBulletHellSpawnOrigin, rotationZ);

                Debug.Log("Enemy Boss Proceed Projectile Launch");
            }
            else if (attackType == BossAttackType.ProjectileBulletHell)
            {
                timeToAttack = timeLaunchProjectileBulletHell;
                attackDone = true;

                Debug.Log("Enemy Boss Proceed Bullet Hell Launch");
            }
            else if (attackType == BossAttackType.Punch)
            {
                timeToAttack = timeAttackPunch;
                attackDone = true;

                if (rb2)
                {
                    // Calculate rotation to target
                    Vector3 direction = target.position - transform.position;

                    rb2.AddForce(direction * punchForce, ForceMode2D.Impulse);
                }

                Debug.Log("Enemy Boss Proceed Punch");
            }
        }
    }

    private void BulletHellController()
    {
        bulletHellAngle += bulletHellAngleSpeed * Time.deltaTime;
        if (bulletHellAngle >= 360)
        {
            bulletHellAngle -= 360;
        }

        RotateProjectileOrigin(projectileBulletHellController, bulletHellAngle);

        if (attackType == BossAttackType.ProjectileBulletHell)
        {
            if (bulletHellTime <= 0)
            {
                FireProjectile(projectileBulletPrefab, projectileBulletHellSpawnOrigin, bulletHellAngle);
                bulletHellTime += bulletHellFirePerShootInterval;
            } else
            {
                bulletHellTime -= Time.deltaTime;
            }
        }
    }

    private void RotateProjectileOrigin(Transform originTransform, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        originTransform.rotation = rotation;
    }

    private void FireProjectile(GameObject projectilePrefab, Transform spawnerTransform, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject ne = Instantiate(projectilePrefab, spawnerTransform.position, Quaternion.identity);
        WeaponProjectile projectile = ne.GetComponent<WeaponProjectile>();
        if (projectile)
        {
            projectile.Initialize(rotation);
        }
    }
}