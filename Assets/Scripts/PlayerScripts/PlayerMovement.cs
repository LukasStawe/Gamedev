using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private PlayerScript player;

    private Vector3 moveVector;
    private Vector3 camRotation = Vector3.zero;
    private Vector2 move;
    private readonly int STANDARD_DRAG = 15;

    [SerializeField] private Transform head;
    private readonly float HEAD_STANDING = 0.94f;
    private readonly float HEAD_CROUCHING = 0.5f;

    private Rigidbody rb;

    private bool _isGrounded = true;
    private bool isGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            if (value == false && value != isGrounded)
            {
                player.speed = AIR_SPEED;
                rb.drag = 0;
            } else if (value == true && value != isGrounded){
                player.speed = NORMAL_SPEED;
                rb.drag = STANDARD_DRAG;
            }
            _isGrounded = value;
        }
    }
    private bool isCrouched;

    private readonly float PLAYERHEIGHT = 2.3f;

    private readonly float AIR_SPEED = 0.1f;
    private readonly float NORMAL_SPEED = 4f;
    private readonly float SPRINT_SPEED = 6f;
    private readonly float CROUCH_SPEED = 2f;

    private bool isSprinting = false;

    LayerMask usables;

    private GameObject heldObj;
    private Rigidbody heldObjRig;
    [SerializeField]
    private float throwForce = 20.0f;

    private InputAction sprintAction;

    private void Awake()
    {
        StartCoroutine(LateFixedUpdate());
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerScript.Instance;
        CinemachineCore.GetInputAxis = GetAxisCustom;
        sprintAction = player.actions.actionMaps[0].FindAction("Sprint");
        rb = GetComponent<Rigidbody>();
        usables = LayerMask.GetMask("Items", "Readables", "Interactables", "Consumables", "Container");
    }

    private void Update()
    {
        GroundCheck();
        if (player.isHolding)
        {
            MoveObject();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        //player.rigidBody.MoveRotation(Quaternion.Euler(0, camRotation.y, 0));
    }

    private void LateUpdate()
    {
        //player.cam.transform.rotation = Quaternion.Euler(camRotation);
        //transform.localRotation = Quaternion.Euler(0, camRotation.y, 0);
    }

    public float GetAxisCustom(string axisName)
    {       
        if (player.AnyUIActive()) return 0f;
        return Input.GetAxis(axisName);
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        //PlaySound();
    }

    private void OnJump()
    {
        if (isCrouched)
        {
            StandUp();
            return;
        }

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * player.jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCrouch()
    {
        if (isCrouched)
        {
            StandUp();
        } else
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouched = true;

        player.speed = CROUCH_SPEED;
        Vector3 position = head.localPosition;
        position.y = HEAD_CROUCHING;
        head.localPosition = position;

        player.playerCollider.enabled = false;
        player.crouchCollider.enabled = true;
    }

    private void StandUp()
    {
        if (Physics.Raycast(player.playerBody.position, Vector3.up, PLAYERHEIGHT)) return;

        isCrouched = false;

        player.speed = NORMAL_SPEED;
        Vector3 position = head.localPosition;
        position.y = HEAD_STANDING;
        head.localPosition = position;

        player.playerCollider.enabled = true;
        player.crouchCollider.enabled = false;
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(player.transform.position, Vector3.down, player.groundDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnLook(InputValue value)
    {
        Vector2 rotation = value.Get<Vector2>();
        rotation.x *= player.mouseSensitivity;
        rotation.y *= player.mouseSensitivity;
        camRotation.x -= rotation.y;
        camRotation.x = Mathf.Clamp(camRotation.x, -85f, 85f);
        camRotation.y += rotation.x;
    }

    private void OnSprint()
    {
        if (isCrouched)
        {
            StandUp();
        }

        bool wasPressed = sprintAction.WasReleasedThisFrame();
        Debug.Log(wasPressed);
        if (wasPressed)
        {
            isSprinting = true;
            player.speed = NORMAL_SPEED;
        } else
        {
            isSprinting = false;
            player.speed = SPRINT_SPEED;
        }
    }

    private void Move()
    {
        //Maybe forward instead of up.
        Vector3 camUpNormalized = player.cam.transform.forward;
        camUpNormalized.y = 0f;
        camUpNormalized.Normalize();
        Vector3 camRightNormalized = player.cam.transform.right;
        camRightNormalized.y = 0f;
        camRightNormalized.Normalize();

        moveVector = Vector3.zero;
        moveVector = camRightNormalized * move.x + camUpNormalized * move.y;
        moveVector.y = 0f;
        moveVector.Normalize();

        //player.rigidBody.MovePosition(player.rigidBody.position + moveVector * player.speed * Time.fixedDeltaTime);
        player.rigidBody.AddForce(moveVector * player.speed, ForceMode.VelocityChange);
    }

    public void OnInteract()
    {
        if (player.isHolding)
        {
            Drop();
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            InteractWithItem();
        }
    }

    public void OnSecondaryInteract()
    {
        if (player.isHolding) return;

        Ray ray = player.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, player.interactRadius, out hit, player.interactRange, player.interactables))
        {
            if(hit.collider.tag == "hasSecondary")
            {
                if (hit.collider.TryGetComponent(out ConsumablePickup consumable))
                {
                    consumable.Eat();
                } else if (hit.collider.TryGetComponent(out WallTorch wallTorch))
                {
                    wallTorch.SecondaryInteract();
                }
            }
        }
    }

    public void InteractWithItem()
    {
        Ray ray = player.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, player.interactRadius, out hit, player.interactRange, usables))
        {
            if (hit.collider.TryGetComponent(out ParentInteractables interactables))
            {
                if (hit.collider.gameObject.tag == "canPickUp")
                {
                    interactables.Interact();
                    PickupObject(hit.collider.gameObject);
                }
                else
                {
                    interactables.Interact();
                }
            }
        }
    }

    public void PickupObject(GameObject pickupObj)
    {
        player.isHolding = true;
        heldObj = pickupObj;
        heldObjRig = pickupObj.GetComponent<Rigidbody>();

        heldObjRig.useGravity = false;
        heldObjRig.drag = 10;
        heldObjRig.constraints = RigidbodyConstraints.FreezeRotation;

        heldObj.transform.parent = player.holdPoint;
    }
    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, player.holdPoint.position) > 0.1f)
        {
            Vector3 moveDirection = (player.holdPoint.position - heldObj.transform.position);
            heldObjRig.AddForce(moveDirection * throwForce);
        }
    }

    private void Drop()
    {
        Debug.Log("Object dropped");

        heldObjRig.useGravity = true;
        heldObjRig.drag = 1;
        heldObjRig.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null;
        heldObj = null;
        heldObjRig = null;

        player.isHolding = false;
    }

    private void Throw()
    {
        heldObjRig.useGravity = true;
        heldObjRig.drag = 1;
        heldObjRig.constraints = RigidbodyConstraints.None;

        heldObjRig.AddForce(player.cam.transform.forward * throwForce, ForceMode.Impulse);

        heldObj.transform.parent = null;
        heldObj = null;
        heldObjRig = null;

        player.isHolding = false;
    }

    public void OnAttack()
    {
        if (player.isHolding)
        {
            Throw();
        } else
        {
            //Attack();
        }
    }

    public void OnSecondaryAction()
    {
        if (player.isHolding)
        {
            Drop();
        }
    }

    private IEnumerator LateFixedUpdate()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;

            player.rigidBody.MoveRotation(Quaternion.AngleAxis(camRotation.y, Vector3.up));


        }        
    }
}

