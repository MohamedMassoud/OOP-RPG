using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected int health = 100;
    public float movementSpeed;
    private Warrior warriorScript;
    [SerializeField] protected bool followPlayer = false;
    protected Vector3 dir;
    protected Animator enemyAnimator;
    protected bool attackMode = false;
    protected float attackRange = 1f;
    protected Coroutine attackStanceCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        warriorScript = GameObject.Find("Warrior").GetComponent<Warrior>();
        Debug.Log(warriorScript);
    }



    private void FixedUpdate()
    {
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
            if (collision.gameObject.CompareTag("Sword"))
            {
                Debug.Log("Hit By Sword");
                TakeDamage(warriorScript.currentBasicAttackDamage);
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


    protected void TakeDamage(int damage)
    {
        health -= damage;
        enemyAnimator.SetTrigger("hit");
        if(health <=0)
        {
            Die();
        }
    }

    protected void Die()
    {
        enemyAnimator.SetBool("dead", true);
        followPlayer = false;
        transform.GetComponent<BoxCollider>().enabled = false;
    }
    public void TriggeredByPlayer(bool isTriggeredByPlayer)
    {
        followPlayer = isTriggeredByPlayer;
    }



    protected void FollowPlayer()
    {
        Vector3 playerPos = warriorScript.gameObject.transform.position;
        Vector3 distanceBetweenPlayerAndMob = playerPos - transform.position;
        if (distanceBetweenPlayerAndMob.magnitude < attackRange)
        {
            if (!attackMode)
            {
                Debug.Log("Huh?");
                attackMode = true;
                attackStanceCoroutine = StartCoroutine(AttackStance());
            }
            
        }
        else {
            attackMode = false;
            enemyAnimator.SetInteger("battle", 0);
            if(attackStanceCoroutine!=null)StopCoroutine(attackStanceCoroutine);
            if (!followPlayer)
            {
                enemyAnimator.SetBool("walking", false);
                return;
            }
            else if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack 3"))
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.fixedDeltaTime);
                enemyAnimator.SetBool("walking", true);
                transform.LookAt(playerPos, transform.up);
            }
        } 
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
