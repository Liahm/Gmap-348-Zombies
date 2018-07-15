using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class FlockZombie : MyUtility.Singleton<FlockZombie>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "NextScene";
	public bool VERBOSE = false;
	///<TODO>
	///	If button is pressed. Drop x% zombie from parent.
	/// If button 2 is pressed, move it to the current center + offset, then parent.
	/// If ontrigger with a non scientiest, then turn off the randommovement script and enable this on that character.
	///		Turn on the radius too
	/// Move that character to the center with a random offset, then parent.
	///</TODO>

	// Use this for initialization
//---------------------------------------------------------------------------FIELDS:

	public float Speed, MinOffset, MaxOffset, MaxDistance, MovementFrequency;
	[Space(25)]
	public int Hunger; 
	public int ReleasePercentage;
	[System.NonSerialized]
	public bool Moving, Regroup;
	[System.NonSerialized]
	public Vector3 WordPos;

	private GameObject FlockCenter, p;
	private Transform endPos;
	private int childrenNumber, releasedNumber;
	private Ray ray;
	private LayerMask ground;

	
//---------------------------------------------------------------------MONO METHODS:

	void Start () 
	{
		FlockCenter = this.gameObject;
		if(VERBOSE) p = GameObject.FindGameObjectWithTag("Hazard");
		ground = 1 << 9;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 mousePos=new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			
			ray=Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;

			if(Physics.Raycast(ray,out hit,1000f, ground)) 
			{
				WordPos=hit.point;
			} else 
			{
				WordPos=Camera.main.ScreenToWorldPoint(mousePos);
			}
			if(VERBOSE) Instantiate(p, WordPos, Quaternion.identity);
		}
		if(Input.GetKeyUp(KeyCode.Mouse0))
		{
			if(Vector2.Distance(FlockCenter.transform.position, WordPos) <= 0.005f)
			{
				Moving = false;
			}
			else
				Moving = true;
		}
		if(Input.GetKeyDown(KeyCode.Q) && !Regroup)
		{
			Regroup = true;
		}
		if(Input.GetKeyDown(KeyCode.W) && transform.childCount > 1)
		{
			Drop();
		}

		if(Moving)
		{
			flockMovement();
		}
	}


//--------------------------------------------------------------------------METHODS:
	

	public void Defeat()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public float RandomOffset()
	{
		return Random.Range(MinOffset, MaxOffset);
	}

	public void ReturnToCenter(Transform target, Vector3 offsetVal, Transform parent)
	{
		target.position = Vector3.MoveTowards(target.position, 
		FlockCenter.transform.position + offsetVal, Speed * Time.deltaTime);

		foreach(IndividualZombie zombie in GameObject.FindObjectsOfType<IndividualZombie>())
		{
			zombie.Dropped = false;
		}

		if(Vector2.Distance(FlockCenter.transform.position, WordPos) <= 0.005f)
		{
			Regroup = false;
		}
		
	}
	///<Summary>
	///Randomly walks around a small amount (40% of the offsets)
	///If they leave the radius of their parent, then walk back
	///		If they are closer to the target, slow them, otherwise increase speed
	///</Summary>
	public void SwayAround(NavMeshAgent Agent, Transform EndPoint,float SpeedDifference)
	{
		Vector3 offset = new Vector3((RandomOffset()*40)/100,(RandomOffset()*40)/100,0);

		if(Vector3.Distance(transform.position, 
			transform.parent.transform.position) >= MaxDistance)
		{
			if(Vector3.Distance(transform.position, EndPoint.position) <= 
			Vector3.Distance(transform.position, EndPoint.position) + 
			Vector3.Distance(transform.position, transform.parent.transform.position))
			{
				Agent.speed = Speed - SpeedDifference;
			}
			else if(Vector3.Distance(transform.position, EndPoint.position) >= 
			Vector3.Distance(transform.position, EndPoint.position) + 
			Vector3.Distance(transform.position, transform.parent.transform.position))
			{
				Agent.speed = Speed + SpeedDifference;
			}
		}
		Agent.Move(offset);
	}

	

//--------------------------------------------------------------------------HELPERS:

	private void Drop()
	{
		int processed = 0;
		releasedNumber = (transform.childCount * ReleasePercentage) / 100;
		foreach(Transform child in transform)
		{
			if(++processed == releasedNumber)	break;
			child.gameObject.GetComponent<IndividualZombie>().Dropped = true;
			child.parent = null;
		}
	}
	private void flockMovement()
	{
		transform.position = Vector3.MoveTowards(transform.position, WordPos, Speed * Time.deltaTime);
		if(Vector2.Distance(FlockCenter.transform.position, WordPos) <= 0.005f)
		{
			Moving = false;
		}
	}
}