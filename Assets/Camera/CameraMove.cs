using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMove : MonoBehaviour {

    private CardObject cardObjectSelected;

    [SerializeField]
    float TransitionTime = 20f;
    [SerializeField]
    float zoomSensitivity = 10f;
    [SerializeField]
    float ZOffset = 10f;

    
    // NOTE possibly add a snap to object ability in future
   

    // Update is called once per frame
    void LateUpdate () {

        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            float z = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

            float VerticalPosition = Mathf.Clamp(transform.position.y - z * zoomSensitivity, 10, 30);
            Vector3 newPosition = new Vector3(transform.position.x + h, VerticalPosition, transform.position.z + v);
            transform.position = Vector3.Lerp(transform.position, newPosition, TransitionTime * Time.deltaTime);

        }
    }
}
