using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chargeroll : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
	float starttime = 0;
	public static bool recording = false;
	//private bool Searching = false;
	public float MaxTime;
	//Selectable button;

	public Text Chargingtext;

	public Image Chargebar;
	public Image Chargebarback;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!recording && MainGame.PlayerTurn && !MainGame.RollingDice)
		{
			Debug.Log("Down");
			starttime = Time.fixedTime;
			recording = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		recording = false;
	}

	void Update()
	{
		if (recording && !MainGame.RollingDice)
		{
			MainGame.Chargetimeroll = Time.fixedTime - starttime;
			if (MainGame.Chargetimeroll > MaxTime)
			{
				MainGame.Chargetimeroll = MaxTime;
				if (GameControl.control.rules.Vibrate)
				Handheld.Vibrate();
			}

			if (!Chargebar.IsActive())
			{
				Chargebar.gameObject.SetActive (true);
				Chargebarback.gameObject.SetActive (true);
			}

			Chargebar.fillAmount = (MainGame.Chargetimeroll / MaxTime);
			//Chargingtext.color = Color.black;
			//Chargingtext.text = (Mathf.FloorToInt(MainGame.Chargetimeroll / MaxTime * 100)).ToString() + "%";
			//Debug.Log ("Charging");
		}
		if (!recording && Chargebar.IsActive())
		{
			Chargebar.gameObject.SetActive(false);
			Chargebarback.gameObject.SetActive(false);
		}
			//Chargingtext.color -= new Color(0,0,0,.01f);

	}

}
