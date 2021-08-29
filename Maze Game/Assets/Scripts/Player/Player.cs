using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Extensione.Audio;

public class Player : MonoBehaviour
{
    [Header("Network")]
    public PhotonView pv;

    [Header("Player Config")]
    public TeamType teamType;
    [SerializeField] private SpriteRenderer playerHatGraphic;
    [SerializeField] private SpriteRenderer playerBodyGraphic;
    [SerializeField] private Sprite[] teamHatSprites;
    [SerializeField] private Sprite[] teamBodySprites;

    [Header("Player Manager")]
    public PlayerGameManagerReference gameManagerReference;
    public QuestionManager questionManager;
    public GateManager gateManager;
    public InventoryManager inventoryManager;
    public WeaponManager weaponManager;
    public PlayerCompass playerCompass;

    public JoystickController joystickController;
    public PlayerHealth health;
    public PlayerLogin login;

    [Header("Player Object")]
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerLegs;
    [SerializeField] private GameObject playerWeapon;

    [Header("UI Element")]
    [SerializeField] private GameObject etcObjects;

    [SerializeField] private Text nameTag;
    [SerializeField] private Button interactableButton;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Text pauseTeamText;

    public GameObject GameplayUI { get { return gameplayUI; } }
    public GameObject PauseUI { get { return pauseUI; } }
    public Text PauseTeamText { get { return pauseTeamText; } }

    [Header("Config")]
    [SerializeField] private float movementSpeed;

    private Animator anim;
    private Rigidbody2D rb;

    private delegate void PlayerInteractEventHandler();
    private event PlayerInteractEventHandler OnInteract;

    [SerializeField] private string playerName = "Player";

    [SerializeField] private GameObject dieSpawn;

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
        return
            !GameManager.Instance.IsPaused
            && GameManager.Instance.AllowEntityMove
            && GameManager.Instance.AllowPlayerMove
            && !GameManager.Instance.WinnerWasAnnounced
            && AllowMove
            && !questionManager.questionContainer.activeSelf
            && !gateManager.questionContainer.activeSelf;
    }

    [Header("Audio")]
    [SerializeField] private AudioClip damagedSFX;
    [SerializeField] private AudioClip dieSFX;
    [SerializeField] private AudioClip reviveSFX;

    public void Initialize(TeamType team)
    {
        if (pv.isMine)
        {
            teamType = team;
            login.SubmitName();

            playerCompass.FindAllChest();
            playerCompass.FindMainGate();
            playerCompass.FindEnemyBoss();

            pv.RPC("SyncTeamColorGraphic", PhotonTargets.AllBuffered, (int)teamType);
            pv.RPC("DisableEtcObjects", PhotonTargets.OthersBuffered);
        } else
        {
            rb.isKinematic = true;
        }
    }

    [PunRPC]
    private void SyncTeamColorGraphic(int teamTypeInt)
    {
        // brarti red team
        if (teamTypeInt == 0)
        {
            playerHatGraphic.sprite = teamHatSprites[LobbyPlayerList.PlayerRedTeamColorIndex];
            playerBodyGraphic.sprite = teamBodySprites[LobbyPlayerList.PlayerRedTeamColorIndex];
        } else if (teamTypeInt == 1) // berarti blue team
        {
            playerHatGraphic.sprite = teamHatSprites[LobbyPlayerList.PlayerBlueTeamColorIndex];
            playerBodyGraphic.sprite = teamBodySprites[LobbyPlayerList.PlayerBlueTeamColorIndex];
        }
    }

    [PunRPC]
    private void DisableEtcObjects()
    {
        etcObjects.SetActive(false);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        gameManagerReference.Initialize(this);

        questionManager.Initialize(this);
        gateManager.Initialize(this);
        inventoryManager.Initialize(this);
        weaponManager.Initialize(this);
        playerCompass.Initialize(this);

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    void Update()
    {
        if (pv.isMine) {
            AnimationController();

            ExecuteEquippedItem();
        }
    }

    private void Die()
    {
        Debug.Log("Player Dies.");

        if (PhotonNetwork.connected)
        {
            if (pv.isMine)
            {
                pv.RPC("DieRPC", PhotonTargets.AllBuffered, transform.position);

                AllowMove = false;
                rb.velocity = Vector2.zero;
                joystickController.ResetPosition();

                DisableGraphic();

                AudioManager.Instance.PlaySFXOnce(dieSFX);

                Invoke("ResetHealth", 5);
            }
        }
        else
        {
            DieRPC(transform.position);

            AllowMove = false;
            rb.velocity = Vector2.zero;
            joystickController.ResetPosition();

            DisableGraphic();

            Invoke("ResetHealth", 5);
        }
    }

    private void ResetHealth()
    {
        Vector3 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-3.6f, 3.6f));
        transform.position = randPos;

        AllowMove = true;

        AudioManager.Instance.PlaySFXOnce(reviveSFX);

        health.ResetHealth();

        EnableGraphic();
    }

    [PunRPC]
    private void DieRPC(Vector3 diePos)
    {
        Instantiate(dieSpawn, diePos, Quaternion.identity);
    }

    #region All About Movement

    private void MovementController(Vector2 movement)
    {
        //if (pv.isMine || MultiplayerNetworkMaster.Instance.testClientSingle)
        if (true)
        {
            //Player Move
            if (CheckAllowedMove())
            {
                rb.velocity = movement * movementSpeed * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
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
                TakeWeaponOrb(other.gameObject);
            };
        }
        else if (other.tag == "FragmentOrb")
        {
            interactableButton.interactable = true;
            OnInteract = () => {
                TakeFragmentOrb(other.gameObject);
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

            AudioManager.Instance.PlaySFXOnce(damagedSFX);
        }
        else if (other.tag == "EnemyProjectile")
        {
            if (other.TryGetComponent(out WeaponProjectile projectile))
            {
                if (projectile.IsTrigger)
                {
                    health.CurrentHealth -= projectile.Damage;
                    projectile.TerminateProjectile();

                    AudioManager.Instance.PlaySFXOnce(damagedSFX);

                    projectile.DisableTrigger();
                }
            }
        }
        else if (other.tag == "EnemyBoss")
        {
            if (other.TryGetComponent(out EnemyBoss boss))
            {
                health.CurrentHealth -= boss.TouchDamage;

                AudioManager.Instance.PlaySFXOnce(damagedSFX);
            }
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
        else if (other.tag == "FragmentOrb")
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

    private void TakeWeaponOrb(GameObject interactedObject)
    {
        if (interactedObject.TryGetComponent(out WeaponOrb orb))
        {
            WeaponInventory weapon = orb.TakeWeapon(this);
            if (weapon != null)
            {
                OnInteract = null;
            }
        }
    }

    private void TakeFragmentOrb(GameObject interactedObject)
    {
        if (interactedObject.TryGetComponent(out MainGateFragmentOrb orb))
        {
            MainGateFragment fragment = orb.TakeFragment(this);
            if (fragment != null)
            {
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
            EquippedItem = null;
        }
    }

    public void DropAllItems()
    {
        inventoryManager.DropAllItems();
        EquippedItem = null;
    }

    public void ExecuteEquippedItem()
    {
        if (CheckAllowedMove()) {
            if (EquippedItem != null)
            {
                if (EquippedItem is WeaponInventory)
                {
                    WeaponInventory weapon = EquippedItem as WeaponInventory;
                    weaponManager.ExecuteWeapon(weapon);
                } else
                {
                    weaponManager.ExecuteWeapon(null);
                }
            } else
            {
                weaponManager.ExecuteWeapon(null);
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

    [PunRPC]
    public void ChangeDisplayName(string displayName)
    {
        if (PhotonNetwork.connected)
        {
            if (pv.isMine)
            {
                PlayerName = displayName;
                AllowMove = true;

                pv.RPC("ChangeDisplayName", PhotonTargets.OthersBuffered, displayName);
            }
            else
            {
                PlayerName = displayName;
            }
        }
        else
        {
            PlayerName = displayName;
        }
    }

    private void OnSubmitNameSuccess(string playerName)
    {
        PlayerName = playerName;
    }

    #endregion

    private void EnableGraphic()
    {
        playerHead.SetActive(true);
        playerBody.SetActive(true);
        playerLegs.SetActive(true);
        playerWeapon.SetActive(true);
    }

    private void DisableGraphic()
    {
        playerHead.SetActive(false);
        playerBody.SetActive(false);
        playerLegs.SetActive(false);
        playerWeapon.SetActive(false);
    }
}
