﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpell : MonoBehaviour {

    public SpellType spellType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GiveBuff(CardObject cardObject)
    {
        cardObject.ChangeMoveDistance(1);
    }

}
