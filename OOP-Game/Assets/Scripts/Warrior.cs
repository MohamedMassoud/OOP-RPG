using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Warrior : MeleePlayer
{

    public override string GetPlayerClass()
    {
        return "Warrior";
    }


    protected override void LoadBaseStats()
    {
        IncreaseMaxHealth();

    }

    private void IncreaseMaxHealth()
    {
        maxHealth *= 2;
        health = maxHealth;
        healthBarScript.SetMaxHealth(maxHealth);
        healthBarScript.SetHealth(health);
    }
}
