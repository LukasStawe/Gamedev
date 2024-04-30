using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : IState
{

    public PlayerScript playerScript;

    Vector3 velocity;
    //Vector3 camUp;
    //Vector3 camRight;
    bool isGrounded;
    bool isCrouched = false;

    float soundStrength = 4f;

    //float xRotation = 0f;
    //float yRoation = 0f;

    LayerMask usables;

    private GameObject heldObj;
    private Rigidbody heldObjRig;

    private float playerHeight = 2.3f;

    [SerializeField]
    private float throwForce = 20.0f;

    private PlayerInput playerInput;

    public IdleState (PlayerScript playerScript)
    {
        this.playerScript = playerScript;
    }

    public void OnEnter()
    {
        playerInput = playerScript.GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        usables = LayerMask.GetMask("Items", "Readables", "Interactables", "Consumables", "Container");
        playerScript.crossHair.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        playerScript.crossHair.gameObject.SetActive(false);
        playerInput.actions.FindActionMap("Player").Disable();
    }

    public void Tick()
    {
        #region Movement-Script

        if (Input.GetButtonDown("Sprint") && !isCrouched)
        {
            playerScript.speed += 2f;
        }
        if (Input.GetButtonUp("Sprint") && !isCrouched)
        {
            playerScript.speed = 6f;
        }

        #endregion

        #region Combat-Script
        if (Time.time >= playerScript.nextAttackTime)
        {

            if (Input.GetButtonDown("Attack") && playerScript.weaponEquipped && !playerScript.isHolding)
            {
                if (playerScript.weaponScript.weapon == ScriptableWeapon.WeaponType.Bow) 
                {
                    playerScript.DrawBow();
                } else
                {
                    Attack();
                    playerScript.nextAttackTime = Time.time + 1f / playerScript.weaponScript.attackSpeed;
                }
            }

        }

        if (Input.GetButtonUp("Attack") && playerScript.weaponEquipped)
        {
            Shoot();
        }
        #endregion

            #region Interact-Script
        if (!playerScript.isHolding)
        {
            if (Input.GetButtonDown("Interact"))
            {
                InteractWithItem();
            }

            //if (Input.GetButtonDown("Unlock"))
            //{
            //    UnlockDoorChest();
            //}
            if (Input.GetButtonDown("Use"))
            {
                EatConsumable();
            }
            // TODO Remove after testing
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TakeDamage(5);
            }
        }
        #endregion

        #region UI-Script
        if (Input.GetButtonDown("Inventory"))
        {
            playerScript.inventoryScript.Show();
        }

        if (Input.GetButtonDown("Journal"))
        {
            playerScript.journalUI.Show();
        }

        ParentInteractables interactable = playerScript.idleState.GetInteractableObject();
        if (interactable != null && !playerScript.isHolding)
        {
            playerScript.interactUI.Show(interactable);
            playerScript.crossHair.color = Color.green;
        }
        else
        {
            playerScript.interactUI.Hide();
            playerScript.crossHair.color = Color.black;
        }
        #endregion

        #region Holding-Script
        if (playerScript.isHolding)
        {
            MoveObject();

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Drop();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Throw();
            }
        }
        #endregion

        #region Mouse-Script
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            //LookAround();
        }
        #endregion
    }

    public void FixedTick()
    {
        //if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        //{
        //    Move();
        //}
    }

    //private void GroundCheck()
    //{
    //    if (Physics.Raycast(playerScript.transform.position, Vector3.down, playerScript.groundDistance))
    //    {
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }
    //}

    //public void Move()
    //{       
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");      

    //    camUp = playerScript.cam.transform.forward;
    //    camUp.y = 0f;
    //    camUp.Normalize();

    //    camRight = playerScript.cam.transform.right;
    //    camRight.y = 0f;
    //    camRight.Normalize();

    //    Vector3 move = Vector3.zero;
    //    move += camUp * z;
    //    move += camRight * x;

    //    playerScript.rigidBody.MovePosition(playerScript.transform.position + move * playerScript.speed * Time.deltaTime);

    //    PlaySound();
    //}

    private void PlaySound()
    {
        //TODO Maybe put floortype into the equation
        float soundDistance = soundStrength * playerScript.rigidBody.velocity.magnitude * (isCrouched ? 0.1f : 1f);

        Collider[] enemies = Physics.OverlapSphere(playerScript.transform.position, soundDistance/2);
        if (enemies.Length > 0)
        {
            foreach (Collider collider in enemies)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Enemy>().enemyIdleState.HearPlayer(playerScript.transform.position);
                }    
            }
        }
    }

    private void Attack()
    {
        //Play Animation
        //playerScript.playerAnimator.SetTrigger("Attack");

        //Detect Enemies in Range of Attack and damage them
        Ray ray = playerScript.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.SphereCast(ray, playerScript.interactRadius, out hit, playerScript.weaponScript.attackRange, playerScript.enemyLayers))
        {
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.GetComponent<Enemy>().takeDamage(playerScript.weaponScript.damageModifier);
            } else if (hit.collider.TryGetComponent(out Destructable destructable))
            {
                destructable.GetComponent<Destructable>().TakeDamage();
            }
        }
    }

    private void Shoot()
    {
        playerScript.ShootBow();        
    }

    public void TakeDamage(int damage)
    {
        if (playerScript.currentHealth - damage < 0) {
            playerScript.currentHealth = 0;
        } else
        {
            playerScript.currentHealth -= damage;
        }

        playerScript.healthBar.SetHealth(playerScript.currentHealth);

        if (playerScript.currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // TODO Add death screen
        Debug.Log("Literally Dead");
    }

    private void OnDrawGizmosSelected()
    {
        if (playerScript.attackPoint == null)
        {
            return;
        }

        //Gizmos.DrawWireSphere(playerScript.attackPoint.position, playerScript.weaponScript.attackRange);

        if (playerScript.interactPoint == null)
        {
            return;
        }

        //Gizmos.DrawWireCube(playerScript.interactPoint.position, playerScript.interactVector * 2);
    }

    //public void LookAround()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    float mouseX = Input.GetAxis("Mouse X") * playerScript.mouseSensitivity;
    //    float mouseY = Input.GetAxis("Mouse Y") * playerScript.mouseSensitivity;

    //    xRotation -= mouseY;
    //    xRotation = Mathf.Clamp(xRotation, -85f, 85f);
    //    yRoation += mouseX;
        
    //    playerScript.transform.localRotation = Quaternion.Euler(0, yRoation, 0);
    //    //playerScript.playerBody.Rotate(Vector3.up * mouseX);

    //    playerScript.cam.transform.rotation = Quaternion.Euler(xRotation, yRoation, 0);
    //}

    public ParentInteractables GetInteractableObject()
    {
        Ray ray = playerScript.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, playerScript.interactRadius, out hit, playerScript.interactRange, usables))
        {
            if (hit.collider.TryGetComponent(out ParentInteractables interactables))
            {
                return interactables;
            }
        }

        return null;
    }

    public void InteractWithItem()
    {

        Ray ray = playerScript.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, playerScript.interactRadius, out hit, playerScript.interactRange, usables))
        {
            if (hit.collider.TryGetComponent(out ParentInteractables interactables))
            {
                if (hit.collider.gameObject.tag == "canPickUp")
                {
                    interactables.Interact();
                    PickupObject(hit.collider.gameObject);
                } else
                {
                    interactables.Interact();
                }
            }
        }
    }

    public void UnlockDoorChest()
    {

        Ray ray = playerScript.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, playerScript.interactRadius, out hit, playerScript.interactRange, playerScript.interactables))
        {
            if (hit.collider.TryGetComponent(out DoorInteraction doorScript))
            {
                if (doorScript.isLocked)
                {
                    doorScript.Unlock();
                }
            } else if (hit.collider.TryGetComponent(out LootChest lootScript))
            {
                if (lootScript.isLocked)
                {
                    lootScript.Unlock();
                }
            }
        }
    }

    public void EatConsumable()
    {
        Ray ray = playerScript.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, playerScript.interactRadius, out hit, playerScript.interactRange, playerScript.consumables))
        {
            if (hit.collider.TryGetComponent(out ConsumablePickup consumable))
            {
                consumable.Eat();
            }
        }
    }

    public void PickupObject(GameObject pickupObj)
    {
        playerScript.isHolding = true;
        heldObj = pickupObj;
        heldObjRig = pickupObj.GetComponent<Rigidbody>();

        heldObjRig.useGravity = false;
        heldObjRig.drag = 10;
        heldObjRig.constraints = RigidbodyConstraints.FreezeRotation;

        heldObj.transform.parent = playerScript.holdPoint;
    }

    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, playerScript.holdPoint.position) > 0.1f)
        {
            Vector3 moveDirection = (playerScript.holdPoint.position - heldObj.transform.position);
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

        playerScript.isHolding = false;
    }

    private void Throw()
    {
        heldObjRig.useGravity = true;
        heldObjRig.drag = 1;
        heldObjRig.constraints = RigidbodyConstraints.None;

        heldObjRig.AddForce(playerScript.cam.transform.forward * throwForce, ForceMode.Impulse);

        heldObj.transform.parent = null;
        heldObj = null;
        heldObjRig = null;

        playerScript.isHolding = false;
    }
}
