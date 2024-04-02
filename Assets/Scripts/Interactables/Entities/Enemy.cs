using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region StateMachin-Variables
    public StateMachine stateMachine;

    public AttackingState attackState;
    public EnemyIdleState enemyIdleState;
    public ChaseState chaseState;
    
    public bool isPlayerInRange;
    public bool isPlayerInAttackRange;
    public bool isPlayerInChaseRange;
    public bool isEnemyAttacked;
    #endregion

    public ScriptableEnemy enemy;

    public Transform player;

    int maxHealth;
    int currentHealth;

    public float nextAttackTime = 0f;

    private float distanceToPlayer;

    [SerializeField] GameObject bag;

    private void Awake()
    {
        stateMachine = new StateMachine();

        enemyIdleState = new EnemyIdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackingState(this);

        stateMachine.AddTransition(enemyIdleState, attackState, () => distanceToPlayer <= enemy.detectionRadius == true);

        stateMachine.AddTransition(attackState, enemyIdleState, () => distanceToPlayer > enemy.chaseRange == true);

        stateMachine.AddTransition(enemyIdleState, chaseState, () => isEnemyAttacked == true);

        stateMachine.AddTransition(chaseState, attackState, () => isPlayerInAttackRange == true);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = enemy.maxHealth;
        currentHealth = maxHealth;
        stateMachine.SetState(enemyIdleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();

        // Calculate the distance between the enemy and the player
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer <= enemy.detectionRadius)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
        
        // Check if the player is within the attack radius
        if (distanceToPlayer <= enemy.attackRange)
        {
            isPlayerInAttackRange = true;
        }
        else
        {
            isPlayerInAttackRange = false;
        }
                
    }

    void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    /*
     * Calculates the Damage the Enemy takes based on its armor
     * 
     * @param damage The damage dealt by the Player-Weapon
     */
    public void takeDamage(int damage)
    {
        int actualDamage;

        if (damage - enemy.armor < 0)
        {

            actualDamage = 0;

        } else {

            actualDamage = damage - enemy.armor;

        }

        currentHealth -= actualDamage;

        Debug.Log("Enemy hit for " + actualDamage + " damage!");

        //Play Hurt Animation/Sound


        //Die
        if(currentHealth <= 0) 
        {
            Die();
        }
    }

    /*
     * Checks if the Enemy had Loot and Spawns it and then Destroys the Enemy Object     * 
     */
    void Die()
    {
        Debug.Log("Enemy Died!");

        //Spawn Loot
        if(enemy.Loot.Count != 0)
        {
            SpawnLoot();
        }


         //Disable Enemy
         Destroy(this.gameObject);
         Debug.Log("Enemy destroyed");

        if (enemy.addsJournalEntry)
        {
            Actions.OnJournalEntryEvent?.Invoke(enemy.journalID);
        }
    }

    /*
     * Spawns the Lootbag with the Loot from the attached ScriptableEnemy
     */
    public void SpawnLoot()
    {
        //Spawns the bag and places it where the enemy was
        GameObject clonedLoot = Instantiate(bag) as GameObject;
        clonedLoot.transform.position = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z);
        LootBag loot = clonedLoot.GetComponent<LootBag>();

        //Creates a copy of the Loot to put into the Bag
        foreach(ScriptableItem item in enemy.Loot)
        {
            loot.loot.Add(item);
        }

        //Names the Lootbag accordingly
        clonedLoot.name = gameObject.name + " Loot";
        Debug.Log("Loot spawned at " + clonedLoot.gameObject.transform.position);

    }
}
