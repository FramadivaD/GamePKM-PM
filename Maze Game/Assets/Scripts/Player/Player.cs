using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Joystick Analog")]
    public GameObject mainCircle;
    public GameObject outCircle;
    public bool isJoystick = true;
    private Vector2 circleDir;

    [Header("Player Config")]
    private PlayerHealth health;
    private PlayerLogin login;

    [Header("UI Element")]
    [SerializeField] private Text nameTag;
    [SerializeField] private Button interactableButton;

    [Header("Config")]
    [SerializeField] private float movementSpeed;

    private Animator anim;
    private Rigidbody2D rb;
    private Camera mainCamera;

    [SerializeField] private string playerName = "Player";
    public string PlayerName
    {
        get { return playerName; }
        set
        {
            playerName = value;
            nameTag.text = playerName;
        }
    }

    [SerializeField] private bool allowMove = false;
    public bool AllowMove
    {
        get { return allowMove; }
        set { allowMove = value; }
    }

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        login = GetComponent<PlayerLogin>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    void Update()
    {
        if(GameManager.Instance.AllowEntityMove 
            && GameManager.Instance.AllowPlayerMove
            && AllowMove)
        {
            MovementController();
        }
        AnimationController();
    }

    private void MovementController()
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

        rb.velocity = new Vector2(moveX, moveY) * movementSpeed * Time.deltaTime;
    }

    private void AnimationController()
    {
        if (rb.velocity.x > 0)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isWalkL", false);
            anim.SetBool("isWalkR", true);
        }
        else if (rb.velocity.x < 0)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isWalkL", true);
            anim.SetBool("isWalkR", false);
        }
        else if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isWalkL", false);
            anim.SetBool("isWalkR", false);
        }
        else if (rb.velocity.y != 0 && rb.velocity.x == 0)
        {
            anim.SetBool("isWalk", true);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Treasure") interactableButton.interactable = true;
        if (other.tag == "GateOpen") interactableButton.interactable = true;

        if (other.tag == "Enemy")
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                rb.AddForce(Vector2.right * 3f);
                Debug.Log("damage from right");
            }
            else if (other.gameObject.transform.position.x <= transform.position.x)
            {
                rb.AddForce(Vector2.right * -3f);
                Debug.Log("damage from left");
            }
            health.CurrentHealth -= 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Treasure") interactableButton.interactable = false;
        if (other.tag == "GateOpen") interactableButton.interactable = false;
    }

    private void Die()
    {
        Debug.Log("Player Dies.");
    }

    #region All about events

    private void SubscribeEvents()
    {
        login.OnSubmitNameSuccess += OnSubmitNameSuccess;
        health.OnDied += Die;
    }

    private void UnsubscribeEvents()
    {
        login.OnSubmitNameSuccess -= OnSubmitNameSuccess;
        health.OnDied -= Die;
    }

    private void OnSubmitNameSuccess(string playerName)
    {
        PlayerName = playerName;
        AllowMove = true;
    }

    #endregion
}
