using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButtonInteraction : ButtonParent
{
    private bool isRunning;

    private bool isOpen = false;

    [SerializeField]
    private float rotationDegree;

    public override void Interact()
    {
        if (isRunning) return;

        connectedObject.Move();
        StartCoroutine(Rotate( isOpen ? -rotationDegree : rotationDegree, 0.5f));
        isOpen = !isOpen;

    }
    private IEnumerator Rotate(float byDegree, float inTime)
    {
        isRunning = true;
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + new Vector3(byDegree, 0, 0));

        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        isRunning = false;
    }

}
