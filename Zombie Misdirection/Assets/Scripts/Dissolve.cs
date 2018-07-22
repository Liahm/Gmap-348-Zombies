using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dissolve : MonoBehaviour {
	public RawImage Image;
	public float Duration;
	private float alpha;
	private bool transition, once;
	// Use this for initialization
	void Start () {
		transition = false;
		once = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey && once)
		{
			transition = true;
		}
		if (transition)
		{
			lerpAlpha();
			
		}
		
	}

	private void lerpAlpha()
	{
		once = false;
		float lerp = Mathf.PingPong(Time.time, Duration) / Duration;
		var tempColor = Image.color;
		alpha = Mathf.Lerp(0, 1, lerp);
		tempColor.a = alpha;
		Image.color = tempColor;
		if(Image.color.a <= 0.1f)
		{
			tempColor.a = 0;
			Image.color = tempColor;
			transition = false;
			this.gameObject.SetActive(false);
		}
			
	}
}
