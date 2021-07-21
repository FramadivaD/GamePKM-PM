using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Config")]
    private JoystickController joystickController;
    private PlayerHealth health;
    private PlayerLogin login;

    [Header("UI Element")]
    [SerializeField] private Text nameTag;
    [SerializeField] private Button interactableButton;

    [Header("Config")]
    [SerializeField] private float movementSpeed;

    private Animator anim;
    private Rigidbody2D rb;

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
        joystickController = GetComponent<JoystickController>();
        health = GetComponent<PlayerHealth>();
        login = GetComponent<PlayerLogin>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    void Update()
    {
        AnimationController();
    }

    private void MovementController(Vector2 movement)
    {
        //Player Move
        if (GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowPlayerMove
            && AllowMove)
        {
            rb.velocity = movement * movementSpeed * Time.deltaTime;
        }
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

        if (other.tag == "damage")
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
        joystickController.OnMovingJoystick += MovementController;
    }

    private void UnsubscribeEvents()
    {
        login.OnSubmitNameSuccess -= OnSubmitNameSuccess;
        health.OnDied -= Die;
        joystickController.OnMovingJoystick -= MovementController;
    }

    private void OnSubmitNameSuccess(string playerName)
    {
        PlayerName = playerName;
        AllowMove = true;
    }

    #endregion
}
