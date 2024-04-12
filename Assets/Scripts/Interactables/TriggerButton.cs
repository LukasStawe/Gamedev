using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : ButtonParent
{
    private bool isRunning = false;

    private Vector3 movingPosition;

    private Vector3 normalPosition;

    [SerializeField]
    private Vector3 movingDirection;

    // Start is called before the first frame update
    void Start()
    {
        normalPosition = transform.position;
        movingPosition = normalPosition - movingDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || isRunning) return;

        StartCoroutine(MoveButton());
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

    public override void Interact()
    {
        if (isRunning) return;

        StartCoroutine(MoveButton());
    }
}
