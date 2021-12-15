using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekard : Warrior
{
    [Header("Skill Properties")]
    [SerializeField] private float lightShieldSkillDuration = 10f;

    [Header("MISC")]
    [SerializeField] private GameObject lightShield;
    [SerializeField] private AudioClip ekardAttackedSound;

    private bool lighShieldOn = false;
    protected override void InitAnimationNames()
    {
        runAnimationName = "Ekard_Run_01_h";
        attack1AnimationName = "Ekard_Attack_01_h";
        attack2AnimationName = "Ekard_Attack_02_h";
        skillAnimationName = "Ekard_Skill_01_h";
    }

    public void TurnOnLighShield()
    {
        lighShieldOn = true;
        lightShield.SetActive(true);
        CancelInvoke("TurnOffLightShield");
        Invoke("TurnOffLightShield", lightShieldSkillDuration);
    }

    private void TurnOffLightShield()
    {
        lighShieldOn = false;
        lightShield.SetActive(false);
    }

    protected override void DecHealth(int damage)
    {
        if(!lighShieldOn) base.DecHealth(damage);

    }
    protected override void PlayAttackedSound()
    {
        audioSource.PlayOneShot(ekardAttackedSound);
    }

}
