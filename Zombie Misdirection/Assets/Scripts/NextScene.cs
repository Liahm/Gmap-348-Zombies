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
	public int ScientistCount = 1;

	private int currentScientistCount;
//---------------------------------------------------------------------MONO METHODS:


//--------------------------------------------------------------------------METHODS:

	///<TODO>
	/// Literally load next scene on call.
	/// Be it on trigger or by UI or by button (debugging)
	///</TODO>
	void Start()
	{
		currentScientistCount = 0;
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Scientist")
		{
			currentScientistCount++;
		}
		else if (col.tag == "Human")
		{
			Destroy(col.gameObject, Delay);
		}

		if(currentScientistCount == ScientistCount)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

//--------------------------------------------------------------------------HELPERS:
	
}