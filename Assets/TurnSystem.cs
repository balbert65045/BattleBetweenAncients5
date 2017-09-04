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

    PlayerController playerController;
    AIControl aiControl;

    private bool PlayerTurn = true;
    private bool AITurn = false;

    public void EndTurn()
    {
        Debug.Log("Ended Turn");
        TimeWhenPressed = Time.time;
        PlayerTurn = false;
        playerController.DisableTools();
        AITurn = true;
        aiControl.Active();
    }

    void ResetTurn()
    {
        Debug.Log("Reset");
        button.interactable = true;
        buttonText.text = "End Turn";
        PlayerTurn = true;
        playerController.ResetTools();
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        aiControl = FindObjectOfType<AIControl>();
    }

    private void Update()
    {
        if (Time.time < TimeWhenPressed + AITimeTurn  && AITurn)
        {
            button.interactable = false;
            buttonText.text = "Waiting for Enemy turn";
        }

        else if (Time.time >= TimeWhenPressed + AITimeTurn && AITurn)
        {
            
            ResetTurn();
            AITurn = false;

        }
    }

}
