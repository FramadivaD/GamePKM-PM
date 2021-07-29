using UnityEngine;
using System.Collections;

public class SpectatorController : MonoBehaviour
{
    public static SpectatorController Instance { get; private set; }

    [SerializeField] Camera mainCamera;

    public float sensitivity = 1;

    public float borderTop = 50;
    public float borderRight = 50;
    public float borderBottom = -50;
    public float borderLeft = -50;

    private Vector3 initWorldPos;
    private Vector3 initScreenPos;

    private Transform targetLerp;

    private float height = 0;

    private void Awake()
    {
        Instance = this;

        height = transform.position.z;
    }

    private void Start()
    {
        StartCoroutine(RandomPlayer(5.0f));

        targetLerp = transform;
    }

    private void Update()
    {
        if (targetLerp)
        {
            Vector3 lerppos = targetLerp.position;
            lerppos.z = height;

            transform.position = Vector3.Lerp(transform.position, lerppos, 0.2f);
        }
    }

    public void Track(Transform tra)
    {
        StopAllCoroutines();

        targetLerp = tra;
    }

    private IEnumerator RandomPlayer(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0) {
            GameObject randPlayer = players[Random.Range(0, players.Length)];

            targetLerp = randPlayer.transform;
        }

        StartCoroutine(RandomPlayer(timer));
    }

    /*
    private void Update()
    {
        if (Application.isMobilePlatform)
        {

        } else
        {
            bool press = Input.GetMouseButtonDown(0);
            bool hold = Input.GetMouseButton(0);
            bool leave = Input.GetMouseButtonUp(0);

            Controller(press, hold, leave, Input.mousePosition);
        }
    }

    private void Controller(bool press, bool hold, bool leave, Vector3 screenPos)
    {
        screenPos.z = 10;

        if (press)
        {
            initWorldPos = transform.position;

            initScreenPos = screenPos;
        }
        else if (hold)
        {
            Vector3 initPos = mainCamera.ScreenToWorldPoint(screenPos);
            Vector3 pos = mainCamera.ScreenToWorldPoint(screenPos);

            transform.position = initWorldPos + (initPos - pos) * sensitivity;
        }
    }
    */
}
