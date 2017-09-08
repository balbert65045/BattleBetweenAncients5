using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerHealthBar : MonoBehaviour {

    RawImage healthBarRawImage = null;
    public Spawner spawner = null;

    float MaxHealth;
    float currentHealth;
    // Use this for initialization
    void Start()
    {
        spawner = GetComponentInParent<Spawner>(); // Different to way player's health bar finds player
        healthBarRawImage = GetComponent<RawImage>();
        MaxHealth = spawner.getCurrentHealth;

        currentHealth = spawner.getCurrentHealth;
        float HealthasPercentage = (currentHealth) / MaxHealth;
        float xValue = (HealthasPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }

    public void DamageDealt(int damage)
    {
        currentHealth = spawner.getCurrentHealth;

        float HealthasPercentage = (currentHealth - (float)damage) / MaxHealth;

        float xValue = Mathf.Clamp(((HealthasPercentage / 2f) - 0.5f), -.5f, 0);
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }

}

	

