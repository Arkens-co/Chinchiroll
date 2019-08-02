using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class TitleScreen : MonoBehaviour 
{
	public SceneLoader NextScene;
	public Text PlayersMoney;
	public GameObject Options;
	public GameObject Shop;
	public GameObject Tutorial;
	public GameObject Announcement;
	public GameObject OpeningScreen;

	public AudioClip Musictrack;
	public List<AudioClip> Sounds = new List<AudioClip>();

	public Sprite Blankbutton;

	public List<GameObject> MenuButtons = new List<GameObject>();

	private Vector2 StoredButtonSize = new Vector2 ();
	private Sprite Storedsprite;

	private bool Wideningalreadygoing = false;
	private float Screenwidth;

	//Options Screen

	public Slider MusicSDR;
	public Slider SoundSDR;
	public Slider DicespeedSDR;

	public Button[] OptionButtons;
	//House Rolls First 1-2
	//Vibrate 3-4


	//Shop Screen

	public Text PlayerMoneyReminderTXT;

	//Tutorial

	public GameObject Taehae;
	public GameObject[] TutorialScreens;
	private int Tutorialindex = 0;

	//Use this for initialization
	void Start () 
	{
		for (int o = 0; o < TutorialScreens.Length; o++)
		{
			if (o == Tutorialindex)
				TutorialScreens[o].SetActive(true);
			else
				TutorialScreens[o].SetActive(false);
		}

		OpeningScreen.SetActive(true);

		Screenwidth = OpeningScreen.GetComponent<RectTransform>().rect.width;

		//RectTransform temp = Options.GetComponent<RectTransform>();
		//temp.sizeDelta = new Vector2(Screen.width, Screen.height);
		Options.transform.localPosition = new Vector3(-Screenwidth,0,0);
		Options.SetActive(false);

		//RectTransform temp2 = Tutorial.GetComponent<RectTransform>();
		//temp2.sizeDelta = new Vector2(Screen.width, Screen.height);
		Tutorial.transform.localPosition = new Vector3(Screenwidth,0,0);
		Tutorial.SetActive(false);

		//RectTransform temp3 = OpeningScreen.GetComponent<RectTransform>();
		//temp3.sizeDelta = new Vector2(Screen.width, Screen.height);
		OpeningScreen.transform.localPosition = new Vector3(0,0,0);


		Screen.orientation = ScreenOrientation.AutoRotation;
		GameControl.control.PlaySound(Musictrack,true);

		if (GameControl.control.lastlogin.AddDays (1) < DateTime.Now) 
		{
			Announcement.SetActive(true);
			Text Announcementtxt = Announcement.GetComponentInChildren<Text>();

			Announcementtxt.text = "Congratulations, you have received a Daily Reward!";

			if(GameControl.control.lastlogin.AddDays (3) < DateTime.Now)
			{
				GameControl.control.loginarow = 1;
				Announcementtxt.text += "\nYou got 100 gold.\nLog in daily to receive a greater reward!";
			}
			else if (GameControl.control.loginarow >= 5)
			{
				GameControl.control.loginarow = 5;
				Announcementtxt.text += "\nYou received 1600 gold!\nYour daily reward won't increase anymore, but you'll still acrue gold.";
			}
			else
			{
				GameControl.control.loginarow += 1;
				Announcementtxt.text += "\nYou received " + (50 * Math.Pow(2, GameControl.control.loginarow)) + " gold.\nKeep it going!";
			}

			GameControl.control.lastlogin = DateTime.Now;

			GameControl.control.Money += (uint)(50 * Math.Pow(2, GameControl.control.loginarow));
				//On first day, 100 gold, second 200 gold, 
			GameControl.control.UpdateMoney(true);
			Announcementtxt.text += "\nTap to close.";
		}
		PlayersMoney.text = GameControl.control.Money.ToString();
	}

	public void SetOptions ()
	{
		Options.SetActive(true);
		MusicSDR.value = GameControl.control.rules.Musicvolume;
		SoundSDR.value = GameControl.control.rules.Soundvolume;

		DicespeedSDR.value = (GameControl.control.rules.DiceSpeed - 1) * 4;

		SwitchOption("HouseRollsFirst");
		SwitchOption("Vibrate");
		SwitchOption("Announce");
		SwitchOption("Anim");

	}

	public void SetShop ()
	{
		PlayerMoneyReminderTXT.text = GameControl.control.Money.ToString();
	}

	public void ShiftButtons(bool GoingRight)
	{
		GameControl.control.PlaySound(Sounds[0],false);
		Vector3 temppos;
		Vector3 tempscale;
		int tempindex;

		Vector3 lastpos;
		Vector3 lastscale;
		int lastindex;
		bool lastinteractable;

		if (!GoingRight)
		{
			lastpos = MenuButtons [0].transform.position;
			lastscale = MenuButtons [0].transform.localScale;
			lastindex = MenuButtons [0].transform.GetSiblingIndex();
			lastinteractable = MenuButtons[0].GetComponent<Button>().interactable;
				

			for (int f = 0; f < MenuButtons.Count; f++) 
			{
				if (f == MenuButtons.Count - 1) 
				{
					MenuButtons [f].transform.position = lastpos;
					MenuButtons [f].transform.localScale = lastscale;
					MenuButtons [f].transform.SetSiblingIndex (lastindex);
					MenuButtons[f].GetComponent<Button>().interactable = lastinteractable;
				}
				else 
				{
					temppos = MenuButtons [f + 1].transform.position;
					tempscale = MenuButtons [f + 1].transform.localScale;
					tempindex = MenuButtons [f + 1].transform.GetSiblingIndex ();

					MenuButtons [f].transform.position = temppos;
					MenuButtons [f].transform.localScale = tempscale;
					MenuButtons [f].transform.SetSiblingIndex (tempindex);
					MenuButtons[f].GetComponent<Button>().interactable = MenuButtons[f+1].GetComponent<Button>().interactable;
				}
			}
			Debug.Log ("Shifted Buttons Left");
		}
		else 
		{
			lastpos = MenuButtons [MenuButtons.Count-1].transform.position;
			lastscale = MenuButtons [MenuButtons.Count-1].transform.localScale;
			lastindex = MenuButtons [MenuButtons.Count - 1].transform.GetSiblingIndex ();
			lastinteractable = MenuButtons [MenuButtons.Count - 1].GetComponent<Button> ().interactable;
			for (int f = MenuButtons.Count-1; f > -1; f--) 
			{
				if (f == 0) 
				{
					MenuButtons [f].transform.position = lastpos;
					MenuButtons [f].transform.localScale = lastscale;
					MenuButtons [f].transform.SetSiblingIndex (lastindex);
					MenuButtons [f].GetComponent<Button> ().interactable = lastinteractable;
				}
				else 
				{
					temppos = MenuButtons [f - 1].transform.position;
					tempscale = MenuButtons [f - 1].transform.localScale;
					tempindex = MenuButtons [f - 1].transform.GetSiblingIndex ();

					MenuButtons [f].transform.position = temppos;
					MenuButtons [f].transform.localScale = tempscale;
					MenuButtons [f].transform.SetSiblingIndex (tempindex);
					MenuButtons [f].GetComponent<Button> ().interactable = MenuButtons [f - 1].GetComponent<Button> ().interactable;
				}
			}
			Debug.Log ("Shifted Buttons Right");

		}
	}

	public void PressedButton(int buttonindex)
	{
		GameControl.control.PlaySound(Sounds[0],false);
		if (buttonindex == 0)
			//SceneManager.LoadScene (1);
			NextScene.Startloading();
		else if (buttonindex == 1)
			Application.Quit();	
		else
		{
			Debug.Log("Moving Buttons at index " + buttonindex);
			StartCoroutine(MoveButton(buttonindex));
			//MenuButtons[3].GetComponent<Button>().interactable = false;
			//StartCoroutine(WidenButton(3));
		}
	}

	IEnumerator MoveButton (int x)
	{
		if (Wideningalreadygoing)
			yield break;
		else
			Wideningalreadygoing = true;


		float goal = Screenwidth;
		switch (x)
		{
		case 2:
			SetOptions();
			for (int i = 0; i < 30; i++)
			{
				OpeningScreen.transform.localPosition += new Vector3 (goal/30,0,0);
				if (Options.transform.localPosition.x < 0)
					Options.transform.localPosition += new Vector3 (goal/30,0,0);
				yield return new WaitForSeconds(0.01f);
			}
			Options.gameObject.transform.localPosition = new Vector3(0,0,0);
			OpeningScreen.SetActive(false);
			break;

		case 3:
			GameControl.control.UpdateMoney(false);
			OpeningScreen.SetActive(true);
			for (int j = 0; j < 30; j++)
			{
				OpeningScreen.transform.localPosition += new Vector3 (-goal/30,0,0);
				Options.transform.localPosition += new Vector3 (-goal/30,0,0);
				yield return new WaitForSeconds(0.01f);
			}
			Options.SetActive(false);
			//Options.gameObject.transform.position = new Vector3(goal,0,0);
			break;

		case 4:
			Tutorial.SetActive(true);
			for (int i = 0; i < 30; i++)
			{
				OpeningScreen.transform.localPosition += new Vector3 (-goal/30,0,0);
				if (Tutorial.transform.localPosition.x < 0)
					Tutorial.transform.localPosition += new Vector3 (goal/30,0,0);
				Tutorial.transform.localPosition += new Vector3 (-goal/30,0,0);
				yield return new WaitForSeconds(0.01f);
			}
			Tutorial.transform.localPosition = Vector3.zero;
			break;

		case 5:
			for (int i = 0; i < 30; i++)
			{
				OpeningScreen.transform.localPosition += new Vector3 (goal/30,0,0);
				Tutorial.transform.localPosition += new Vector3 (goal/30,0,0);
				yield return new WaitForSeconds(0.01f);
			}
			Tutorial.SetActive(false);
			break;

		case 6:
			break;

		case 7:
			break;
		}
		Wideningalreadygoing = false;
	}
	/*
	IEnumerator WidenButton(int x)
	{
		if (Wideningalreadygoing)
			yield break;
		else
			Wideningalreadygoing = true;
		
		Storedsprite = MenuButtons [x].GetComponent<Image> ().sprite;
		MenuButtons[x].GetComponent<Image>().sprite = Blankbutton;

		RectTransform goal = Options.GetComponent<RectTransform>();
		RectTransform temprect = MenuButtons[x].GetComponent<RectTransform>();

		float tempwidth = temprect.sizeDelta.x;//.rect.width;
		float tempheight = temprect.sizeDelta.y;//.rect.height;

		StoredButtonSize = new Vector2 (tempwidth, tempheight);

		float goalwidth = goal.rect.width;
		float goalheight = goal.rect.height;

		Debug.Log("scaling" + tempwidth + ", " + tempheight + ", " + goalwidth + ", " + goalheight);
		while (tempwidth < goalwidth)
		{
			tempwidth *= 1.2f;

			if (tempwidth > goalwidth)
				tempwidth = goalwidth;

			yield return new WaitForSeconds(0.02f);
			temprect.sizeDelta = new Vector2 (tempwidth, tempheight);
		}
		while (tempheight < goalheight) 
		{
			tempheight *= 1.2f;

			if (tempheight > goalheight)
				tempheight = goalheight;

			yield return new WaitForSeconds (0.02f);

			temprect.sizeDelta = new Vector2 (tempwidth, tempheight);
		}
		Debug.Log ("Done Scaling" + tempwidth + ", " + tempheight + ", " + goalwidth + ", " + goalheight);

		MenuButtons [x].SetActive (false);
		if (x == 1)
		{
			Options.SetActive (true);
			SetOptions();
		}
		else if (x == 3)
			Tutorial.SetActive (true);
		else if (x == 4)
		{
			Shop.SetActive (true);
			SetShop();
		}
		Wideningalreadygoing = false;
	}

	public void ExitPopupScreen(int y)
	{
		GameControl.control.PlaySound(Sounds[0],false);
		Options.SetActive (false);
	
		Shop.SetActive (false);
	
		Tutorial.SetActive(false);

		MenuButtons [y].GetComponent<RectTransform> ().sizeDelta = new Vector2 (StoredButtonSize.x, StoredButtonSize.y);

		MenuButtons [y].GetComponent<Image> ().sprite = Storedsprite;

		MenuButtons [y].SetActive (true);

		GameControl.control.UpdateMoney(false);
	}
*/
	public void Volume(Slider slider)
	{
		if (slider.name.Contains("Music"))
		{
			GameControl.control.rules.Musicvolume = (float)slider.value;
			GameControl.control.Music.volume = (float)slider.value;
		}
		else if (slider.name.Contains("Sound"))
			GameControl.control.rules.Soundvolume = slider.value;
	}

	public void Dicespeed(Slider slider)
	{
		GameControl.control.rules.DiceSpeed = 1 + (slider.value/4);
	}

	public void RemoveAnnouncementScreen()
	{
		GameControl.control.PlaySound(Sounds[0],false);
		Announcement.SetActive(false);
	}

	public void SwitchOption(String Option)
	{
		int onoroff;

		if(Option.Contains(" :On"))
		{
			GameControl.control.PlaySound(Sounds[0],false);
			onoroff = 1;
			Option = Option.Substring(0,Option.Length-4);
		}
		else if (Option.Contains(" :Off"))
		{
			GameControl.control.PlaySound(Sounds[0],false);
			onoroff = -1;
			Option = Option.Substring(0,Option.Length-5);
		}
		else
			onoroff = 0;

		switch (Option)
		{

		case "HouseRollsFirst":
			if (onoroff == 1)
			{
				//Debug.Log("House Rolls First is now on");
				GameControl.control.rules.HouseRollsFirst = true;
				OptionButtons[0].interactable = false;
				OptionButtons[1].interactable = true;
			}

			else if (onoroff == -1)
			{
				//Debug.Log ("House Rolls First is now off");
				GameControl.control.rules.HouseRollsFirst = false;
				OptionButtons[0].interactable = true;
				OptionButtons[1].interactable = false;
			}

			else
			{
				//Debug.Log ("going with what was already there");
				OptionButtons[0].interactable = !GameControl.control.rules.HouseRollsFirst;
				OptionButtons[1].interactable = GameControl.control.rules.HouseRollsFirst;
			}

			break;

		case "Vibrate":
			if (onoroff == 1)
			{
				//Debug.Log("House Rolls First is now on");
				GameControl.control.rules.Vibrate = true;
				OptionButtons[2].interactable = false;
				OptionButtons[3].interactable = true;
			}

			else if (onoroff == -1)
			{
				//Debug.Log ("House Rolls First is now off");
				GameControl.control.rules.Vibrate = false;
				OptionButtons[2].interactable = true;
				OptionButtons[3].interactable = false;
			}

			else
			{
				//Debug.Log ("going with what was already there");
				OptionButtons[2].interactable = !GameControl.control.rules.Vibrate;
				OptionButtons[3].interactable = GameControl.control.rules.Vibrate;
			}

			break;

		case "Announce":
			if (onoroff == 1)
			{
				//Debug.Log("House Rolls First is now on");
				GameControl.control.rules.Annoucements = true;
				OptionButtons[4].interactable = false;
				OptionButtons[5].interactable = true;
			}

			else if (onoroff == -1)
			{
				//Debug.Log ("House Rolls First is now off");
				GameControl.control.rules.Annoucements = false;
				OptionButtons[4].interactable = true;
				OptionButtons[5].interactable = false;
			}

			else
			{
				//Debug.Log ("going with what was already there");
				OptionButtons[4].interactable = !GameControl.control.rules.Annoucements;
				OptionButtons[5].interactable = GameControl.control.rules.Annoucements;
			}

			break;			

		case "Anim":
			if (onoroff == 1)
			{
				//Debug.Log("House Rolls First is now on");
				GameControl.control.rules.PlayAnimations = true;
				OptionButtons[6].interactable = false;
				OptionButtons[7].interactable = true;
			}

			else if (onoroff == -1)
			{
				//Debug.Log ("House Rolls First is now off");
				GameControl.control.rules.PlayAnimations = false;
				OptionButtons[6].interactable = true;
				OptionButtons[7].interactable = false;
			}

			else
			{
				//Debug.Log ("going with what was already there");
				OptionButtons[6].interactable = !GameControl.control.rules.PlayAnimations;
				OptionButtons[7].interactable = GameControl.control.rules.PlayAnimations;
			}

			break;	
		}
		//return;
	}

	public void TutorialAdvance(bool Orreverse)
	{
		GameControl.control.PlaySound(Sounds[0],false);

		TutorialScreens[Tutorialindex].SetActive(false);

		if (Orreverse)
			Tutorialindex--;
		else
			Tutorialindex++;

		if (Tutorialindex >= TutorialScreens.Length)
			Tutorialindex = 0;
		else if (Tutorialindex < 0)
			Tutorialindex = TutorialScreens.Length - 1;
		
		TutorialScreens[Tutorialindex].SetActive(true);


	}
}
