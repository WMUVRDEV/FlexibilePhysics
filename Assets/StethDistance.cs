using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using DG.Tweening;


public class StethDistance : MonoBehaviour {

    public Transform stethEnd;
    public Rigidbody stethEndRB;
    public float maxDistance;
    public float actualDistance;

	public VRTK_InteractableObject stethEndVRTK;

	public VRTK_InteractGrab grabber;

	public GameObject grabbedObject;
	
	public float jumpBackAmount;
	
	public Transform cubeA, cubeB;
	
	// Use this for initialization
	void Start () {
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
	}
	
	// Update is called once per frame
	void Update () {
		
		//	cubeA.position = stethEnd.position + (gameObject.transform.position - (stethEnd.position/jumpBackAmount));

        actualDistance = Vector3.Distance(gameObject.transform.position, stethEnd.position);

     

        if (actualDistance > maxDistance)
        {

              stethEndRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
             stethEndRB.constraints = RigidbodyConstraints.None;

            //  Debug.Log(actualDistance);
            //   stethEnd.position = stethEnd.position + (gameObject.transform.position - stethEnd.position)/2;

            if (grabber.GetGrabbedObject() == stethEnd.gameObject)
	        {
	            grabber.ForceRelease(true);
	            
	            Vector3 snapPos = stethEnd.position + (gameObject.transform.position - (stethEnd.position/jumpBackAmount));
	            
	            //  stethEnd.DOMove(cubeB.position, 0.5f, false);
	            
	            // stethEnd.DoMove(stethEnd.position + (gameObject.transform.position - (stethEnd.position)/jumpBackAmount), 0.5, false);
	            //  stethEnd.position = stethEnd.position + (gameObject.transform.position - (stethEnd.position)/jumpBackAmount);
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
