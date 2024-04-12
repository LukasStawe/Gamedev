using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : IState
{
    public Enemy enemyScript;

    public AttackingState(Enemy enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    public void OnEnter()
    {
        Debug.Log(enemyScript.enemy.name + "entering Attack State.");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        //if (enemyScript.isPlayerInRange && !enemyScript.isPlayerInAttackRange)
        //{
        //    Move();
        //}

        if (Time.time >= enemyScript.nextAttackTime)
        {
            if (enemyScript.isPlayerInAttackRange)
            {
                Attack();
                enemyScript.nextAttackTime = Time.time + 1f / enemyScript.enemy.attackSpeed;
            }
        }
    }
    private void Move()
    {
        enemyScript.transform.LookAt(enemyScript.player.position);
        enemyScript.transform.Translate(enemyScript.enemy.movementSpeed * Time.deltaTime * Vector3.forward);
    }

    private void Attack()
    {
        //Detect Player in Range of Attack and damage them
        Ray ray = new Ray(enemyScript.transform.position, enemyScript.transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.02f, out hit, enemyScript.enemy.attackRange, LayerMask.GetMask("Player")))
        {
            if (hit.collider.TryGetComponent(out PlayerScript player))
            {
                player.GetComponent<PlayerScript>().idleState.TakeDamage(enemyScript.enemy.attackDamage);
            }
        }
    }
    public void FixedTick()
    {
    }
}
