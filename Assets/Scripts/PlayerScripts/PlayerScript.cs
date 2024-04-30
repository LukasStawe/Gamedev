using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    //public CharacterController controller;
    public Rigidbody rigidBody;

    public float speed = 8f;
    public float gravity = -9.8f;
    public float jumpHeight = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator playerAnimator;

    public Transform playerBody;

    public CapsuleCollider playerCollider;
    public CapsuleCollider crouchCollider;
    #endregion

    #region Combat-Variables
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask enemies;

    public ScriptableWeapon weaponScript;

    public float nextAttackTime = 0f;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    public DrawBar drawBar;

    public bool weaponEquipped;

    public SkinnedMeshRenderer bowString;
    public SkinnedMeshRenderer bowHandle;
    public GameObject bow;

    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    private Coroutine drawArrow;
    private Vector3 BOW_STANDARD_POSITION = new Vector3(-0.00318349455f, 0.0106449518f, 0.00206251559f);
    private Quaternion BOW_STANDARD_ROTATION = new Quaternion(0.640023887f, 0.757192492f, 0.102656633f, 0.0805639997f);
    private Vector3 BOW_SHOOT_POSITION = new Vector3(0.00456999987f, -0.00380000006f, 0.00461000018f);
    private Quaternion BOW_SHOOT_ROTATION = new Quaternion(0.184796646f, 0.974401176f, 0.0186493937f, 0.126668304f);
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

    public InputActionAsset actions;
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

    public bool AnyUIActive()
    {
        return readableUIScript.isShown || lootUI.isShown || dialogueUI.isShown || inventoryScript.isShown || journalUI.isShown;
    }

    public void DrawBow()
    {
        drawArrow = StartCoroutine(DrawRangedWeapon());
        drawBar.Toggle(true);
    }

    public void ShootBow()
    {
        StopCoroutine(drawArrow);
        if (bowString.GetBlendShapeWeight(0) * 2 >= 75)
        {
            //TODO Shoot an arrow with speed and damage based on drawprogress
            GameObject arrowInstance;
            Quaternion rotation = arrowSpawn.rotation;
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            Debug.Log(rotation.x);
            arrowInstance = Instantiate(arrowPrefab, arrowSpawn.position, rotation);
            arrowInstance.GetComponent<Rigidbody>().AddForce(0.5f * bowString.GetBlendShapeWeight(0) * cam.transform.forward, ForceMode.Impulse);
        }
        bowString.SetBlendShapeWeight(0, 0);
        bowHandle.SetBlendShapeWeight(0, 0);
        bow.transform.localPosition = BOW_STANDARD_POSITION;
        bow.transform.localRotation = BOW_STANDARD_ROTATION;
        drawBar.Toggle(false);
    }

    private IEnumerator DrawRangedWeapon()
    {
        bow.transform.localPosition = BOW_SHOOT_POSITION;
        bow.transform.localRotation = BOW_SHOOT_ROTATION;
        while (bowString.GetBlendShapeWeight(0) <= 50f && bowHandle.GetBlendShapeWeight(0) <= 50f)
        {
            bowString.SetBlendShapeWeight(0, bowString.GetBlendShapeWeight(0) + 25 * Time.deltaTime);
            bowHandle.SetBlendShapeWeight(0, bowHandle.GetBlendShapeWeight(0) + 25 * Time.deltaTime);
            drawBar.setValue(bowString.GetBlendShapeWeight(0) * 2);
            yield return null;
        }
    }
}

