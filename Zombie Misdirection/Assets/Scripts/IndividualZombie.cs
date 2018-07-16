using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IndividualZombie : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "IndividualZombie";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float WaitTimer;
	public GameObject child, Parent;

	[System.NonSerialized]
	public bool Alert, Dropped;

	private bool individualMove, chasing;
	[System.NonSerialized]
	public Collider coll;
	private float pos1, pos2, timer, individualSpeed, frequency;
	private Transform parent;
	private BoxCollider box;
	private CapsuleCollider sphere;
	private NavMeshObstacle obstacle;
//---------------------------------------------------------------------MONO METHODS:
	void Start()
	{
		individualMove = false;
		parent = Parent.transform;
		individualSpeed = FlockZombie.Instance.Speed;
		frequency = FlockZombie.Instance.MovementFrequency;
		pos1 = FlockZombie.Instance.RandomOffset();
		pos2 = FlockZombie.Instance.RandomOffset();
		Dropped = false;
		box = GetComponent<BoxCollider>();
		sphere = GetComponent<CapsuleCollider>();
		obstacle = child.GetComponent<NavMeshObstacle>();
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Human" && transform.tag == "Zombie" || 
		col.tag == "Scientist" && transform.tag == "Zombie")
		{
			coll = col;
			box.enabled = true;
			sphere.enabled = false;
			obstacle.enabled = false;
			Alert = true;
		}

		if(chasing)
		{
			if(col.gameObject.tag == "Human" && transform.tag == "Zombie")
			{
				
				//Deactive RandomMovement script
				col.gameObject.GetComponent<RandomMovement>().enabled = false;
				col.gameObject.GetComponent<NavMeshAgent>().enabled = false;
				col.gameObject.GetComponent<HumanMovement>().SpawnBubble();
				col.gameObject.GetComponent<HumanMovement>().enabled = false;
				col.transform.parent = null;

				///<PlayAnimationArea>

				///</PlayAnimationArea>
				FlockZombie.Instance.Moving = false;
				individualMove = true;
				Alert = false;
				timer = Time.timeSinceLevelLoad + WaitTimer;
			}
			else if (col.gameObject.tag == "Scientist" && transform.tag == "Zombie" )
			{
				FlockZombie.Instance.Defeat();
			}
		}
	}
	void Update()
	{
		frequency -= Time.deltaTime;

		if(FlockZombie.Instance.Hunger > 0)
		{
			if(FlockZombie.Instance.Regroup && !Dropped)
			{
				if(Input.GetKeyUp(KeyCode.Q))
				{
					pos1 = FlockZombie.Instance.RandomOffset();
					pos2 = FlockZombie.Instance.RandomOffset();
					transform.parent = parent;
				}
				FlockZombie.Instance.ReturnToCenter(transform, 
					new Vector3(pos1, pos2, 0), parent);
			}
		}
		else if(FlockZombie.Instance.SingleChase)
		{
			if(transform.parent != parent)
			{
				transform.position = Vector3.MoveTowards(transform.position, 
					FlockZombie.Instance.HungerTarget.transform.position, individualSpeed * Time.deltaTime);
			}
		}

		if(Alert)
		{
			if(VERBOSE)	Debug.Log("Alert has been hit");
			chasing = true;
			transform.position = Vector3.MoveTowards(transform.position, coll.transform.position, individualSpeed * Time.deltaTime);						
		}
		if(individualMove)
		{
			//Activate IndividualZombie script after x seconds
			if(Time.timeSinceLevelLoad >= timer)
			{
				if(VERBOSE) Debug.Log("Transformation START!");
				
				coll.gameObject.tag = "Zombie";
				coll.transform.parent = parent; 
				//You might be thinking, why do you have a getcomponent in update?
				//Well, this section is literally called once per time it happens
				//Now you might think, why put it here then?
				//Well, I need the timer to continue ticking
				coll.gameObject.GetComponent<IndividualZombie>().enabled = true;
				coll.gameObject.GetComponent<IndividualZombie>().child.SetActive(true);
				coll.gameObject.GetComponent<CapsuleCollider>().enabled = true;
				//Activate anything else needed.
				//Teleport to flock.
				//Yea, having issues with just lerp move to them. I don't have time for that
				hungerFix();

				coll.transform.position = Vector3.MoveTowards(
				transform.parent.transform.position, 
				transform.position + 
					new Vector3(pos1, 
								pos2, 
								0), 
				FlockZombie.Instance.Speed);
				obstacle.enabled = true;
				//FlockZombie.Instance.Moving = true;
				FlockZombie.Instance.SingleChase = false;
				box.enabled = false;
				coll.GetComponent<BoxCollider>().enabled = false;
				sphere.enabled = true;
				coll.GetComponent<CapsuleCollider>().enabled = true;
				chasing = false;
				individualMove = false;
			}
		}
		
		if(frequency <= 0 && FlockZombie.Instance.Moving)
		{
			SwayAround(0.1f);
			frequency = Random.Range (FlockZombie.Instance.MovementFrequency, WaitTimer);
		}
	}

//--------------------------------------------------------------------------METHODS:
	public void SwayAround(float SpeedDifference)
	{
		Vector3 offset = new Vector3((FlockZombie.Instance.RandomOffset()*20)/100,
		(FlockZombie.Instance.RandomOffset()*20)/100, 
		parent.transform.position.z);

		if(Vector3.Distance(transform.position, 
			parent.transform.position) >= FlockZombie.Instance.MaxDistance)
		{
			if(Vector3.Distance(transform.position, FlockZombie.Instance.WordPos) <= 
			Vector3.Distance(transform.position, FlockZombie.Instance.WordPos) + 
			Vector3.Distance(transform.position, parent.transform.position))
			{
				individualSpeed -= SpeedDifference;
			}
			else if(Vector3.Distance(transform.position, FlockZombie.Instance.WordPos) >= 
			Vector3.Distance(transform.position, FlockZombie.Instance.WordPos) + 
			Vector3.Distance(transform.position, parent.transform.position))
			{
				individualSpeed += SpeedDifference;
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, offset, individualSpeed * Time.deltaTime);			
		}
	}
//--------------------------------------------------------------------------HELPERS:
	
	private void hungerFix()
	{
		transform.parent = parent;
		FlockZombie.Instance.Hunger += FlockZombie.Instance.Satiation;
		FlockZombie.Instance.ReturnToCenter(transform, 
					new Vector3(pos1, pos2, 0), parent);
	}
}