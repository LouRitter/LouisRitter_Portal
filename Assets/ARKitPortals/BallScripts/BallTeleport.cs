using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
[RequireComponent(typeof(Rigidbody))]
public class BallTeleport : MonoBehaviour {
	public delegate void PortalTransitionAction();
	public static event PortalTransitionAction OnPortalTransition;
	void OnTriggerEnter(Collider portal){	
		Portal logic = portal.GetComponentInParent<Portal> ();
		var distance = Vector3.Distance(Camera.main.transform.localPosition,logic.transform.position);
		
		transform.position = logic.PortalCameras[0].transform.position + (transform.forward * 2);
		Debug.Log(transform.position);
}
}
