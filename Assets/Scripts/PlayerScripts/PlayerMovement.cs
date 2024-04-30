using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private PlayerScript player;

    private Vector3 moveVector;
    private Vector3 camRotation = Vector3.zero;
    private Vector2 move;

    private bool isGrounded;
    private bool isCrouched;

    private readonly float PLAYERHEIGHT = 2.3f;

    private readonly float NORMAL_SPEED = 6f;
    private readonly float SPRINT_SPEED = 8f;
    private readonly float CROUCH_SPEED = 4f;

    private bool isSprinting = false;

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
    }

    private void Update()
    {
        GroundCheck();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();        
    }

    private void LateUpdate()
    {
        //player.cam.transform.rotation = Quaternion.Euler(camRotation);
        //transform.localRotation = Quaternion.Euler(0, camRotation.y, 0);
    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (!player.AnyUIActive())
            {
                return Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (!player.AnyUIActive())
            {
                return Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
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
            player.rigidBody.AddForce(Vector3.up * player.jumpHeight, ForceMode.Impulse);
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

        player.playerCollider.enabled = false;
        player.crouchCollider.enabled = true;
    }

    private void StandUp()
    {
        if (Physics.Raycast(player.playerBody.position, Vector3.up, PLAYERHEIGHT)) return;

        isCrouched = false;

        player.speed = NORMAL_SPEED;

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

        moveVector = Vector3.zero;
        moveVector = player.cam.transform.right * move.x + player.cam.transform.forward * move.y;
        moveVector.y = 0f;

        player.rigidBody.MovePosition(player.rigidBody.position + moveVector * player.speed * Time.fixedDeltaTime);
        //player.rigidBody.MoveRotation(Quaternion.Euler(0, camRotation.y, 0));
        //player.rigidBody.AddForce(moveVector * player.speed, ForceMode.VelocityChange);
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
