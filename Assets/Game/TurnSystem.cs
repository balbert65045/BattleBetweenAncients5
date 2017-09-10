using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour {

    // Use this for initialization
    public Text buttonText;
    public Button button;
    float AITimeTurn = 3f;
    float TimeWhenPressed;
    bool pressed = false;

    public int TurnCount = 0;

    PlayerController playerController;
    CardHand cardHand;
    CardCreator cardCreator;
    AIControl aiControl;
    PowerCounter powerCounter;

    private bool PlayerTurn = true;
    private bool AITurn = false;

    public void EndTurn(int player)
    {
        if (player == 1)
        {
            PlayerTurn = false;
            playerController.DisableTools();

            AITurn = true;
            aiControl.Active();

            cardCreator.enabled = false;

            button.interactable = false;
            buttonText.text = "Waiting for Enemy turn";
        }
        else if (player == 2)
        {
            PlayerTurn = true;
            playerController.ResetTools();
            cardHand.Redraw();
            cardCreator.enabled = true;
            TurnCount += 1;

            powerCounter.AddPower(1);
            button.interactable = true;
            buttonText.text = "End Turn";

            AITurn = false;
        }

    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        aiControl = FindObjectOfType<AIControl>();
        cardHand = FindObjectOfType<CardHand>();
        cardCreator = FindObjectOfType<CardCreator>();
        powerCounter = FindObjectOfType<PowerCounter>();
    }


}
