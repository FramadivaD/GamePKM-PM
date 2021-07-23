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

    [Header("Weapon Control")]
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
            AttackTarget();
        }
    }

    private void RandomizeAttackType()
    {
        attackType = (BossAttackType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(BossAttackType)).Length);

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

            GameObject ne = Instantiate(projectileLaunchPrefab, projectileLaunchSpawnOrigin.position, Quaternion.identity);
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

    private void BulletHellController()
    {
        bulletHellAngle += Time.deltaTime;
        if (bulletHellAngle >= 360)
        {
            bulletHellAngle -= 360;
        }

        RotateBulletHellOrigin();

        if (attackType == BossAttackType.ProjectileBulletHell)
        {
            if (bulletHellTime <= 0)
            {
                FireBulletHell();
                bulletHellTime += timeLaunchProjectileBulletHell;
            } else
            {
                bulletHellTime -= Time.deltaTime;
            }
        }
    }

    private void RotateBulletHellOrigin()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, bulletHellAngle);
        projectileBulletHellController.rotation = rotation;
    }

    private void FireBulletHell()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, bulletHellAngle);

        GameObject ne = Instantiate(projectileBulletPrefab, projectileBulletHellSpawnOrigin.position, Quaternion.identity);
        WeaponProjectile projectile = ne.GetComponent<WeaponProjectile>();
        if (projectile)
        {
            projectile.Initialize(rotation);
        }
    }
}
