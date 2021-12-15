using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rogue : MeleePlayer
{
    public override string GetPlayerClass()
    {
        return "Rogue";
    }

    protected override void LoadBaseStats()
    {
        speed *= 2;
    }
}
