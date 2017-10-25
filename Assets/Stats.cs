using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    // Use this for initialization
    public Text MoveText;
    public Text RangeText;
    public Text DamageMinText;
    public Text DamageMaxText;
    public Text HealthText;
    public Text PowerText;


    public void SetStats(CardObject card, int power)
    {
        MoveText.text = card.initialMaxMoveDistance.ToString();
        RangeText.text = card.initialMaxAttackDistance.ToString();
        DamageMinText.text = card.initAttackDamageMin.ToString();
        DamageMaxText.text = card.initAttackDamageMax.ToString();
        HealthText.text = card.maxHealthPoints.ToString();
        PowerText.text = power.ToString();
    }
}
