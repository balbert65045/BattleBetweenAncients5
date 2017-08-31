using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObjectHealthBar : MonoBehaviour
{
    RawImage healthBarRawImage = null;
    public CardObject cardObject = null;
    public float X;

    // Use this for initialization
    void Start()
    {
        cardObject = GetComponentInParent<CardObject>(); // Different to way player's health bar finds player
        healthBarRawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = -(cardObject.healthAsPercentage / 2f) - 0.5f;
        X = xValue;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
