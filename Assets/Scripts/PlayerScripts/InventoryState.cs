using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryState : IState
{
    public PlayerScript playerScript;

    private PlayerInput playerInput;

    public InventoryState(PlayerScript playerScript)
    {
        this.playerScript = playerScript;
    }

    public void FixedTick()
    {
    }

    public void OnEnter()
    {
        Debug.Log("Entering Inventory State.");
        HideMouse();
        playerInput = playerScript.GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("UI").Enable();
    }

    public void OnExit()
    {
        playerInput.actions.FindActionMap("UI").Disable();
    }

    public void Tick()
    {
        if (playerScript.inventoryScript.isShown && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I)))
        {
            playerScript.inventoryScript.Hide();
        }

        if (playerScript.journalUI.isShown && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Tab)))
        {
            playerScript.journalUI.Hide();
        }
    }

    void HideMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
