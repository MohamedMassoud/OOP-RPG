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

    protected override void Skill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Skill");
        }
    }

    public void EnableAttackCollider()
    {

    }

    public void DisableAttackCollide()
    {

    }
}
