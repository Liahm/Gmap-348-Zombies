using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "RandomMovement";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	///<TODO>
	/// Get current pos of user.
	/// Shoot a raycast to a random location, if nothing touches that end, create end pos.
	/// Else, repeat on another random location
	///</TODO>

	public float MovementCooldown;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {

    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}