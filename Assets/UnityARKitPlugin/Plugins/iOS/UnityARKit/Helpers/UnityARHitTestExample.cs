using System;
using System.Collections.Generic;

using System.Collections;
using UnityEngine;
namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
   
		public Transform DoorToVirtual;
        public Transform DoorToOther;
        public GameObject PortalCam;
        public GameObject ballPrefab;
        public Transform ballSpawn;
        public GameObject Button;
        public bool isCreated = false;
 
        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            Debug.Log (Input.touchCount);
            if (point.y > 0.175){
                if (Input.touchCount == 1){
                    DoorToOther.gameObject.SetActive (false);
                    PortalCam.SetActive(false);
                    if (hitResults.Count > 0) {
                        foreach (var hitResult in hitResults) {
                            DoorToVirtual.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                            DoorToVirtual.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                            StartCoroutine(ScaleUp(DoorToVirtual.gameObject, 2f));
                            DoorToVirtual.gameObject.SetActive (true);
                            Debug.Log(point.y);
                        }
                    }
                }else if (Input.touchCount > 1){
                    DoorToVirtual.gameObject.SetActive (false);
                    PortalCam.SetActive(true);
                    if (hitResults.Count > 0) {
                        foreach (var hitResult in hitResults) {
                            DoorToOther.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                            DoorToOther.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform); 
                            StartCoroutine(ScaleUp(DoorToOther.gameObject, 2f));
                            DoorToOther.gameObject.SetActive (true);
                            Debug.Log(point.y);
                        }
                    }
                }
            }
            return false;
        }

		// Update is called once per frame
		void Update () {

			if (Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
                            return;
                        }
                    }
				}
			}
		}
         IEnumerator ScaleUp(GameObject item, float duration){
		    float t = 0;
		    var startScale= new Vector3(0f,0f,0f);
		    var endScale = new Vector3(1.25f, 1f, 1f);
		    while (t < duration) {
			t += Time.deltaTime;
			item.transform.localScale = Vector3.Lerp (startScale, endScale, t / duration);
			yield return null;
		    }
	    }
    
	
	}
}

