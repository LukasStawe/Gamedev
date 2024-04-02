using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gate : MonoBehaviour
{
    [SerializeField]
    protected Vector3 openPosition;
    [SerializeField]
    protected Vector3 closePosition;
    [SerializeField]
    protected bool isOpen;

    public virtual void Move()
    {
        isOpen = !isOpen;
    }
}
