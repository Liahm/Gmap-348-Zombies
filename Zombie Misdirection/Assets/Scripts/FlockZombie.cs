using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI; 

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
	public float Hunger;
	public float Satiation; 
	public GameObject HungerBar;
	public Slider bar;
	public GameObject HumanGroup;
	public int ReleasePercentage;
	
	[System.NonSerialized]
	public bool Moving, Regroup, SingleChase;
	[System.NonSerialized]
	public Vector3 WordPos;
	[System.NonSerialized]
	public GameObject HungerTarget;

	private GameObject FlockCenter, p;
	private Transform endPos;
	private int childrenNumber, releasedNumber;
	private Ray ray;
	private LayerMask ground;
	private Image Fill;

	
//---------------------------------------------------------------------MONO METHODS:

	void Start () 
	{
		Fill = HungerBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
		FlockCenter = this.gameObject;
		bar.value = Hunger;
		bar.maxValue = Hunger;
		if(VERBOSE) p = GameObject.FindGameObjectWithTag("Hazard");
		ground = 1 << 9;
		WordPos = transform.Find("Zombie").transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		HungerMeter();
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
				WordPos += new Vector3(0,0, transform.position.z);
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
			Debug.Log("omg");
			Moving = false;
			Drop();
			Moving = true;
		}

		if(Moving)
		{
			flockMovement();
		}
	}


//--------------------------------------------------------------------------METHODS:
	
	public void ChangeSliderColor()
	{
		if(Hunger <= 0)
		{
			Fill.color = Color.red;
		}
		else
			Fill.color = Color.yellow;
	}
	public void Defeat()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void HungerMeter()
	{
		Hunger -= 1 * Time.deltaTime;
		bar.value = Hunger;
		if(Hunger < 0)
		{
			Hunger = 0;
			if(!SingleChase)
			{
				Drop();
				float[] DistanceBetween = new float[HumanGroup.transform.childCount];
				int i = 0;
				float min = Vector3.Distance(FlockCenter.transform.position, HumanGroup.transform.position);
				HumanMovement[] movement = FindObjectsOfType<HumanMovement>();
				Debug.Log(movement[0].name);
				foreach(HumanMovement human in movement)
				{
					if(human.transform.tag == "Zombie")
					{
						continue;
					}	
						
					DistanceBetween[i] = Vector3.Distance(FlockCenter.transform.position, human.transform.position);
					if(DistanceBetween[i] < min && human.transform.tag != "Zombie")
					{
						min = DistanceBetween[i];
						HungerTarget = human.gameObject;
					}
					else
					{
						HungerTarget = GameObject.FindGameObjectWithTag("Scientist");
					}
				}
				SingleChase = true;
			}
		}
	}
	public float RandomOffset()
	{
		return Random.Range(MinOffset, MaxOffset);
	}

	public void ReturnToCenter(Transform target, Vector3 offsetVal, Transform parent)
	{
		target.position = Vector3.MoveTowards(target.position, 
		WordPos + offsetVal, Speed * Time.deltaTime);

		foreach(IndividualZombie zombie in GameObject.FindObjectsOfType<IndividualZombie>())
		{
			zombie.Dropped = false;
		}

		if(Vector2.Distance(FlockCenter.transform.position, WordPos) <= 0.005f)
		{
			Regroup = false;
		}
		
	}

	

//--------------------------------------------------------------------------HELPERS:

	private void Drop()
	{
		if(VERBOSE)		Debug.Log("DROPPING");
		int processed = 0;
		releasedNumber = (transform.childCount * ReleasePercentage) / 100;
		if(releasedNumber < 1 && transform.childCount > 1)	
			releasedNumber = 1;
		foreach(Transform child in transform)
		{
			if(processed++ == releasedNumber)	break;
			child.gameObject.GetComponent<IndividualZombie>().Dropped = true;
			child.parent = null;
		}
	}
	private void flockMovement()
	{
		//transform.position = Vector3.MoveTowards(transform.position, WordPos, Speed * Time.deltaTime);
		if(Vector2.Distance(FlockCenter.transform.position, WordPos) <= 0.005f)
		{
			Moving = false;
		}
	}

}