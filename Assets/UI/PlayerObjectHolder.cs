using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolder : MonoBehaviour {

    public CreatorButton[] PlayerObjects;
    public GameObject ReadyObject;
    public GameObject ReadyImage;
    public CreatorButton CardUsing;

    // Use this for initialization
    void Start()
    {
        PlayerObjects = GetComponentsInChildren<CreatorButton>();

    }
	
	// TODO disable ability to use other cards after one has been used then reset after turn
    public void DestroyCardUsed()
    {
        Destroy(CardUsing.gameObject);
        CardUsing = null;
        ReadyObject = null;
        ReadyImage = null;
    }

    public void DeactivateotherButton(int type)
    {
        foreach (CreatorButton CB in PlayerObjects){
            if (CB.type != type)
            {
                CB.Deactivate();
            }
        }
    }

    public void ActiveObject(GameObject Obj, GameObject ObjImage, CreatorButton card)
    {
        //TerrainControll.ObjectReady(Obj);
        CardUsing = card;
        ReadyObject = Obj;
        ReadyImage = ObjImage;
    }

    public void ClearObject()
    {
        //TerrainControll.ObjectReady(null);
        ReadyObject = null;
    }


}
