using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuablesPickup : Items
{
    public ScriptableValuable valuable;
    public override string interactTextToDisplay => "Press E to pick up " + valuable.value + " Gold.";


    public override void Interact()
    {
        base.Interact();

        Actions.OnMoneyChangedEvent?.Invoke(valuable.value);

        Destroy(gameObject);
    }
}
