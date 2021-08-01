using UnityEngine;
using System.Collections;

using Extensione.Audio;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float destroyAfter = 5;

    public float Damage { get { return damage; } }
    public float Speed { get { return damage; } }

    public int LaunchedBy { get; private set; } // 0 red Team, 1 blue team, neither ignoreable
    public bool IsTrigger { get; private set; } = true;

    [SerializeField] protected GameObject explosiveEffect;

    [SerializeField] protected AudioClip explosiveSFX;

    Rigidbody2D rb2;

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(DestroyProjectile(destroyAfter));
    }

    public void Initialize(int launchedBy, Quaternion rotation)
    {
        LaunchedBy = launchedBy;
        IsTrigger = true;

        transform.rotation = rotation;

        Launch();
    }

    public void Initialize(Quaternion rotation)
    {
        Initialize(-1, rotation);
    }

    private void Launch()
    {
        Vector3 dir = transform.right * speed;
        rb2.AddForce(dir, ForceMode2D.Impulse);
    }

    public void TerminateProjectile()
    {
        SpawnExplosiveEffect();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
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

    private void SpawnExplosiveEffect()
    {
        if (explosiveEffect)
        {
            AudioManager.Instance.PlaySFXOnce(explosiveSFX);

            Instantiate(explosiveEffect, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DestroyProjectile(float destroyAfter)
    {
        yield return new WaitForSecondsRealtime(destroyAfter);

        TerminateProjectile();
    }

    public void DisableTrigger()
    {
        IsTrigger = false;
    }
}
