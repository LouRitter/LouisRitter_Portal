using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
// This class shows and hides doors (aka portals) when you walk into them. It listens for all OnPortalTransition events
// and manages the active portal.

public class DoorManager : MonoBehaviour {

	public delegate void DoorAction(Transform door);
	public static event DoorAction OnDoorOpen;
	public GameObject doorToOther;
	public GameObject doorToVirtual;
	public GameObject doorToReality;
	public GameObject portalCamera;
	public Camera mainCamera;
	private GameObject currDoor;
	private bool isCurrDoorOpen = false;
	private int currentLocation = 0;
	public GameObject generatePlanes;
	public GameObject hitTest;
	void Start(){
		currDoor = doorToVirtual;
		PortalTransition.OnPortalTransition += OnDoorEntrance;
	}

	void Update(){			
		if ( Input.GetMouseButtonDown (0)) {
			OpenDoorInFront(Input.mousePosition);
			Debug.Log("ISCURRDOOROPEN = "+isCurrDoorOpen);
		}
	}

	// This method is called from the Spawn Portal button in the UI. It spawns a portal in front of you.
	public void OpenDoorInFront(Vector2 screenPos){
		
		Ray r = mainCamera.ScreenPointToRay (screenPos);
        RaycastHit hit;
		switch(currentLocation){
				case 0:
					currDoor = doorToVirtual;
					//AnchorDoor(hit.point);
				break;
				case 1: 
					portalCamera.SetActive (true);
					currDoor = doorToOther;
					
					AnchorDoor(mainCamera.transform.forward);
				break;
				case 2:
					portalCamera.SetActive (false);
					
					currDoor = doorToReality;
					AnchorDoor(mainCamera.transform.forward);
				break;
			}
		}	

	private void AnchorDoor(Vector3 hit){

			Debug.Log(currDoor + " this door being called");
			currDoor.SetActive (true);

			currDoor.transform.position = (Vector3.ProjectOnPlane(hit, Vector3.up)).normalized
				+mainCamera.transform.position;
			Debug.Log(currDoor + "'s position is: "+ currDoor.transform.position);
			currDoor.transform.rotation = Quaternion.LookRotation (
				Vector3.ProjectOnPlane(currDoor.transform.position - mainCamera.transform.position, Vector3.up));
			currDoor.GetComponentInParent<Portal>().Source.transform.localPosition = (Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up)).normalized
				+ mainCamera.transform.position;

			//StartCoroutine(growDoor(currDoor,3f));

			isCurrDoorOpen = true;
			
			if (OnDoorOpen != null) {
				OnDoorOpen (currDoor.transform);
			}
	}

	// Respond to the player walking into the doorway. Since there are only two portals, we don't need to pass which
	// portal was entered.
	private void OnDoorEntrance() {
		Debug.Log("ONDOORENTRANCE CURRDOOR= "+currDoor);
		var portalName = PortalTransition.name;
		currDoor.SetActive (false);
		isCurrDoorOpen = false;
		switch(currentLocation){
			case 0:
				currentLocation = 1;
				generatePlanes.SetActive (false);
				hitTest.SetActive (false);
			break;
			case 1:
				if (portalName == "BackToRealityDoor"){
					currentLocation = 0;
					ResetTracking();
				}else{
					currentLocation = 2;
				}
			break;
			case 2: 
				if (portalName == "BackToVirtualDoor"){
					currentLocation = 1;
				}else{
					currentLocation = 0;
					ResetTracking();
				}
			break;
		}
	}
	public void ResetTracking(){
			ARKitWorldTrackingSessionConfiguration sessionConfig = new ARKitWorldTrackingSessionConfiguration ( UnityARAlignment.UnityARAlignmentGravity, UnityARPlaneDetection.Horizontal);
        	UnityARSessionNativeInterface.GetARSessionNativeInterface().RunWithConfigAndOptions(sessionConfig, UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking);
			generatePlanes.SetActive (true);
			hitTest.SetActive (true);
	}
	IEnumerator growDoor(GameObject item, float duration)
	{

		// float t = 0;
		// var startSize = item.transform.localScale;
		// var endSize = new Vector3(startSize.x + 1, startSize.y + 1, startSize.z +1);
		// while (t< duration) {
		// 	t += Time.deltaTime;
		// 	item.transform.localScale = Vector3.Lerp (startSize, endSize, t / duration);
			yield return null;
		//}
		
	}
}