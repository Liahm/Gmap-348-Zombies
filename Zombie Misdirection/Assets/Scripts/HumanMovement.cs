using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HumanMovement : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "HumanMovement";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float Speed;
	public GameObject EndPos, AlertBubble;
	private NavMeshAgent Agent;
	private bool runAway, stopRunning;
	private Collider collision;
//---------------------------------------------------------------------MONO METHODS:

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "RunAway")
		{
			collision = col;
			stopRunning = false;
			runAway = true;
		}
	}

	void OnTriggerExit()
	{
		stopRunning = true;
		Agent.speed = Speed;
	}
	void Start() 
	{
		Agent = GetComponent<NavMeshAgent>();
		Agent.destination = EndPos.transform.position;
		Agent.speed = Speed;
		stopRunning = true;
		runAway = false;
	}
		
	void Update()
    {
		if(runAway)
		{
			runAwayFromThisSpot(collision);
			if(stopRunning)
				runAway = false;
		}
		else
			Agent.SetDestination(EndPos.transform.position);
    }

//--------------------------------------------------------------------------METHODS:
	public void SpawnBubble()
	{
		Instantiate(AlertBubble, transform.position, transform.rotation);
	}
//--------------------------------------------------------------------------HELPERS:

	private void runAwayFromThisSpot(Collider coll)
	{
		Debug.Log("RUN");
		Agent.speed = 1.5f;
		Vector3 direction = transform.position - coll.gameObject.transform.position;
		Agent.SetDestination(direction);
	}

}