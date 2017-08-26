using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Tile2 : MonoBehaviour {

 //   private PlayerObjectHolder PlayerObjectHolder;
 //   public GameObject ParentforObjects;
 //   private Camera cam;
 //   private EscortObj EscortObj;
 //   private CameraRaycaster cameraRaycaster;

    
 //   public bool PlayerOnTile = false;
 //   public bool ObjectValid = true;

 //   public GameObject Image;

 //   public CardType cardType;
	//// Use this for initialization
	//void Awake () {
 //       PlayerObjectHolder = FindObjectOfType<PlayerObjectHolder>();
 //       cam = FindObjectOfType<CameraControl>().GetComponent<Camera>();
 //       EscortObj = FindObjectOfType<EscortObj>();
 //       cameraRaycaster = FindObjectOfType<CameraRaycaster>();
 //       cardType = CardType.Open;

 //       //    cameraRaycaster.layerChangeObservers += CheckObjectSpawn;

 //   }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

 //   public void OnItemMake(GameObject newItem)
 //   {
 //       if (newItem.GetComponent<Item>() == null)
 //       {
 //           Debug.LogError("Item Passed does not have item script attached");
 //       }
 //       Vector3 location = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 7);
 //       //   location = new Vector3(location.x, 0, location.y);
 //       Image = Instantiate(newItem, location, Quaternion.identity, ParentforObjects.transform);
 //       itemHeld = newItem.GetComponent<Item>().itemType;

 //   }


 //   void CheckObjectSpawn(Transform newTransform)
 //   {
 //       if (newTransform == transform)
 //       {
 //          // print("New Transform Found" + newTransform);
 //       }
 //   }
   

 //   //public void SpawnObject()
 //   //{

 //   //    if (PlayerObjectHolder.ReadyObject != null && EscortObj.transform.position.z < (transform.position.z + 5))
 //   //    {
 //   //        if (ObjHeld == null && PlayerOnTile == false && ObjectValid)
 //   //        {
 //   //            Vector3 location = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 7);
 //   //            //   location = new Vector3(location.x, 0, location.y);
 //   //            Instantiate(PlayerObjectHolder.ReadyObject, location, Quaternion.identity, ParentforObjects.transform);
 //   //            ObjHeld = PlayerObjectHolder.ReadyObject;
 //   //        }
 //   //    }
 //   //}

 //   //public void SpawnImage()
 //   //{
 //   //    if (PlayerObjectHolder.ReadyImage != null && EscortObj.transform.position.z < (transform.position.z + 5))
 //   //    {
 //   //        if (ObjHeld == null && PlayerOnTile == false && ObjectValid)
 //   //        {
 //   //            Vector3 location = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 7);
 //   //            Image = Instantiate(PlayerObjectHolder.ReadyImage, location, Quaternion.identity, ParentforObjects.transform);
 //   //        }
 //   //    }
 //   //}

 //   public void DestroyImage()
 //   {
 //       itemHeld = ItemTypes.Open;
 //       Destroy(Image);
 //   }

 //   private void OnCollisionEnter(Collision collision)
 //   {
 //       if (collision.gameObject.GetComponent<EscortObj>())
 //       {
         
 //           PlayerOnTile = true;
 //       }
 //   }

 //   private void OnCollisionExit(Collision collision)
 //   {
 //       if (collision.gameObject.GetComponent<EscortObj>())
 //       {

 //           PlayerOnTile = false;
 //       }
 //   }

 //   public void TogglePlacement(bool value)
 //   {
 //       ObjectValid = value;
 //   }

}
