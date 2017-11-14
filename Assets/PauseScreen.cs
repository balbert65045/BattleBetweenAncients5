using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseScreen : MonoBehaviour {

    public Button EndTurnButton; 

    public void Pause()
    {
        Time.timeScale = 1;
        EndTurnButton.interactable = false;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        EndTurnButton.interactable = true;
        this.gameObject.SetActive(false);
    }
}
