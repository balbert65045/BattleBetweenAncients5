using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpell : MonoBehaviour {

    public SpellType spellType;
    public CardSpellName cardSpellName;

    public enum SpellAtribute
    {
        Movement = 1,
        //Range = 2,
        Damage = 2,
        Heal = 3,
    }

    public SpellAtribute spellAtribute;

    
    public int Modifier = 0;
    
    public int ModifierDuration = 0;
 
    public int HealAmount = 0;


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
