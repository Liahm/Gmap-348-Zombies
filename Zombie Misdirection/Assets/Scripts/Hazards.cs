using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Hazards";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float Delay;
//---------------------------------------------------------------------MONO METHODS:

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Scientist")
		{
			FlockZombie.Instance.Defeat();
		}
		else if (col.tag == "Human")
		{
			//Put a "dead animation/stuff"
			Destroy(col.gameObject, Delay);
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}