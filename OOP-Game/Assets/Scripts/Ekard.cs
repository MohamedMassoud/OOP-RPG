using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekard : Warrior
{
    [Header("MISC")]
    [SerializeField] private GameObject lightShield;
    protected override void InitAnimationNames()
    {
        runAnimationName = "Ekard_Run_01_h";
        attack1AnimationName = "Ekard_Attack_01_h";
        attack2AnimationName = "Ekard_Attack_02_h";
        skillAnimationName = "Ekard_Skill_01_h";
    }

    public void TurnOnLighShield()
    {
        lightShield.SetActive(true);
        Invoke("TurnOffLightShield", 10f);
    }

    private void TurnOffLightShield()
    {
        lightShield.SetActive(false);
    }

}
