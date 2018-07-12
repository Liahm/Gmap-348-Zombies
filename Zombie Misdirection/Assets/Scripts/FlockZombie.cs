using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlockZombie : MyUtility.Singleton<FlockZombie>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "NextScene";
	public bool VERBOSE = false;
	///<TODO>
	/// Get user input on mouse click
	/// On click, set destination of "Zombie" gameobject to end position.
	/// All zombies flock to that position + offset.
	/// 	Offset is decided depending on the original location of the zombie
	///			Look for the center of the group. Calculate distance between that fake center to current zombie
	///			That's the offset with another slight offset.
	///	If button is pressed. Drop x% zombie from parent.
	///		Save offset values in an array
	/// If button 2 is pressed, move it to the current center + offset, then parent.
	/// If ontrigger with a non scientiest, then turn off the randommovement script and enable this on that character.
	///		Turn on the radius too
	/// Move that character to the center with a random offset, then parent.
	///</TODO>

	// Use this for initialization
//---------------------------------------------------------------------------FIELDS:

	public float Speed;
	public int ReleasePercentage;

	private GameObject FlockCenter, p;
	private Transform endPos;
	private bool moving;
	private Vector3 wordPos;
	private int childrenNumber;
	
//---------------------------------------------------------------------MONO METHODS:

	void Start () 
	{
		FlockCenter = this.gameObject;

		if(VERBOSE) p = GameObject.FindGameObjectWithTag("Hazard");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 mousePos=new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			
			Ray ray=Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000f)) 
			{
				wordPos=hit.point;
			} else 
			{
				wordPos=Camera.main.ScreenToWorldPoint(mousePos);
			}
			if(VERBOSE) Instantiate(p, wordPos, Quaternion.identity);
		}
		if(Input.GetKeyUp(KeyCode.Mouse0))
		{
			if(Vector2.Distance(FlockCenter.transform.position, wordPos) <= 0.005f)
			{
				moving = false;
			}
			else
				moving = true;
		}
		if(moving)
		{
			flockMovement();
		}
	}


//--------------------------------------------------------------------------METHODS:
	public float RandomOffset()
	{
		return Random.Range(-8, 9);
	}

	public void Defeat()
	{
		SceneManager.LoadScene("Defeat");
	}

//--------------------------------------------------------------------------HELPERS:

	private void flockMovement()
	{
		transform.position = Vector3.MoveTowards(transform.position, wordPos, Speed * Time.deltaTime);
		if(Vector2.Distance(FlockCenter.transform.position, wordPos) <= 0.005f)
		{
			moving = false;
		}
	}

	private void moveToParent()
	{

	}

	private void releaseFromParent()
	{
		childrenNumber = transform.childCount;
		int Remaining = (ReleasePercentage * childrenNumber) / 100;
		foreach(Transform child in transform)
		{
			child.transform.parent = null;
			if(Remaining <= 0) break;
			else Remaining--;
		}
	}


}