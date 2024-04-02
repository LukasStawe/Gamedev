using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static Action<Readables> OnRead;

    public static Action<ScriptableItem> OnLoot;

    public static Action<int> OnJournalEntryEvent;
}
