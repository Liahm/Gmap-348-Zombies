using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IndividualZombie : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "IndividualZombie";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float WaitTimer;

	private bool individualMove;
	private Collider coll;
	private float pos1,pos2, timer;
//---------------------------------------------------------------------MONO METHODS:
	void Start()
	{
		individualMove = false;
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Human")
		{
			coll = col;
			//Deactive RandomMovement script
			col.GetComponent<RandomMovement>().enabled = false;

			///<PlayAnimationArea>

			///</PlayAnimationArea>

			individualMove = true;
			timer = Time.timeSinceLevelLoad + WaitTimer;
			pos1 = FlockZombie.Instance.RandomOffset();
			pos2 = FlockZombie.Instance.RandomOffset();
		}
		else if (col.tag == "Scientist")
		{
			//defeat screen after x seconds
			//I can actually use an IENumerator here eh?
			FlockZombie.Instance.Defeat();
		}
	}

	void Update()
	{
		if(individualMove)
		{
			//Activate IndividualZombie script after x seconds
			if(Time.timeSinceLevelLoad >= timer)
			{
				if(VERBOSE) Debug.Log("Transformation START!");

				coll.gameObject.tag = "Zombie";
				coll.transform.parent = transform.parent; 
				coll.GetComponent<IndividualZombie>().enabled = true;
				//Activate anything else needed.
				//Teleport to flock.
				//Yea, having issues with just lerp move to them. I don't have time for that
				coll.transform.position = Vector3.MoveTowards(
				transform.parent.transform.position, 
				transform.position + 
					new Vector3(pos1, 
								pos2, 
								0), 
				FlockZombie.Instance.Speed);

				individualMove = false;

			}
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}