using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A stat for player and ememies with a Stat-Type (enum) and a list of StatModifiers that need to be updated on EquipmentChange for Players.
 */
public class Stats : MonoBehaviour
{
	public enum StatEnum
	{
		Attack,
		Defense,
		Health,
		MovementSpeed
	}

	public StatEnum stat;
	public List<StatModifier> modifiers = new List<StatModifier>();
	public float totalValue = 0;

	public Stats(StatEnum stat, List<StatModifier> modifiers, float totalValue)
	{
		this.stat = stat;
		this.modifiers = modifiers;
		this.totalValue = totalValue;
	}

	public float getValue(){

		foreach (StatModifier modifier in modifiers)
		{
			totalValue = totalValue + modifier.value;
		}

		return totalValue;
	}

	public void addModifier(StatModifier modifier)
	{
		modifiers.Add(modifier);
	}
}
