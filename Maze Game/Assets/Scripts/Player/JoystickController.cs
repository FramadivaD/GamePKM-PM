using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [Header("Joystick Analog")]
    public GameObject mainCircle;
    public GameObject outCircle;
    public bool isJoystick = true;

    private Vector2 circleDir;
    private Camera mainCamera;

    public delegate void JoystickEventHandler(Vector2 delta);
    public event JoystickEventHandler OnMovingJoystick;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        outCircle.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowPlayerMove)
        {
            Controller();
        }
    }

    private void Controller()
    {
        //Joystick Control
        if (isJoystick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseDir = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseDis = mouseDir - mainCamera.transform.position;
                if (mouseDis.x <= -2f && mouseDis.y <= 0)
                {
                    outCircle.SetActive(true);
                    outCircle.transform.position = new Vector3(mouseDir.x, mouseDir.y, outCircle.transform.position.z);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                outCircle.SetActive(false);
                circleDir = Vector2.zero;
            }

            if (outCircle.activeSelf)
            {
                Vector2 circleDis = mainCamera.ScreenToWorldPoint(Input.mousePosition) - outCircle.transform.position;
                circleDir = Vector2.ClampMagnitude(circleDis, 1f);
                mainCircle.transform.position = new Vector2(outCircle.transform.position.x + circleDir.x, outCircle.transform.position.y + circleDir.y);
            }
        }
        else if (!isJoystick)
        {
            outCircle.SetActive(false);
        }

        //Player Move
        float moveX = 0;
        float moveY = 0;
        if (circleDir.x >= 0.2f || circleDir.x <= -0.2f)
        {
            moveX = circleDir.x;
        }
        if (circleDir.y >= 0.2f || circleDir.y <= -0.2f)
        {
            moveY = circleDir.y;
        }

        OnMovingJoystick?.Invoke(new Vector2(moveX, moveY));
    }
}
