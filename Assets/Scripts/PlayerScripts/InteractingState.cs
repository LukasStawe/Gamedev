using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingState : IState
{
    public PlayerScript playerScript;

    public InteractingState (PlayerScript playerScript)
    {
        this.playerScript = playerScript;
    }

    public void FixedTick()
    {
    }

    public void OnEnter()
    {
        Debug.Log("Entering Interacting State.");
        HideMouse();
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if (Input.GetButtonDown("Cancel/Menu") || Input.GetButtonDown("Interact"))
        {
            playerScript.readableUIScript.Hide();
            playerScript.lootUI.Hide();
        }

        if (playerScript.lootUI.isShown && Input.GetButtonDown("Loot All"))
        {
            playerScript.lootUI.LootAll();
        }
    }
    void HideMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}

