using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Attackable
{
    [Header("Enemy Properties")]
    [SerializeField] private int health = 100;
    [SerializeField] private float movementSpeed;
    public int currentBasicAttackDamage = 10;

    [Header("MISC")]
    [SerializeField] protected BoxCollider weaponCollider;
    [SerializeField] private float attackRange = 1f;

    private Player playerScript;
    private bool followPlayer = false;
    private Animator enemyAnimator;
    private bool attackMode = false;
    
    private Coroutine attackStanceCoroutine;
    private Vector3 playerPos;
    private Vector3 distanceBetweenPlayerAndMob;
    
    

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        playerScript = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
    }



    private void FixedUpdate()
    {
        UpdatePlayerEnemyDistanceInfo();
        UpdateStance();
        FollowPlayer();
    }

    private void OnTriggerEnter(Collider collision)
    {

        if(collision is SphereCollider)            //Mobs Trigger Collider
        {
            Debug.Log("Ima Follow The Player :*");
            TriggeredByPlayer(true);
        }
        else if(collision is BoxCollider)          //Sword Trigger Collider
        {
            if (collision.gameObject.CompareTag("Melee_Weapon_Player"))
            {
                Debug.Log("Hit By Sword");
                TakeDamage(playerScript.currentBasicAttackDamage);
                playerScript.PlayHitSound();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other is SphereCollider)            //Mobs Trigger Collider
        {
            Debug.Log("Ima Leave The Player :*");
            TriggeredByPlayer(false);
        }
    }


    public void TakeDamage(int damage)
    {
        weaponCollider.enabled = false;
        health -= damage;
        enemyAnimator.SetTrigger("hit");
        if(health <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        enemyAnimator.SetBool("dead", true);
        followPlayer = false;
        transform.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void TriggeredByPlayer(bool isTriggeredByPlayer)
    {
        followPlayer = isTriggeredByPlayer;
    }

    protected void UpdatePlayerEnemyDistanceInfo()
    {
        playerPos = playerScript.gameObject.transform.position;
        distanceBetweenPlayerAndMob = playerPos - transform.position;
    }

    protected void FollowPlayer()
    {
        if(!IsAttacking() && followPlayer)
        {

            if (!attackMode)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.fixedDeltaTime);
                enemyAnimator.SetBool("walking", true);
            }
            
            transform.LookAt(playerPos, transform.up);
        }
    }

    protected void UpdateStance()
    {
        if (IsPlayerInRange())
        {
            if (!attackMode)
            {
                attackMode = true;
                attackStanceCoroutine = StartCoroutine(AttackStance());
            }
        }
        else
        {
            attackMode = false;
            enemyAnimator.SetInteger("battle", 0);
            if (attackStanceCoroutine != null) StopCoroutine(attackStanceCoroutine);
            if (!followPlayer)
            {
                enemyAnimator.SetBool("walking", false);
                return;
            }
        }
    }

    protected bool IsPlayerInRange()
    {
        return distanceBetweenPlayerAndMob.magnitude < attackRange;
    }

    protected bool IsAttacking()
    {
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 3");
    }

    protected void Attack()
    {
        enemyAnimator.SetInteger("battle", 1);
        enemyAnimator.SetTrigger("attack");

    }

    protected IEnumerator AttackStance()
    {
        while (attackMode)
        {
            enemyAnimator.SetBool("walking", false);
            yield return 0;
            Attack();
            yield return 0;
            enemyAnimator.SetBool("walking", false);
            yield return new WaitForSeconds(5);
        }
    }
}
