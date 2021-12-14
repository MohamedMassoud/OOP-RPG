using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avelyn : Warrior
{

    [SerializeField] private GameObject lightSpell;
    [SerializeField] private int spellDamage = 50;
    private Vector3 spellOffset = new Vector3(0, 0.2f, 1);


    protected override void InitAnimationNames()
    {
        runAnimationName = "Avelyn_Run_01_h";
        attack1AnimationName = "Avelyn_Attack_01_h";
        attack2AnimationName = "Avelyn_Attack_02_h";
        skillAnimationName = "Avelyn_Skill_01_h";
    }

    public void PlayLightSkill()
    {
        
        LightSpell lightSpellScript = Instantiate(lightSpell, transform.position + spellOffset, transform.rotation).GetComponent<LightSpell>();
        lightSpellScript.spellDamage = spellDamage;
    }

}
