﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
// This component lives on the camera parent object and triggers a transition when you walk through a portal 
[RequireComponent(typeof(Rigidbody))]
public class PortalTransition : MonoBehaviour {

	public delegate void PortalTransitionAction();
	public static event PortalTransitionAction OnPortalTransition;
	public static string name;
	public GameObject portalCamera;
	public GameObject VRDoor;
	public GameObject OtherDoor;
	public GameObject[] staticDoors;
	public GameObject hitTest;
	// The main camera is surrounded by a SphereCollider with IsTrigger set to On
	void OnTriggerEnter(Collider portal){
		
		Portal logic = portal.GetComponentInParent<Portal> ();
		name = logic.transform.name;
		transform.position = logic.PortalCameras[0].transform.position - GetComponentInChildren<Camera>().transform.localPosition;
		
		var BackToRealityDoor = staticDoors[0];
		var posOne = BackToRealityDoor.transform.position = transform.position;
		posOne[2] = posOne[2] - 1;
		
		Debug.Log(name + "    "+BackToRealityDoor.transform.position);
		switch(name){

			case "VRDoor":
				posOne[1] = 200;
				BackToRealityDoor.SetActive(true);
				hitTest.SetActive(false);
			break;	
			case "OtherDoor":
				posOne[1] = 400;
				BackToRealityDoor.SetActive(true);
				hitTest.SetActive(false);
				
			break;
			case "RealWorldDoor":
				BackToRealityDoor.SetActive(false);
				VRDoor.SetActive(false);
				OtherDoor.SetActive(false);
				hitTest.SetActive(true);
				resetTracking();

			break;
		}
	
	Debug.Log(GetComponentInChildren<Camera>());
		if (OnPortalTransition != null) {
			// Emit a static OnPortalTransition event every time the camera enters a portal. The DoorManager listens for this event.
			OnPortalTransition ();
			
		}
	}
	public void resetTracking(){
	ARKitWorldTrackingSessionConfiguration sessionConfig = new ARKitWorldTrackingSessionConfiguration ( UnityARAlignment.UnityARAlignmentGravity, UnityARPlaneDetection.Horizontal);
        UnityARSessionNativeInterface.GetARSessionNativeInterface().RunWithConfigAndOptions(sessionConfig, UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking);
	}
}
