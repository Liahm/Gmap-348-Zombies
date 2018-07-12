using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Hazards";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
//---------------------------------------------------------------------MONO METHODS:

	void OnTriggerEnter(Collider col)
	{
		///<TODO>
		/// If this object (hazard) touches scientiest. Gameover.!--
		/// Else if it touches zombie/other entities, "destroy" them
		///</TODO>
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}