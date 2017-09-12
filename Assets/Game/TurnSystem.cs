using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour {


    public GameType gameType;
    public int MaxTurns = 15; 

    public Text buttonText;
    public Button button;

    public int TurnCount = 0;


    private bool PlayerTurn = true;
    private bool AITurn = false;

    PlayerController playerController;
    CardHand cardHand;
    CardCreator cardCreator;
    AIControl aiControl;
    PowerCounter powerCounter;
    WinScreen winScreen;

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

            powerCounter.AddPower(1);
            button.interactable = true;
            buttonText.text = "End Turn";

            AITurn = false;

            TurnCount += 1;
            if (TurnCount >= MaxTurns)
            {
                winScreen.gameObject.SetActive(true);
            }
        }

    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        aiControl = FindObjectOfType<AIControl>();
        cardHand = FindObjectOfType<CardHand>();
        cardCreator = FindObjectOfType<CardCreator>();
        powerCounter = FindObjectOfType<PowerCounter>();
        winScreen = FindObjectOfType<WinScreen>();
    }


}
