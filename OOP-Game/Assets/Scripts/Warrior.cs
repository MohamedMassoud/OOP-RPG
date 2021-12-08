using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public BoxCollider swordCollider;
    protected override void InitAnimationNames()
    {
        runAnimationName = "Ekard_Run_01_h";
        attack1AnimationName = "Ekard_Attack_01_h";
        attack2AnimationName = "Ekard_Attack_02_h";
        skillAnimationName = "Ekard_Skill_01_h";
    }


    public void EnableAttackCollider()      //Called by the animation
    {
        swordCollider.enabled = true;
    }

    public void DisableAttackCollide()      //Called by the animation
    {
        swordCollider.enabled = false;
    }

    public override string GetPlayerClass()
    {
        return "Warrior";
    }
}
