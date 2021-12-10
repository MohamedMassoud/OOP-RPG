using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avelyn : Warrior
{

    [SerializeField] private ParticleSystem lightSkill;
    protected override void InitAnimationNames()
    {
        runAnimationName = "Avelyn_Run_01_h";
        attack1AnimationName = "Avelyn_Attack_01_h";
        attack2AnimationName = "Avelyn_Attack_02_h";
        skillAnimationName = "Avelyn_Skill_01_h";
    }

    public void PlayLightSkill()
    {
        lightSkill.Play();
    }

}
