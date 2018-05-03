using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {
	private Vector3 defaultCamLocalPos;
	private float defaultCamDistance;

	// Use this for initialization
	void Start () {
		defaultCamLocalPos = transform.localPosition;

		Vector3 charPos = transform.parent.position;
		Vector3 camPos = transform.position;
		Vector3 charToCam = camPos - charPos;
		defaultCamDistance = charToCam.magnitude;
        //Debug.Log("Camp Position :" + camPos);
        
       
    }
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;

		Vector3 charPos = transform.parent.position;
		Vector3 camPos = transform.position;
		Vector3 charToCam = camPos - charPos;

		//int mapLayer = LayerMask.NameToLayer("Map");
		int mapLayerMask = LayerMask.GetMask(new string[] { "Camera Wall" });


		float distance = defaultCamDistance;
        //Debug.Log("Camp Position :" + camPos);
        //Debug.Log("Distance :" + distance);
		
		Debug.DrawRay(charPos, charToCam.normalized * distance, Color.magenta, 0, false);
		
		if (Physics.Raycast(charPos, charToCam.normalized, out hit, distance))
		{
			//Debug.Log("camera hit " + hit.collider);
			Vector3 newCamPos = hit.point + -0.5f * charToCam.normalized;
			//Debug.DrawLine(charPos, newCamPos, Color.magenta, 0, false);
			transform.position = newCamPos;
		}
		else
		{
            
            transform.localPosition = defaultCamLocalPos;
		}
	}
}
