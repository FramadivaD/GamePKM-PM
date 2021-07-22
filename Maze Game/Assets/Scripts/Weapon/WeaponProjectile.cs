using UnityEngine;
using System.Collections;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;

    public float Damage { get { return damage; } }
    public float Speed { get { return damage; } }

    Rigidbody2D rb2;

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    public void Initialize(Quaternion rotation)
    {
        transform.rotation = rotation;

        Launch();
    }

    private void Launch()
    {
        Vector3 dir = transform.right * speed;
        rb2.AddForce(dir, ForceMode2D.Impulse);
    }
}
