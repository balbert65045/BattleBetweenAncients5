using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour {

    // Use this for initialization
    CardObject cardObject;
    [SerializeField]
    float TextDuration = 1f;

    float TimeShown;

    private int damageTaken;
    private Text text;

	void Start () {
        cardObject = GetComponentInParent<CardObject>();
        text = GetComponent<Text>();
        text.text = "";
    }

    public void DamageDealt(int damage)
    {
        damageTaken = damage;
    }


    // Update is called once per frame
    void Update () {
		if (damageTaken != 0)
        {
            TimeShown = Time.time;
            text.text = "-" + damageTaken.ToString();
            damageTaken = 0;
        }

        if (Time.time > TimeShown + TextDuration)
        {
            text.text = "";
        }
	}
}
