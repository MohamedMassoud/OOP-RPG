using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected int health = 100;
    public float movementSpeed;
    private Warrior warriorScript;
    protected bool followPlayer = false;
    protected Vector3 dir;
    protected Animator enemyAnimator;


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

        if(collision is CapsuleCollider)            //Mobs Trigger Collider
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

    protected void TakeDamage(int damage)
    {
        health -= damage;
        enemyAnimator.SetInteger("moving", 13);
        if(health <=0)
        {
            Die();
        }
    }

    protected void Die()
    {
        enemyAnimator.SetInteger("moving", 9);
        followPlayer = false;
    }
    public void TriggeredByPlayer(bool isTriggeredByPlayer)
    {
        followPlayer = isTriggeredByPlayer;
    }



    protected void FollowPlayer()
    {
        Vector3 playerPos = warriorScript.gameObject.transform.position;
        if (!followPlayer || (playerPos - transform.position).magnitude < 1)
        {
            enemyAnimator.SetInteger("moving", 0);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, playerPos, movementSpeed * Time.fixedDeltaTime);
        enemyAnimator.SetInteger("moving", 1);
        transform.LookAt(playerPos, transform.up);
    }

    protected void Attack()
    {

    }
}
