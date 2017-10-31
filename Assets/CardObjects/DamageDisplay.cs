using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float TextDuration = 1f;

    float TimeShown;

    private int damageTaken;
    private int healTaken;
    private Text text;

	void Start () {
        text = GetComponent<Text>();
        text.text = "";
    }

    public void DamageDealt(int damage)
    {
        damageTaken = damage;
    }

    public void Heal(int amount)
    {
        healTaken = amount;
    }


        // Update is called once per frame
        void Update () {
		if (damageTaken != 0)
        {
            TimeShown = Time.time;
            text.text = "-" + damageTaken.ToString();
            text.color = Color.red;
            damageTaken = 0;
        }
        else if (healTaken != 0)
        {
            TimeShown = Time.time;
            text.text = "+" + healTaken.ToString();
            text.color = Color.green;
            healTaken = 0;
        }

        if (Time.time > TimeShown + TextDuration)
        {
            text.text = "";
        }
	}
}
