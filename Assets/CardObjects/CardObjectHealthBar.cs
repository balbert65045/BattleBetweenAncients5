using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObjectHealthBar : MonoBehaviour
{
    RawImage healthBarRawImage = null;
    public CardObject cardObject = null;

    float MaxHealth;
    float currentHealth;
    // Use this for initialization
    void Start()
    {
        cardObject = GetComponentInParent<CardObject>(); // Different to way player's health bar finds player
        healthBarRawImage = GetComponent<RawImage>();
        MaxHealth = cardObject.getCurrentHealth;

        currentHealth = cardObject.getCurrentHealth;
        float HealthasPercentage = (currentHealth) / MaxHealth;
        float xValue = (HealthasPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }

    public void DamageDealt(int damage)
    {
        currentHealth = cardObject.getCurrentHealth;

        float HealthasPercentage = (currentHealth - (float)damage) / MaxHealth;

        float xValue = Mathf.Clamp(((HealthasPercentage / 2f) - 0.5f), -.5f, 0);
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }

    public void Heal(int amount)
    {
        currentHealth = cardObject.getCurrentHealth;

        float HealthasPercentage = (currentHealth + (float)amount) / MaxHealth;

        float xValue = Mathf.Clamp(((HealthasPercentage / 2f) - 0.5f), -.5f, 0);
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //float xValue = (cardObject.getCurrentHealthasPercentage / 2f) - 0.5f;
        //healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
