using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy, MeleeAttacker
{
    public void DisableAttackCollider()
    {
        weaponCollider.enabled = false;
    }

    public void EnableAttackCollider()
    {
        weaponCollider.enabled = true;
    }
}
