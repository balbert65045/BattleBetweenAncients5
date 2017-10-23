using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInspector : MonoBehaviour {

    // Use this for initialization
    Dropdown dropdown;
    public GameObject CardFieldObjects;
    public GameObject CardFieldSpells;

	void Start () {
        dropdown = GetComponent<Dropdown>();
        CardFieldObjects.SetActive(false);
        CardFieldSpells.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
       if (dropdown.value == 0)
        {
            CardFieldObjects.SetActive(true);
            CardFieldSpells.SetActive(false);
        }
       else if (dropdown.value == 1)
        {
            CardFieldObjects.SetActive(false);
            CardFieldSpells.SetActive(true);
        }

    }
}
