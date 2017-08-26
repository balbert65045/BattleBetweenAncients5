using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Cursor : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster CameraRaycaster;
    public bool Activated = false;
    void Start()
    {
        CameraRaycaster = FindObjectOfType<CameraRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(CameraRaycaster.layerHit);

        if (CrossPlatformInputManager.GetButtonUp("pointer1"))
        {
            Activated = false;
        }
        else if (Activated && CameraRaycaster.layerHit == Layer.LevelTerrain)
        {
            
         //   if (CameraRaycaster.ObjectTransform.gameObject.GetComponent<PlayerObjectCreator>() )
         //   {
         //       CameraRaycaster.ObjectTransform.gameObject.GetComponent<PlayerObjectCreator>().SpawnImage() ;
         //   }
        }
    }

    public void Active()
    {
        Activated = true;
    }

}
