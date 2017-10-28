using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLUT : MonoBehaviour {

    public GameObject[] SummonCards;
    public GameObject[] SpellCards;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}
