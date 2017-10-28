using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CardSelector : MonoBehaviour {

    // Use this for initialization
    CardHand cardHand;
    Card[] creatorButtons;
    PowerCounter powerCounter;

	void Start () {
        cardHand = FindObjectOfType<CardHand>();
        powerCounter = FindObjectOfType<PowerCounter>();

    }

    void Update()
    {

        if (CrossPlatformInputManager.GetButtonDown("pointer1"))
        {
            creatorButtons = FindObjectsOfType<Card>();
            foreach (Card CB in creatorButtons)
            {
                if (powerCounter.PowerQuery(CB.GetPowerAmount)) {CB.CheckMousePositionOnButton(); }
            }

        }

        //Cancel Object to spawn
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            cardHand.Clear();
        }
        
    }
    //In late update to allow spawning to happend before clearing whats selected
    private void LateUpdate()
    {
        if (CrossPlatformInputManager.GetButtonUp("pointer1"))
        {
            cardHand.Clear();
        }
    }

    }
