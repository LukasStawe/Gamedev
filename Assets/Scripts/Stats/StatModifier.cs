using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  A Stat Modifier for stats which consists of a Type and a Value. This is what Items get.
 */
public class StatModifier : MonoBehaviour
{
    public enum StatEnum
    {
        Attack,
        Defense,
        Health,
        MovementSpeed
    }

    public StatEnum stat;
	public float value;
}
