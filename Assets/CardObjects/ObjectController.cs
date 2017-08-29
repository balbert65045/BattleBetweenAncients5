using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ObjectController : MonoBehaviour {

    public CardObject ObjectSelected;

    //TO DO make object move when right click on another tile 
    public void SelectedObjectinPlay(CardObject Obj)
    {
        ObjectSelected = Obj;
    }

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
    }

}
