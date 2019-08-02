using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour 
{

	public Vector3 Endposition;
	public Quaternion Endrotation;
	public Vector3 Startposition;

	public Camera Viewport;
	public GameObject Leftscreen;
	public GameObject Rightscreen;
	public float distancescreensmove;
	public float movementofviewport;
	public float rotationspeed;

	public AudioClip Dooropening;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(Openingscene());
		Viewport.transform.localPosition = Startposition;
		Viewport.transform.rotation = new Quaternion(0,0,0,0);
	}

	IEnumerator Openingscene()
	{
		yield return new WaitForSeconds(0.5f);
		GameControl.control.PlaySound(Dooropening,false);

		float distancemoved = 0;

		Vector3 leftmove = Leftscreen.transform.position;
		Vector3 rightmove = Rightscreen.transform.position;

		while(distancemoved <= distancescreensmove)
		{
			leftmove.x -= 0.2f;
			rightmove.x += 0.2f;
			Leftscreen.transform.position = leftmove;
			Rightscreen.transform.position = rightmove;
			//Debug.Log("tick");
			distancemoved += 0.2f;
			yield return new WaitForSeconds(0.03f);
		}

		while(Viewport.transform.localPosition != Endposition)
		{
			Viewport.transform.Translate(Vector3.forward * Time.deltaTime * movementofviewport);
			yield return new WaitForSeconds (0.01f);
			if (Viewport.transform.localPosition.z > Endposition.z)
				Viewport.transform.localPosition = Endposition;
		}

		for (float x = 0; x < 59; x+= 1f * rotationspeed)
		{
			Viewport.transform.Rotate(1f * rotationspeed,0,0);
			yield return new WaitForSeconds(0.01f);
		}
		Viewport.transform.rotation = Endrotation;

		MainGame.openingdone = true;
		this.enabled = false;
	}

}
