using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    #region Singleton
    private static PlayerScript instance;

    public static PlayerScript Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("PlayerScript not instantiated.");
            }

            return instance;
        }
    }
    #endregion

    #region StateMachin-Variables
    public StateMachine stateMachine;

    public InteractingState interactingState;
    public IdleState idleState;
    public InventoryState inventoryState;
    public HoldingState holdingState;
    #endregion

    #region Movement-variables
    public CharacterController controller;

    public float speed = 8f;
    public float gravity = -9.8f;
    public float jumpHeight = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator playerAnimator;

    public Transform playerBody;

    public BoxCollider playerCollider;
    public BoxCollider crouchCollider;
    #endregion

    #region Combat-Variables
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public ScriptableWeapon weaponScript;

    public float nextAttackTime = 0f;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public bool weaponEquipped;
    #endregion

    #region Mouse-Variables
    public float mouseSensitivity = 2f;
    public Image crossHair;

    #endregion

    #region Interaction-Variables
    public Transform interactPoint;
    public LayerMask interactables;

    public LayerMask items;

    public LayerMask readables;

    public LayerMask consumables;

    public Camera cam;

    public float interactRange = 1.6f;
    public float interactRadius = 0.02f;

    public Vector3 interactVector = new(0.01f, 0.01f, 0.9f);

    public bool isHolding = false;
    public Transform holdPoint;
    public Transform dropPoint;
    #endregion

    #region UI-Variables
    public InventoryUI inventoryScript;
    public ReadableUI readableUIScript;
    public PlayerInteractUI interactUI;
    public LootUI lootUI;
    public DIalogueUI dialogueUI;
    public JournalUI journalUI;
    #endregion


    private void Awake()
    {
        instance = this;

        stateMachine = new StateMachine();

        idleState = new IdleState(this);
        interactingState = new InteractingState(this);
        inventoryState = new InventoryState(this);
        holdingState = new HoldingState(this);

        stateMachine.AddTransition(idleState, interactingState, () =>  readableUIScript.isShown == true || lootUI.isShown == true || dialogueUI.isShown == true);

        stateMachine.AddTransition(interactingState, idleState, () => readableUIScript.isShown == false && lootUI.isShown == false && dialogueUI.isShown == false);

        stateMachine.AddTransition(idleState, inventoryState, () => inventoryScript.isShown == true || journalUI.isShown == true);

        stateMachine.AddTransition(inventoryState, idleState, () => inventoryScript.isShown == false && journalUI.isShown == false);
    }

    //Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        stateMachine.SetState(idleState);
        weaponEquipped = false;
    }

    //Update is called once per frame
    private void Update()
    {
        stateMachine.Tick();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

}

