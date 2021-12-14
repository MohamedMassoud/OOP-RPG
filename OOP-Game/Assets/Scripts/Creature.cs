using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, Attackable
{


    [Header("Player Properties")]
    [SerializeField] protected float speed;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    [Header("MISC")]
    [SerializeField] private GameObject headNub;
    [SerializeField] private GameObject healthBarPrefab;

    protected HealthBar healthBarScript;
    public bool isAlive = true;



    public void CreateHealthBar()
    {
        GameObject healthBar = Instantiate(healthBarPrefab);
        healthBarScript = healthBar.GetComponentInChildren<HealthBar>();
        healthBarScript.SetHeadNub(headNub);
        healthBarScript.SetMaxHealth(maxHealth);
    }

    
    public void PlayRandomFootSteps()
    {

    }

    public void TakeDamage(int damage)
    {
        DecHealth(damage);
        PlayOnHitAnimation();
        if (health <= 0)
        {
            isAlive = false;
            Die();
        }
    }

    protected virtual void DecHealth(int damage)
    {
        health -= damage;
        healthBarScript.SetHealth(health);
    }
    public abstract void Die();
    
    protected abstract void PlayOnHitAnimation();

    public abstract void PlayHitSound();
    protected abstract void PlayHitAirSound();

}
