using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    public Enemy enemyScript;

    public ChaseState(Enemy enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    public void FixedTick()
    {
    }

    public void OnEnter()
    {
        Debug.Log(enemyScript.enemy.name + "entering Chase State.");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
    private void Move()
    {
        enemyScript.transform.LookAt(enemyScript.player.position);
        enemyScript.transform.Translate(enemyScript.enemy.movementSpeed * Time.deltaTime * Vector3.forward);
    }
}
