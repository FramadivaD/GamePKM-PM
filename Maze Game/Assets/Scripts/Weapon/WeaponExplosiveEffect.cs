using UnityEngine;
using System.Collections;

public class WeaponExplosiveEffect : MonoBehaviour
{
    [SerializeField] private float destroyAfter = 5;

    private void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
