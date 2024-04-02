using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MovingGate : Gate
{
    /// <summary>
    /// Update is called once per frame. Moves the Gate/Entrance-blocking-Object to the Open/Close position after the corresponding Lever/Button/Plate has been (de-)activated.
    /// </summary>
    void Update()
    {
        if (isOpen && transform.position != openPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, 0.01f);
        }
        if (!isOpen && transform.position != closePosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, closePosition, 0.01f);
        }
    }
}
