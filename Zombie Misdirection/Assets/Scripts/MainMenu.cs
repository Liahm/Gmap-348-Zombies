using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "MainMenu";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

	public GameObject CanvasObject;
	public AudioClip ClipMove, ClipSelect;

	private AudioSource soundManager;

	private bool x;

//-------------------------------------------------------------------MONO METHODS:
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(!x)	
			{
				CanvasObject.SetActive(true);
				x = true;
			}
			else
			{
				CanvasObject.SetActive(false);
				x = false;
			}
		}
		if(Input.GetKeyDown(KeyCode.N))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else if(Input.GetKeyDown(KeyCode.M))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

//--------------------------------------------------------------------------METHODS:
	//Main menu UI buttons
	public void StartGame()
	{
		if(SceneManager.GetActiveScene().name == "GameOver" 
			|| SceneManager.GetActiveScene().name == "Victory")
		{	
			SceneManager.LoadScene("MainMenu");	
		}
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);		
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void MouseEnter() 
	{
		Debug.Log("!");
		soundManager = gameObject.GetComponent<AudioSource>();
		soundManager.clip = ClipMove;
        soundManager.Play();
    }
 
    public void MouseClick() 
	{
		Debug.Log("@");
		soundManager.clip = ClipSelect;
        soundManager.Play();
    }  
//--------------------------------------------------------------------------HELPERS:
	
}