using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Config")]
    public TeamType teamType;

    [Header("Player Manager")]
    public QuestionManager questionManager;
    public GateManager gateManager;
    public InventoryManager inventoryManager;

    public JoystickController joystickController;
    public PlayerHealth health;
    public PlayerLogin login;

    [Header("UI Element")]
    [SerializeField] private Text nameTag;
    [SerializeField] private Button interactableButton;

    [Header("Config")]
    [SerializeField] private float movementSpeed;

    private Animator anim;
    private Rigidbody2D rb;

    private delegate void PlayerInteractEventHandler();
    private event PlayerInteractEventHandler OnInteract;

    [SerializeField] private string playerName = "Player";

    public IInventoryAble EquippedItem { get; private set; }

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

    public bool CheckAllowedMove()
    {
        return GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowPlayerMove
            && AllowMove
            && !questionManager.questionContainer.activeSelf;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        questionManager.Initialize(this);
        gateManager.Initialize(this);
        inventoryManager.Initialize(this);

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

    private void Die()
    {
        Debug.Log("Player Dies.");
    }

    #region All About Movement

    private void MovementController(Vector2 movement)
    {
        //Player Move
        if (CheckAllowedMove())
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

    #endregion

    #region All About Interaction

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Treasure")
        {
            interactableButton.interactable = true;
            OnInteract = () => {
                OpenChest(other.gameObject);
            };
        }
        else if (other.tag == "GateOpen")
        {
            interactableButton.interactable = true;
            OnInteract = () => {
                OpenGate(other.gameObject);
            };
        }
        else if (other.tag == "WeaponOrb")
        {
            interactableButton.interactable = true;
            OnInteract = () => {
                TakeWeapon(other.gameObject);
            };
        }
        else if (other.tag == "damage")
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
        if (other.tag == "Treasure")
        {
            interactableButton.interactable = false;
            OnInteract = null;
        }
        else if (other.tag == "GateOpen")
        {
            interactableButton.interactable = false;
            OnInteract = null;
        }
        else if (other.tag == "WeaponOrb")
        {
            interactableButton.interactable = false;
            OnInteract = null;
        }
    }

    public void InteractAction()
    {
        OnInteract?.Invoke();
    }

    private void OpenChest(GameObject interactedObject)
    {
        if (interactedObject.TryGetComponent(out ChestContainer chest))
        {
            questionManager.OpenQuestion(chest);
            if (chest.IsUnlocked)
            {
                OnInteract = null;
            }
        }
    }

    private void OpenGate(GameObject interactedObject)
    {
        if (interactedObject.TryGetComponent(out Gate gate))
        {
            gateManager.OpenGate(gate, inventoryManager);
            if (gate.IsOpened)
            {
                OnInteract = null;
            }
        }
    }

    private void TakeWeapon(GameObject interactedObject)
    {
        if (interactedObject.TryGetComponent(out WeaponOrb orb))
        {
            WeaponInventory weapon = orb.TakeWeapon(this);
            if (weapon != null)
            {
                EquippedItem = weapon;
                OnInteract = null;
            }
        }
    }

    public void EquipItem(IInventoryAble item)
    {
        if (item != null)
        {
            EquippedItem = item;
        }
    }

    public void DropItem()
    {
        if (EquippedItem != null)
        {
            inventoryManager.DropItem(EquippedItem);
        }
    }

    public void ExecuteEquippedItem()
    {
        if (EquippedItem != null)
        {
            if (EquippedItem is WeaponInventory)
            {
                WeaponInventory weapon = EquippedItem as WeaponInventory;
                if (weapon.weaponType == WeaponType.None)
                {
                    // gak ngapa ngapain
                }
                else if (weapon.weaponType == WeaponType.Basoka)
                {
                    // some logic
                }
            }
        }
    }

    #endregion

    #region All about events

    private void SubscribeEvents()
    {
        login.OnSubmitNameSuccess += OnSubmitNameSuccess;
        health.OnDied += Die;
        joystickController.OnMovingJoystick += MovementController;
        joystickController.OnCheckConditionJoystick += CheckAllowedMove;
    }

    private void UnsubscribeEvents()
    {
        login.OnSubmitNameSuccess -= OnSubmitNameSuccess;
        health.OnDied -= Die;
        joystickController.OnMovingJoystick -= MovementController;
        joystickController.OnCheckConditionJoystick -= CheckAllowedMove;
        OnInteract = null;
    }

    private void OnSubmitNameSuccess(string playerName)
    {
        PlayerName = playerName;
        AllowMove = true;
    }

    #endregion
}
