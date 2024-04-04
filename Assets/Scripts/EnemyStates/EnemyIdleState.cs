using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IState
{
    public Enemy enemyScript;

    private Vector3 lastPosition;

    private Vector3 soundLocation;

    public LayerMask playerLayer;

    private bool isPlayerInRange;

    private bool isPlayerInView;

    private bool isPlayerInSight;

    private Transform target;

    private bool wasPlayerHeard;

    public EnemyIdleState(Enemy enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    public void OnEnter()
    {
        Debug.Log(enemyScript.enemy.name + " entering Idle State.");
        isPlayerInRange = false;
        isPlayerInView = false;
        isPlayerInSight = false;
        wasPlayerHeard = false;
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if (!isPlayerInRange)
        {
            DetectPlayerInRange();
        }
        if (isPlayerInRange)
        {
            DetectPlayerInView();
        }
        if (isPlayerInView)
        {
            DetectPlayerInSight();
        }
        if (isPlayerInSight)
        {
            enemyScript.lastPlayerPosition = target.position;
            enemyScript.isPlayerFound = true;
        }
    }
    public void FixedTick()
    {
    }

    public void DetectPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyScript.detectionRadius, playerLayer);

        if (hitColliders.Length != 0 ) 
        {
            target = hitColliders[0].gameObject.transform;
            isPlayerInRange = true;
        } else
        {
            isPlayerInRange = false;
        }
    }

    public void DetectPlayerInView()
    {
        Vector3 playerDirection = target.position - transform.position;

        float angle = Vector3.Angle(playerDirection, transform.forward);

        if (angle <= enemyScript.detectionAngle)
        {
            isPlayerInView = true;
        }
        else
        {
            isPlayerInView = false;
        }
    }

    public void DetectPlayerInSight()
    {
        if (Physics.Raycast(transform.position, target.position, enemyScript.detectionRadius, playerLayer))
        {
            isPlayerInSight = true;
        } else
        {
            isPlayerInSight = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, enemyScript.detectionRadius);

        Vector3 direction = Quaternion.AngleAxis(enemyScript.detectionAngle, transform.up) * transform.forward;
        Vector3 lineEnd = transform.position + direction * enemyScript.detectionRadius;
        Gizmos.DrawRay(transform.position, lineEnd);

        direction = Quaternion.AngleAxis(-enemyScript.detectionAngle, transform.up) * transform.forward;
        lineEnd = transform.position + direction * enemyScript.detectionRadius;
        Gizmos.DrawRay(transform.position, lineEnd);
    }

    public void WasAttacked()
    {
        enemyScript.lastPlayerPosition = PlayerScript.Instance.transform.position;
        enemyScript.isPlayerFound = true;
    }

    public void HearPlayer(Vector3 location)
    {
        wasPlayerHeard = true;
        lastPosition = transform.position;
        soundLocation = location;
        transform.LookAt(soundLocation);
        transform.position = Vector3.MoveTowards(transform.position, soundLocation, enemyScript.enemy.movementSpeed * Time.deltaTime);
    }
}
