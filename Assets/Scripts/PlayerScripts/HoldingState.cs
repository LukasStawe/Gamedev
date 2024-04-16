using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldingState : IState
{
    public PlayerScript playerScript;

    private GameObject heldObj;
    private Rigidbody heldObjRig;
    private float throwForce;
    private Transform holdPoint;

    Vector3 velocity;
    bool isGrounded;

    float xRotation = 0f;
    float yRoation = 0f;

    float moving = 0;

    public HoldingState(PlayerScript playerScript)
    {
        this.playerScript = playerScript;
        this.holdPoint = playerScript.holdPoint;
        //this.throwForce = playerScript.throwForce;
    }

    public void OnEnter()
    {
        //heldObj = playerScript.heldObj;
        //heldObjRig = playerScript.heldObjRig;

        heldObjRig.useGravity = false;
        heldObjRig.drag = 10;
        heldObjRig.constraints = RigidbodyConstraints.FreezeRotation;

        heldObj.transform.parent = playerScript.holdPoint;
    }

    public void OnExit()
    {
        //playerScript.heldObj = null;
        //playerScript.heldObjRig = null;
    }

    public void Tick()
    {
        MoveObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Drop();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            Throw();
        }

        #region Movement-Script
        isGrounded = Physics.CheckSphere(playerScript.groundCheck.position, playerScript.groundDistance, playerScript.groundMask);

        if (velocity.y < 0 && isGrounded)
        {
            velocity.y = -6f;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Move();
        }
        else
        {
            moving = 0;
            playerScript.playerAnimator.SetFloat("Moving", moving);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jumping");
            Jump();
        }

        velocity.y += playerScript.gravity * Time.deltaTime;
        //playerScript.controller.Move(velocity * Time.deltaTime);
        playerScript.rigidBody.velocity = velocity * Time.deltaTime;

        #endregion

        #region Mouse-Script
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            LookAround();
        }
        #endregion
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(playerScript.jumpHeight * -2f * playerScript.gravity);
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moving = x + z;
        playerScript.playerAnimator.SetFloat("Moving", Mathf.Abs(moving));

        //Vector3 move = playerScript.controller.transform.right * x + playerScript.controller.transform.forward * z;
        Vector3 move = playerScript.rigidBody.transform.right * x + playerScript.rigidBody.transform.forward * z;

        //playerScript.controller.Move(move * playerScript.speed * Time.deltaTime);
        playerScript.rigidBody.velocity = move * playerScript.speed * Time.deltaTime;
    }

    public void LookAround()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * playerScript.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * playerScript.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRoation += mouseX;

        playerScript.transform.localRotation = Quaternion.Euler(xRotation, yRoation, 0);
        playerScript.playerBody.Rotate(Vector3.up * mouseX);
    }

    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdPoint.position) > 0.1f)
        {
            Vector3 moveDirection = (holdPoint.position - heldObj.transform.position);
            heldObjRig.AddForce(moveDirection * throwForce);
        }
    }

    private void Drop()
    {
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

        heldObjRig.AddForce(playerScript.gameObject.transform.forward * throwForce);

        heldObj.transform.parent = null;
        heldObj = null;
        heldObjRig = null;

        playerScript.isHolding = false;
    }

    public void FixedTick()
    {
    }
}
