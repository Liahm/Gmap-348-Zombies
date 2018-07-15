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
	public GameObject endPos;
	private NavMeshAgent Agent;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		Agent = GetComponent<NavMeshAgent>();
		Agent.destination = endPos.transform.position;
		Agent.speed = Speed;
	}
		
	void Update()
    {
		Agent.SetDestination(endPos.transform.position);
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}