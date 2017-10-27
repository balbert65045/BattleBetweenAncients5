using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLUT : MonoBehaviour {

    public GameObject[] Cards;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}
