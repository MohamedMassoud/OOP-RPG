using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Image healthFillImage;
    private GameObject headNub;

    [Header("Health Bar Properties")]

    [SerializeField] private Gradient healthColorGradient;
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1, 0);


    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        healthFillImage = GetComponentInChildren<Image>();
    }

    public void SetHeadNub(GameObject headNub)
    {
        this.headNub = headNub;
    }


    private void LateUpdate()
    {
        if (headNub != null)
        transform.root.position = headNub.transform.position + healthBarOffset;     
    }
    public void SetHealth(int health)
    {
        
            healthSlider.value = health;
            healthFillImage.color = healthColorGradient.Evaluate(healthSlider.normalizedValue);
            
        
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
}
