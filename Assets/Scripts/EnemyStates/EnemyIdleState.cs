using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : IState
{
    public Enemy enemyScript;

    private Vector3 investigatingLocation;

    private LayerMask playerLayer;

    private bool isPlayerInRange;

    private bool isPlayerInView;

    private bool _isPlayerInSight;

    private bool isPlayerInSight
    {
        get { return _isPlayerInSight; }
        set
        {
            if (_isPlayerInSight == true && value == false)
            {
                if (seenTimer >= 2f)
                {
                    Investigate(investigatingLocation);
                }
            }
            _isPlayerInSight = value;
        }
    }

    private Transform target;

    private bool wasPlayerHeard;

    private float seenTimer = 0f;

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
        playerLayer = LayerMask.GetMask("Player");
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
        }

        if (isPlayerInSight)
        {
            enemyScript.transform.rotation = Quaternion.Slerp(enemyScript.transform.rotation, Quaternion.LookRotation(target.position), 2f * Time.deltaTime);
            seenTimer += Time.deltaTime;
            Debug.Log(seenTimer);
        }

        if (!isPlayerInSight && seenTimer >= 0f)
        {
            seenTimer -= Time.deltaTime;
            Debug.Log(seenTimer);
        }

        if (seenTimer >= 4f )
        {
            enemyScript.lastPlayerPosition = investigatingLocation;
            enemyScript.isPlayerFound = true;
        }
    }
    public void FixedTick()
    {
    }

    public void DetectPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemyScript.eyes.position, enemyScript.detectionRadius/2, playerLayer);

        if (hitColliders != null && hitColliders.Length > 0)
        {
            target = hitColliders[0].gameObject.transform;
            Debug.Log("Player in range");
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    public void DetectPlayerInView()
    {
        Vector3 playerDirection = target.position - enemyScript.eyes.position;

        float angle = Vector3.Angle(enemyScript.eyes.forward, playerDirection);

        if (angle <= enemyScript.detectionAngle)
        {
            isPlayerInView = true;
            Debug.Log("Player in view");
        }
        else
        {
            isPlayerInView = false;
        }
    }

    public void DetectPlayerInSight()
    {
        Vector3 playerDirection = target.position - enemyScript.eyes.position;
        Debug.DrawRay(enemyScript.eyes.position, playerDirection, Color.red);
        if (Physics.Raycast(enemyScript.eyes.position, playerDirection, enemyScript.detectionRadius, playerLayer))
        {
            isPlayerInSight = true;
            if (investigatingLocation == Vector3.zero)
            {
                Debug.Log("Player in Sight");
                investigatingLocation = target.position;
                //enemyScript.transform.LookAt(investigatingLocation);


            }
        } else
        {
            isPlayerInSight = false;
            investigatingLocation = Vector3.zero;
        }
    }

    public void WasAttacked()
    {
        enemyScript.lastPlayerPosition = PlayerScript.Instance.transform.position;
        enemyScript.isPlayerFound = true;
    }

    public void HearPlayer(Vector3 location)
    {
        wasPlayerHeard = true;
        investigatingLocation = location;
        investigatingLocation.y = enemyScript.transform.position.y;
        Investigate(investigatingLocation);
        
    }

    private void Investigate(Vector3 location)
    {
        //enemyScript.transform.LookAt(investigatingLocation);
        enemyScript.agent.SetDestination(investigatingLocation);
    }
}
