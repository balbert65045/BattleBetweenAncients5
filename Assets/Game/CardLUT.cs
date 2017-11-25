using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLUT : MonoBehaviour {

    public GameObject[] SummonCards;
    public bool[] SummonCardsActive;

    public GameObject[] SpellCards;
    public bool[] SpellCardsActive;

    private void Awake()
    {
        CardLUT[] luts = FindObjectsOfType<CardLUT>();

        
        if (luts.Length > 1)
        {
            Destroy(luts[1].gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }


}
