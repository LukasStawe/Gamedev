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
        Time.timeScale = 1;
        playerInput = playerScript.GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        usables = LayerMask.GetMask("Items", "Readables", "Interactables", "Consumables", "Container");
        playerScript.crossHair.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        Time.timeScale = 0;
        playerScript.crossHair.gameObject.SetActive(false);
        playerInput.actions.FindActionMap("Player").Disable();
    }

    public void Tick()
    {
        #region Movement-Script

        //if (Input.GetButtonDown("Sprint") && !isCrouched)
        //{
        //    playerScript.speed += 2f;
        //}
        //if (Input.GetButtonUp("Sprint") && !isCrouched)
        //{
        //    playerScript.speed = 6f;
        //}

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
    }

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

    public void FixedTick()
    {
    }
}
