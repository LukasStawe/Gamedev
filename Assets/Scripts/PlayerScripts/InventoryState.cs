using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : IState
{
    public PlayerScript playerScript;

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
    }

    public void OnExit()
    {
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
