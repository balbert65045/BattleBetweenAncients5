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
    public List<int> AiWavesTimes;
    public int LastWaveTime = 0; 
  

    public bool PlayerTurn = true;
    public bool AITurn = false;

    PlayerController playerController;
    CardHand cardHand;
    CardSelector cardCreator;
    AIControl aiControl;
    PowerCounter powerCounter;
    WinScreen winScreen;

    public delegate void IncrimentTurnListeners();
    public event IncrimentTurnListeners TurnListenerObserver;

    public void EndTurn(int player)
    {
        if (player == 1)
        {
            CardObject[] cardObjects = FindObjectsOfType<CardObject>();

            bool CombatHappening = false;
            foreach (CardObject cardobject in cardObjects)
            {
                if (cardobject.CurrentCommbatState != CombatType.OutOfCombat)
                {
                    CombatHappening = true;
                }
            }
            if (!CombatHappening)
            {
                StartCoroutine(PlayerTurnOver());
            }
        }
        else if (player == 2)
        {
            StartCoroutine(AITurnOver());
        }

    }

    IEnumerator PlayerTurnOver()
    {
        button.interactable = false;
        buttonText.text = "Waiting for Enemy turn";
        yield return new WaitForSeconds(.3f);
        PlayerTurn = false;
        playerController.DisableTools();

        AITurn = true;

        aiControl.Active();

        cardCreator.enabled = false;



        CheckEndGame();


    }

        IEnumerator AITurnOver()
    {
        yield return new WaitForSeconds(.3f);
        PlayerTurn = true;
        playerController.ResetTools();

        cardHand.Redraw();
        cardCreator.enabled = true;



        powerCounter.AddPower(1);
        Debug.Log("Button On");
        button.interactable = true;
        buttonText.text = "End Turn";

        AITurn = false;

        TurnCount += 1;
        if (TurnListenerObserver != null) { TurnListenerObserver(); }

        CheckEndGame();


    }

    void CheckEndGame()
    {
        switch (gameType)
        {
            case (GameType.Survival):
                if (TurnCount > LastWaveTime)
                {
                    if (aiControl.NumberOfEnemiesLeft() <= 0)
                    {
                        winScreen.gameObject.SetActive(true);
                    }
                }
                break;
            case (GameType.Defense):
                //if (TurnCount >= MaxTurns)
                //{
                //    winScreen.gameObject.SetActive(true);
                //}
                break;
            case (GameType.Attack):
                break;
        }
    }


    public void SetWaves(List<int> WaveTimes)
    {
        AiWavesTimes = WaveTimes;
        // Makes the list from smallest to highest 
        AiWavesTimes.Sort(delegate (int a, int b) { return a.CompareTo(b); });
        foreach (int WaveTime in AiWavesTimes)
        {
          
            if (WaveTime > LastWaveTime)
            {
                LastWaveTime = WaveTime;
            }
        }
    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        aiControl = FindObjectOfType<AIControl>();
        cardHand = FindObjectOfType<CardHand>();
        cardCreator = FindObjectOfType<CardSelector>();
        powerCounter = FindObjectOfType<PowerCounter>();
        winScreen = FindObjectOfType<WinScreen>();
    }

}
