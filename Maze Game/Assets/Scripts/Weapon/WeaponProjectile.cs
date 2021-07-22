using UnityEngine;
using System.Collections;

public class WeaponProjectile : MonoBehaviour
{
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float speed;

    Rigidbody2D rb2;

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    public void Initialize(Vector3 direction, Quaternion rotation)
    {
        this.direction = direction;
        transform.rotation = rotation;

        Launch();
    }

    private void Launch()
    {
        Vector3 dir = direction.normalized * speed;
        rb2.AddForce(dir, ForceMode2D.Impulse);
    }
}
