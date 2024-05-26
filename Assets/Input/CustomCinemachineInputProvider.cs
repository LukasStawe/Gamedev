using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCinemachineInputProvider : Cinemachine.CinemachineInputProvider
{
    public override float GetAxisValue(int axis)
    {
        if (PlayerScript.Instance.AnyUIActive()) return 0;
        return base.GetAxisValue(axis);
    }
}
