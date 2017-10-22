using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpell : MonoBehaviour {

    public SpellType spellType;

    public enum SpellAtribute
    {
        Movement = 1,
        //Range = 2,
        Damage = 2,
        Heal = 3,
    }

    public SpellAtribute spellAtribute;

    [SerializeField]
    int Modifier = 0;
    [SerializeField]
    int ModifierDuration = 0;

    [SerializeField]
    int HealAmount = 0;


    public void GiveBuff(CardObject cardObject)
    {
        switch (spellAtribute)
        {
            case SpellAtribute.Movement:
                cardObject.ChangeMoveDistance(Modifier, ModifierDuration);
                break;

            // Should only be allowed for range characters 
            //case SpellAtribute.Range:
            //    cardObject.ChangeAttackDistance(Modifier, ModifierDuration);
            //    break;


            case SpellAtribute.Damage:
                cardObject.ChangeAttackDamage(Modifier, ModifierDuration);
                break;
            case SpellAtribute.Heal:
                cardObject.HealObject(HealAmount);
                break;

        }
    }

}
