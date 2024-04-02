using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState
{
    public Enemy enemyScript;

    public EnemyIdleState(Enemy enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    public void OnEnter()
    {
        Debug.Log(enemyScript.enemy.name + " entering Idle State.");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
    public void FixedTick()
    {
    }
}
