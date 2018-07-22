using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class StethDistance : MonoBehaviour {

    public Transform stethEnd;
    public float maxDistance;
    public float actualDistance;

	public VRTK_InteractableObject stethEndVRTK;

	public VRTK_InteractGrab grabber;

	public GameObject grabbedObject;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        actualDistance = Vector3.Distance(gameObject.transform.position, stethEnd.position);

        
        if (actualDistance > maxDistance)
        {
	        //  Debug.Log(actualDistance);
         //   stethEnd.position = stethEnd.position + (gameObject.transform.position - stethEnd.position)/2;

	        if (grabber.GetGrabbedObject() == stethEnd.gameObject)
	        {
		        grabber.ForceRelease(false);
	        }
	        
	      
        }

    }




   // function restrinctPos(posStart, Vector2, newPos:Vector2)
   // {

   //     var posRestrinct:Vector2;

      //  distance = Vector2.Distance(posStart, newPos);
     //   if (distance > maxDistance) distance = maxDistance;

     //   posRestrinct = posStart + (newPos - posStart).normalized * distance;

        //return posRestrinct;
   // }






}
