using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleePlayer : Player, MeleeAttacker
{
    [Header("Melee Player MISC")]
    [SerializeField] private BoxCollider meleeWeaponCollider;

    public void EnableAttackCollider()      //Called by the animation
    {
        meleeWeaponCollider.enabled = true;
        PlayHitAirSound();
    }

    public void DisableAttackCollider()      //Called by the animation
    {
        meleeWeaponCollider.enabled = false;
    }

}
