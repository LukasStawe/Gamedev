using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages the Stats (mostly) for the player. Needs to make sure the Stats get updated when Equipment gets changed.
 */
public class StatManager : MonoBehaviour
{

    public List<Stats> stats;

    private void Start()
    {
        Stats Attack = new Stats(Stats.StatEnum.Attack, new List<StatModifier>(), 0f);
        Stats Defense = new Stats(Stats.StatEnum.Defense, new List<StatModifier>(), 0f);
        Stats Health = new Stats(Stats.StatEnum.Health, new List<StatModifier>(), PlayerScript.Instance.maxHealth);
        Stats MovementSpeed = new Stats(Stats.StatEnum.MovementSpeed, new List<StatModifier>(), 0f);

        stats = new List<Stats> { Attack, Defense, Health, MovementSpeed };
    }

}
