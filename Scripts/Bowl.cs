using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour 
{

	public static int numbinbowl = 0;
	public static bool outofbounds = false;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Die has left bowl");
		if (other.gameObject.tag == "Die")
			outofbounds = true;
	}

	void OnCollisionEnter()
	{
		Debug.Log ("Die has hit the table");
		outofbounds = true;
	}
}
