using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallButton : ButtonParent
{
    [SerializeField]
    private Vector3 movingDirection;

    private Vector3 movingPosition;

    private Vector3 normalPosition;

    private bool isRunning = false;
    
    public override void Interact()
    {
        if (isRunning) return;

        connectedObject.Move();
        StartCoroutine(MoveButton());
    }

    // Start is called before the first frame update
    void Start()
    {
        normalPosition = transform.position;
        movingPosition = normalPosition - movingDirection;
    }

    private IEnumerator MoveButton()
    {
        isRunning = true;

        while (transform.position != movingPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, movingPosition, 0.001f);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (transform.position != normalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, normalPosition, 0.001f);
            yield return null;
        }

        isRunning = false;
    }
}
