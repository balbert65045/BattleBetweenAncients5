using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellStats : MonoBehaviour {

    // Use this for initialization
    public Image DamageImage;
    public Image HealthImage;
    public Image MovementImage;

    public Text ModifierText;
    public Text DurationText;
    public Text PowerText;


    public void SetStats(CardSpell card, int power)
    {
        CardSpell cardSpell = GetComponentInParent<CardSpell>();
        DamageImage.gameObject.SetActive(false);
        HealthImage.gameObject.SetActive(false);
        MovementImage.gameObject.SetActive(false);

        Debug.Log(cardSpell.cardSpellName);

        switch (cardSpell.cardSpellName)
        {
            case (CardSpellName.DamageBuff):

                DamageImage.gameObject.SetActive(true);
                if (card.Modifier > 0)
                {
                    ModifierText.text = "+ " + card.Modifier.ToString();
                    DurationText.text = card.ModifierDuration.ToString();
                    PowerText.text = power.ToString();
                }
                else
                {
                    ModifierText.text = "- " + card.Modifier.ToString();
                    DurationText.text = card.ModifierDuration.ToString();
                    PowerText.text = power.ToString();
                }
                break;
            case (CardSpellName.Heal):
                HealthImage.gameObject.SetActive(true);
                ModifierText.text = "+ " + card.HealAmount.ToString();
                DurationText.text = "0";
                PowerText.text = power.ToString();
                break;
            case (CardSpellName.MoveBuff):
                MovementImage.gameObject.SetActive(true);
                if (card.Modifier > 0)
                {
                    ModifierText.text = "+ " + card.Modifier.ToString();
                    DurationText.text = card.ModifierDuration.ToString();
                    PowerText.text = power.ToString();
                }
                else
                {
                    ModifierText.text = "- " + card.Modifier.ToString();
                    DurationText.text = card.ModifierDuration.ToString();
                    PowerText.text = power.ToString();
                }
                break;
        }

        


    }
}
