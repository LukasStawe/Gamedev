using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class for Pressure Plates and all its Methods.
/// </summary>
public class PressurePlate : MonoBehaviour
{

    [SerializeField]
    private Gate connectedGate;

    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Vector3 endPosition;

    [SerializeField]
    private bool isTriggered = false;

    /// <summary>
    /// Sets the corresponding positions where the pressure plate should move when (de-)activated
    /// </summary>
    private void Awake()
    {
        // TODO change it to make it based on the isTriggered bool so that Pressure Plates can start activated isntead of activating them by dropping a crate on them on Scene Start
        startPosition = transform.position;
        endPosition = transform.position;
        endPosition.y = endPosition.y == 1.03f ? 0.98f : 1.03f;
    }

    /// <summary>
    /// Function that is called when an Object with a Collider steps/lands on the pressure plate
    /// </summary>
    /// <param name="other"> The Collider of the object that triggered the pressure plate </param>
    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        isTriggered = true;

        connectedGate.Move();
    }

    /// <summary>
    /// Function that is called when an Object with a Collider exits the pressure plate
    /// </summary>
    /// <param name="other"> The Collider of the object that exits the pressure plate </param>
    private void OnTriggerExit(Collider other)
    {
        if (!isTriggered) return;
        isTriggered = false;

        connectedGate.Move();
    }

    /// <summary>
    /// Update is called once per frame. Moves the Pressure Plate up/down after getting (de-)activated.
    /// </summary>
    private void Update()
    {
        if (isTriggered && transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, 0.01f);
        }
        if (!isTriggered && transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.01f);
        }
    }
}
