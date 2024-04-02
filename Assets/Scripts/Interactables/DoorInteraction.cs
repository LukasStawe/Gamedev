using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

/// <summary>
/// Class for interactable Doors that handles all the door interactions
/// </summary>
public class DoorInteraction : Interactable
{
    //Bool to check if Door is closed
    bool isClosed = true;

    //Bool to check if Door is locked
    public bool isLocked = true;

    //Required Key to open the Door (if its closed)
    public ScriptableKey requiredKey;

    //Reference to the PlayerInventory in GameManager
    private Inventory inventory;

    private float lastOpenRotation;

    private bool isRunning = false;

    private void Start()
    {
        inventory = Inventory.instance;
    }

    //Text to display when looking at the Door according to its status
    public override string interactTextToDisplay
    {
        get
        {
            if (isLocked) return "Press E to Unlock. (Requires " + requiredKey.name + ")";
            else if (isClosed) return "Press E to Open.";
            else return "Press E to Close.";
        }
    }

    /// <summary>
    /// Opens the chest if its not locked and changes the bool
    /// </summary>
    public override void Interact() {
        if (isLocked)
        {
            Unlock();
        } else if (!isRunning) { 
            base.Interact();
            isClosed = !isClosed;
            if (isClosed)
            {
                StartCoroutine(Rotate(-lastOpenRotation, 0.7f));
            } else
            {
                StartCoroutine((Rotate(GetPlayerSide() == true ? -90 : 90, 0.7f)));
            }
        }
    }

    /// <summary>
    /// Unlocks the Door if it was locked and the player has the Required Key
    /// </summary>
    public void Unlock() {
        if (inventory.keys.Contains(requiredKey))
        {
            isLocked = false;
            Debug.Log("Door Unlocked!");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool GetPlayerSide()
    {
        return Vector3.Dot(gameObject.transform.position - PlayerScript.Instance.transform.position, gameObject.transform.forward) > 0;
    }

    private IEnumerator Rotate(float byDegree, float inTime)
    {
        isRunning = true;
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + new Vector3(0, byDegree, 0));
        if (!isClosed)
        {
            lastOpenRotation = byDegree;
        }
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        isRunning = false;
    }

}
