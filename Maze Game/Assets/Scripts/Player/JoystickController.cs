using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public bool active = true;

    [SerializeField] private Player player;

    [Header("Joystick Analog")]
    public GameObject mainCircle;
    public GameObject outCircle;

    private Vector2 circleDir;
    private Camera mainCamera;

    public delegate void JoystickEventHandler(Vector2 delta);
    public event JoystickEventHandler OnMovingJoystick;

    public delegate bool JoystickConditionEventHandler();
    public event JoystickConditionEventHandler OnCheckConditionJoystick = () => true;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        outCircle.SetActive(false);

        if (PhotonNetwork.connected)
        {
            if (player.pv.isMine)
            {
                mainCamera.transform.parent = null;
            }
        } else
        {
            mainCamera.transform.parent = null;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsPaused
            && GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowPlayerMove
            && !GameManager.Instance.WinnerWasAnnounced
            && !player.questionManager.questionContainer.activeSelf
            && !player.gateManager.questionContainer.activeSelf)
        {
            if (PhotonNetwork.connected)
            {
                if (player.pv.isMine && (!PhotonNetwork.player.IsMasterClient || MultiplayerNetworkMaster.Instance.testClientSingle)) {
                    Controller();
                }
            } else
            {
                Controller();
            }
        } else
        {
            ResetPosition();
        }
    }

    private void Controller()
    {
        //Joystick Control
        if (active && OnCheckConditionJoystick.Invoke())
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 0)
                {
                    bool tapCondition = Input.GetTouch(0).phase == TouchPhase.Began;
                    bool holdCondition =
                        Input.GetTouch(0).phase == TouchPhase.Moved
                        || Input.GetTouch(0).phase == TouchPhase.Stationary;
                    bool releaseCondition = Input.GetTouch(0).phase == TouchPhase.Ended;

                    Controlling(tapCondition, holdCondition, releaseCondition, Input.GetTouch(0).position);
                }
            }
            else
            {
                bool tapCondition = Input.GetMouseButtonDown(0);
                bool holdCondition = Input.GetMouseButton(0);
                bool releaseCondition = Input.GetMouseButtonUp(0);

                Controlling(tapCondition, holdCondition, releaseCondition, Input.mousePosition);
            }
        }
    }

    private void Controlling(bool tapCondition, bool holdCondition, bool releaseCondition, Vector3 touchPos)
    {
        if (tapCondition)
        {
            Vector3 mouseDir = mainCamera.ScreenToWorldPoint(touchPos);
            Vector3 mouseDis = mouseDir - mainCamera.transform.position;
            if (mouseDis.x <= -2f && mouseDis.y <= 0)
            {
                outCircle.SetActive(true);
                outCircle.transform.position = new Vector3(mouseDir.x, mouseDir.y, outCircle.transform.position.z);
            }
        }

        if (releaseCondition)
        {
            outCircle.SetActive(false);
            circleDir = Vector2.zero;
        }

        if (outCircle.activeSelf)
        {
            Vector2 circleDis = mainCamera.ScreenToWorldPoint(touchPos) - outCircle.transform.position;
            circleDir = Vector2.ClampMagnitude(circleDis, 1f);
            mainCircle.transform.position = new Vector2(outCircle.transform.position.x + circleDir.x, outCircle.transform.position.y + circleDir.y);
        }

        /*
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
        */

        OnMovingJoystick?.Invoke(circleDir);
    }

    public void ResetPosition()
    {
        Controlling(false, false, true, Vector3.zero);
    }
}
