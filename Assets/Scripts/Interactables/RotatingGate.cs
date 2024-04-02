using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGate : Gate
{
    public void Move(float byDegree)
    {
       StartCoroutine(Rotate(byDegree, 3f));
    }

    private IEnumerator Rotate(float byDegree, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + new Vector3(0, byDegree, 0));
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }
}
