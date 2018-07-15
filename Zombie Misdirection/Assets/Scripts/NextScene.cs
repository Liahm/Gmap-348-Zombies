using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "NextScene";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float Delay;
//---------------------------------------------------------------------MONO METHODS:


//--------------------------------------------------------------------------METHODS:

	///<TODO>
	/// Literally load next scene on call.
	/// Be it on trigger or by UI or by button (debugging)
	///</TODO>

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Scientist")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else if (col.tag == "Human")
		{
			Destroy(col.gameObject, Delay);
		}
	}
//--------------------------------------------------------------------------HELPERS:
	
}