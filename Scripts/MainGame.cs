using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainGame : MonoBehaviour 
{

	uint TempMoney = 0;
	int BetAmount = 0;
	int CurrentRoll = 0;
	public static bool PlayerTurn = false;
	float BetMultiplier = 1;
	int Playerscore = 0;
	int Housescore = 0;
	public static float Chargetimeroll = 0;
	public float outofboundsheight;
	public static bool openingdone = false;

	public Animator WinLossAnim;

	public Slider MusicSDR;
	public Slider SoundSDR;

	public List<Button> BTNVariables = new List<Button>();
	// 0 - 4: Bet money buttons
	//5: Roll Dice Button
	//6: Exit Button
	//7 and 8: House and Player Score
	//9-11: Individual Die results
	//12: Settings button

	public List<Text> TextVariables = new List<Text> ();
	//0 - 4: Bet money buttons
	//5 and 6: house and Player Score
	//7 - 9: Individual die results
	//10: Announcement Text
	//11: Roll Dice Button
	//12: House overhead text
	//13: Player overhead text

	List<Dice> Dices = new List<Dice>();

	public List<Vector3> Diesides = new List<Vector3>();

	public Collider Diceholder;

	public static bool RollingDice = false;

	public GameObject Die1;
	public GameObject Die2;
	public GameObject Die3;

	public Vector3 Rollposition1;
	public Vector3 Rollposition2;
	public Vector3 Rollposition3;

	public Color highlighted;
	public Color unhighlighted;
	public Color announcetext;

	public AudioClip Musictrack;
	public List<AudioClip> Sounds = new List<AudioClip>();

	public Canvas Pausescreen;

	void Start () 
	{
		WinLossAnim.Play("Neutral");
		openingdone = false;
		GameControl.control.PlaySound(Musictrack,true);
		Screen.orientation = ScreenOrientation.AutoRotation;
		//GameControl.control.rules.HouseRollsFirst = false;
		//GameControl.control.rules.Multiplier = 3;
		//GameControl.control.rules.StraightsWin = 1;
		//GameControl.control.rules.PairIsScore = false;
		//GameControl.control.rules.DiceSpeed = 2.5f;
		//GameControl.control.Money = 1000;
		//GameControl.control.rules.Annoucements = true;

		TempMoney = GameControl.control.Money;
		GameControl.control.UpdateMoney(false);

		Diesides.Add(Vector3.up);
		Diesides.Add(Vector3.forward);
		Diesides.Add(Vector3.left);

		MusicSDR.value = GameControl.control.rules.Musicvolume;
		SoundSDR.value = GameControl.control.rules.Soundvolume;

		SetDice();
	}

	void Update()
	{
		if (openingdone)
		{
			SetBetText();
			enabled = false;
		}
	}

	void Announce (string text, bool Optional)
	{
		TextVariables[10].color = announcetext;
		if (GameControl.control.rules.Annoucements && (!Optional || TextVariables[10].text == ""))
		{
			TextVariables[10].text = text;
		}
	}

	void HideorUnhideButton (Button button, Text text, bool hide)
	{
		if (hide)
		{
			button.interactable = false;
			text.raycastTarget = false;
			text.color = Color.clear;
		}
		else
		{
			button.interactable = true;
			text.raycastTarget = true;
			text.color = announcetext;
		}
	}

	void SetDice()
	{

		for(int i = 0; i < 3; i++)
		{
			Dices.Add(new Dice());
			if(i == 0)
			{
				Dices[i].Model = Die1; 
				Dices[i].pos = Rollposition1;
			}
			else if (i == 1)
			{
				Dices[i].Model = Die2; 
				Dices[i].pos = Rollposition2;
			}
			else if (i == 2)
			{
				Dices[i].Model = Die3; 
				Dices[i].pos = Rollposition3;
			}

			//Dices[i].pos = Dices[i].Model.transform.position;
			Dices[i].body = Dices[i].Model.GetComponent<Rigidbody>();
			Dices[i].value = 0;
			Dices[i].rot = Dices[i].Model.transform.rotation;

			//Debug.Log("There are " + Dices[i].Colliders.Count + " Colliders attached");
			//Debug.Log(Dices[i].Colliders[i].bounds.ToString());
		}
	}

	void SwapHighlight(int houseplayerorneither)
	{
		//-1 = house, 1 = player, 0 = neither, other = uhhhhhh
		
		ColorBlock cbh = BTNVariables[7].colors;
		ColorBlock cbp = BTNVariables[8].colors;

		if (houseplayerorneither == -1)
		{
			cbh.normalColor = highlighted;
			cbh.pressedColor = highlighted;
			cbh.highlightedColor = highlighted;
			cbp.normalColor = unhighlighted;
			cbp.highlightedColor = unhighlighted;
			cbp.pressedColor = unhighlighted;
		}
		else if (houseplayerorneither == 1)
		{
			cbh.normalColor = unhighlighted;
			cbh.pressedColor = unhighlighted;
			cbh.highlightedColor = unhighlighted;
			cbp.normalColor = highlighted;
			cbp.highlightedColor = highlighted;
			cbp.pressedColor = highlighted;

		}
		else if (houseplayerorneither == 0)
		{
			cbh.normalColor = unhighlighted;
			cbp.normalColor = unhighlighted;
		}

		BTNVariables[7].colors = cbh;
		BTNVariables[8].colors = cbp;

	}

	void SetBetText()
	{
		//0 = 10%, 1 = 25%, 2 = 50%, 3 = 75%, 4 = 100%, 5 = House Score, 6 = Player Score, 7 = Die 1, 8 = Die 2, 9 = Die 3
		TextVariables [0].text = "10%: " + (TempMoney / 10).ToString();
		TextVariables [1].text = "25%: " + (TempMoney / 4).ToString();
		TextVariables [2].text = "50%: " + (TempMoney / 2).ToString();
		TextVariables [3].text = "75%: " + (TempMoney * 3 / 4 ).ToString();
		TextVariables [4].text = "All " + (TempMoney / 1).ToString() + " of it";

		for (int t = 0; t < 5; t++) 
		{
			HideorUnhideButton(BTNVariables[t],TextVariables[t],false);
		}

		HideorUnhideButton (BTNVariables[5],TextVariables[11],true);
		HideorUnhideButton (BTNVariables[14],TextVariables[15],false);
		HideorUnhideButton (BTNVariables[12],TextVariables[16],false);
		HideorUnhideButton (BTNVariables[15],TextVariables[10],false);
		Announce ("Place your bet", true);
		TextVariables[15].text = "Money: \n" + TempMoney;
	}

	public void SetMoney(float betPercentage)
	{
		for (int t = 0; t < 5; t++) 
		{
			HideorUnhideButton (BTNVariables[t],TextVariables[t],true);
		}
		//RollDiceBTN.interactable = true;

		GameControl.control.PlaySound(Sounds[0],false);
		
		if (TempMoney == 0)
		{
			Debug.Log ("YOU HAVE NO MONEY!  GET OUT OF HERE POOR PERSON!");
			Announce ("One needs funds to actually bet...", false);
			return;
		}

		if (BetAmount == 0 && betPercentage != 0)
		{
			BetAmount = (int)Mathf.Floor(TempMoney * betPercentage / 100);
			//Debug.Log ("Betted " + BetAmount);
			
			HideorUnhideButton(BTNVariables[7],TextVariables[5],false);
			HideorUnhideButton(BTNVariables[8],TextVariables[6],false);

			TextVariables [5].text = "";
			TextVariables [6].text = "";

			TextVariables [12].color = announcetext;
			TextVariables [13].color = announcetext;

			HideorUnhideButton(BTNVariables[9],TextVariables[7],false);
			HideorUnhideButton(BTNVariables[10],TextVariables[8],false);
			HideorUnhideButton(BTNVariables[11],TextVariables[9],false);

			HideorUnhideButton(BTNVariables[12],TextVariables[7],false);

			HideorUnhideButton(BTNVariables[13],TextVariables[14],false);
			TextVariables[14].text = "Bet: " + BetAmount;

			Announce ("", false);

			if (GameControl.control.rules.HouseRollsFirst)
			{
				Announce ("House rolls first", true);
				//Debug.Log ("Houses Turn!");
				StartCoroutine(RollPhysicalDice(false,0));//RollDice (false);
				//BTNVariables[8].interactable = false;
				//BTNVariables[7].interactable = true;
				SwapHighlight(-1);
			}
			else
			{
				Announce ("You roll first", true);
				PlayerTurn = true;
				//BTNVariables[8].interactable = true;
				//BTNVariables[7].interactable = false;
				SwapHighlight(1);
				GameControl.control.PlaySound(Sounds[11],false);
				HideorUnhideButton (BTNVariables[5],TextVariables[11],false);
			}
		}
	}
		
	IEnumerator RollPhysicalDice(bool PlayerorHouse, float timecharged)
	{
		Bowl.outofbounds = false;
		int r = Random.Range(1,10);
		GameControl.control.PlaySound(Sounds[r],false);
		//Debug.Log("Played Sound " + r);

		foreach(var i in Dices)
		{
			i.Model.transform.localPosition = i.pos;
			i.Model.transform.rotation = i.rot;
			i.body.velocity = Vector3.zero;
			i.body.angularVelocity = Vector3.zero;
		}
		
		RollingDice = true;
		Vector3 Randomforce = Vector3.one;
		Vector3 RandomAngle;
		//Diceholder.enabled = false;
		Time.timeScale = GameControl.control.rules.DiceSpeed;

		for (int i = 0 ; i < Dices.Count; i++) 
		{

			if (!PlayerorHouse)
				Randomforce = new Vector3(Random.Range(-400, -150) * Dices[i].body.mass,Random.Range(-400, -150) * Dices[i].body.mass, 0);
			else if (PlayerorHouse)
			{
				Debug.Log("Timecharged is " + timecharged);
				float x = (-65 * timecharged - Random.Range(0, 130) - 65) * Dices[i].body.mass;
				float y = (-65 * timecharged - Random.Range(0, 130) - 65) * Dices[i].body.mass;
				float z = 0;

				Randomforce = new Vector3(x,y,z);
			}

			//RandomAngle = new Vector3 (90,90,90);
			RandomAngle = new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360));
			Dices[i].body.AddForce(Randomforce);
			Dices[i].Model.transform.Rotate(RandomAngle);
			//Dices[i].body.rotation.eulerAngles.Set(RandomAngle.x,RandomAngle.y,RandomAngle.z);
			//Dices[i].body.AddTorque(RandomAngle);
		}
		yield return new WaitForSeconds(4f);

		for (int j = 0; j < Dices.Count; j++) 
		{
			Dices [j].value = WhichIsUp (Dices [j].Model);

			if (Bowl.outofbounds)
			{
				Dices [j].value = -10;
			}
			if (Dices [j].value != -10)
				TextVariables [j + 7].text = Dices [j].value.ToString ();
			else 
				TextVariables [j + 7].text = "X";
		}

		Time.timeScale = 1f;
		Dices.Sort(SortByScore);

		//Diceholder.enabled = true;

		RollingDice = false;

		DetermineScore (PlayerorHouse);


		if (PlayerorHouse)
			Playerturnend();
		else
			Houseturnend();

		//yield return new WaitForSeconds(0.01f);
	}

	int WhichIsUp(GameObject Die)  
	{
		float[] Angles = new float[6];
		int result = 0;

		Angles[4] = Vector3.Angle(Die.transform.right, Vector3.up);
		Angles[2] = Vector3.Angle(Die.transform.up, Vector3.up);
		Angles[5] = Vector3.Angle(-Die.transform.forward, Vector3.up);
		Angles[0] = Vector3.Angle(Die.transform.forward, Vector3.up);
		Angles[3] = Vector3.Angle(-Die.transform.up, Vector3.up);
		Angles[1] = Vector3.Angle(-Die.transform.right, Vector3.up);

		for (int i = 0; i < 6; i++)
		{
			//Debug.Log (i + " is " + Angles[i]);
			if (Angles[0] >= Angles[i])
			{
				result = i+1;
				Angles[0] = Angles[i];
			}
		}
		//Debug.Log ("Result is " + result);
		return result;
		/*float maxY = Mathf.NegativeInfinity;
		int result = -1;

		for(int i = 0; i < 3; i++) 
		{
			// Transform the vector to world-space:
			Vector3 worldSpace = Die.transform.TransformDirection(Diesides[i]);
			if(worldSpace.y > maxY) 
			{
				result = i+4; // index 0 is 1
				maxY=worldSpace.y;
			}
			if (-worldSpace.y > maxY) 
			{ // also check opposite side
				result = 3-i; // sum of opposite sides = 7
				maxY=-worldSpace.y;
			}
		}
		return result;
	*/
	}

	void Playerturnend()
	{
		if (Playerscore > 70 || -70 > Playerscore)
			ApplyWinner ();

		if (CurrentRoll >= 3 || Playerscore != 0) 
		{
			
			if (GameControl.control.rules.HouseRollsFirst)
				ApplyWinner ();
			else
			{
				SwapHighlight(-1);
				CurrentRoll = 0;
				Announce ("House's Roll", true);
				StartCoroutine(RollPhysicalDice(false,0));//RollDice (false);
			}
		}
	}

	void Houseturnend()
	{
		if (Housescore == 0 && CurrentRoll < 2)
		{
			CurrentRoll++;
			StartCoroutine (RollPhysicalDice(false,0));
			return;
		}
		else
			CurrentRoll = 0;

		if (Housescore > 70 || -70 > Housescore || !GameControl.control.rules.HouseRollsFirst)
			ApplyWinner ();
		else
		{
			Announce ("Your Roll", true);
			//Debug.Log("Players turn!");
			PlayerTurn = true;
			//BTNVariables[8].colors.normalColor = highlighted;
			//BTNVariables[7].interactable = false;
			SwapHighlight(1);
			GameControl.control.PlaySound(Sounds[11],false);

			HideorUnhideButton (BTNVariables[5], TextVariables[11],false);
		}
			
	}

	static int SortByScore(Dice p1, Dice p2)
	{
		return p1.value.CompareTo(p2.value);
	}

	void DetermineScore(bool PlayerorHouse)
	{
		//Bowl.numbinbowl = 0;
		//GameControl.control.PlaySound(Sounds[0],false);
		int score = 0;
		Announce ("",false);
		//Straights

		if (GameControl.control.rules.StraightsWin != 1)
		{
			if (Dices[0].value == 2 && Dices[1].value == 3 && Dices[2].value == 4) 
			{
				if (GameControl.control.rules.StraightsWin == 0)
					score = 100;
				else
					score = -100;
			}
			else if (Dices[0].value == 3 && Dices[1].value == 4 && Dices[2].value == 5)
			{
				if (GameControl.control.rules.StraightsWin == 0)
					score = 100;
				else
					score = -100;
			}
		}

		if (Dices[0].value == 4 && Dices[1].value == 5 && Dices[2].value == 6) 
		{
			if (GameControl.control.rules.StraightsWin <= 1)
			{
				score = 100;
				Announce ("4,5,6 is an instant win", false);
			}
			else
				score = -100;
		}

		else if (Dices[0].value == 1 && Dices[1].value == 2 && Dices[2].value == 3) 
		{
			if (GameControl.control.rules.StraightsWin == 0)
				score = 100;
			else
			{
				score = -100;
				Announce ("1,2,3 is an instant loss", false);
			}
		}


		//Trips
		if (Dices[1].value == Dices[0].value && Dices[1].value == Dices[2].value && Dices[1].value != -10) 
		{
			if (Dices[1].value == 6) 
			{
				Announce ("6,6,6 is not only an instant win, they award X3 money", false);
				BetMultiplier = GameControl.control.rules.Multiplier;
				score = 666;
				//WinLossAnim.Play("Storm Lose");
			} 

			else if (Dices[1].value == 1) 
			{
				Announce ("1,1,1 is not only an instant loss, they lose X3 money", false);
				BetMultiplier = GameControl.control.rules.Multiplier;
				score = -666;
			}

			else
			{
				Debug.Log("Trigger only on trip");
				Announce ("Trips can only be beaten by a higher triple and offer X2 money", false);
				BetMultiplier = 2;
				score = Dices[1].value * 10;
			}
		}

		//Pairs
		else if (Dices[1].value == Dices[0].value)
		{
			if (GameControl.control.rules.PairIsScore)
				score = Dices[1].value;
			else
			{
				score = Dices[2].value;
				//Announce ("Pairs use the non matching number to determine score", false);
			}
				
		}

		else if (Dices[1].value == Dices[2].value)
		{
			if (GameControl.control.rules.PairIsScore)
				score = Dices[1].value;
			else
			{
				score = Dices[0].value;
				//Announce ("You need a higher score to win", false);
			}
				
		}

		//Ringout

		if (Dices [0].value == -10 || Dices [1].value == -10 || Dices [2].value == -10) 
		{
			score = -108;
			Announce("Dice falling out of the bowl is an instant loss", false);
		}
		//End
		if (PlayerorHouse) 
		{
			//GameControl.control.PlaySound(Sounds[11],false);
			Playerscore = score;
			CurrentRoll++;
			//Debug.Log("Player's's Score is " + Playerscore);

			switch (score)
			{

			case 1:
				TextVariables [6].text = score.ToString();
				break;
			case 2:
				TextVariables [6].text = score.ToString();
				break;
			case 3:
				TextVariables [6].text = score.ToString();
				break;
			case 4:
				TextVariables [6].text = score.ToString();
				break;
			case 5:
				TextVariables [6].text = score.ToString();
				break;
			case 6:
				TextVariables [6].text = score.ToString();
				break;
			case 20:
				TextVariables [6].text = "TRIP! " + (score / 10) + "s";
				break;
			case 30:
				TextVariables [6].text = "TRIP! " + (score / 10) + "s";
				break;
			case 40:
				TextVariables [6].text = "TRIP! " + (score / 10) + "s";
				break;
			case 50:
				TextVariables [6].text = "TRIP! " + (score / 10) + "s";
				break;
			case -108:
				TextVariables [6].text = "Out of Bounds";
				break;
			case 100:
				TextVariables [6].text = "Super!";
				break;
			case -100:
				TextVariables [6].text = "Bad luck";
				break;
			case -666:
				TextVariables [6].text = "...Oops";
				break;
			case 666:
				TextVariables [6].text = "EXTREME!";
				break;
			case 0:
				if (CurrentRoll > 2)
					TextVariables[6].text = "No Score";
				else
				{
					Announce("You rolled nothing, you get 3 chances to roll a score", true);
					TextVariables[6].text = "Roll " + (CurrentRoll + 1) + "/3";
				}
				break;
			}
		}
		else 
		{
			Housescore = score;
			//Debug.Log("House's Score is " + Housescore);
			if (0 < score && score < 10)
				TextVariables [5].text = score.ToString();
			else if (score > 10 && score < 60)
				TextVariables [5].text = "TRIP! " + (score / 10) + "s";
			else if (score == -108)
				TextVariables [5].text = "Out of Bounds";
			else if (score == 100)
				TextVariables [5].text = "Super!";
			else if (score == -100)
				TextVariables [5].text = "Bad luck";
			else if (score == -666)
				TextVariables [5].text = "...Oops";
			else if (score == 666)
				TextVariables [5].text = "EXTREME!";
			else if (score == 0 && CurrentRoll > 1)
				TextVariables[5].text = "No Score";
			else
			{
				Announce("House gets 3 chances to roll a score", true);
				TextVariables[5].text = "Roll " + (CurrentRoll + 2) + "/3";
			}
		}

	}


	void ApplyWinner ()
	{
		//BTNVariables[8].interactable = false;
		//BTNVariables[7].interactable = false;
		SwapHighlight(0);
		if (Housescore < Playerscore) 
		{
			//WinLossAnim.Play("YouWin");
			TempMoney += (uint)(BetAmount * BetMultiplier);
			Announce ("You Win", true);

			if (!GameControl.control.rules.PlayAnimations)
			{
				if (BetMultiplier == 2)
					WinLossAnim.Play("Win");
				else if (BetMultiplier == 3)
					WinLossAnim.Play("Storm Win");
			}
		}
		else if (Housescore > Playerscore)
		{
			if ((uint)(BetAmount * BetMultiplier) > TempMoney)
				TempMoney = 0;
			else
				TempMoney -= (uint)(BetAmount * BetMultiplier);

			if (!GameControl.control.rules.PlayAnimations)
			{

				if (BetMultiplier == 2)
					WinLossAnim.Play("Lose");
				else if (BetMultiplier == 3)
					WinLossAnim.Play("Storm Lose");
			}

			//WinLossAnim.Play("YouLose");
			Announce ("You Lose", true);
		}
		else if (Housescore == Playerscore)
		{
			Announce ("Scores are equal, no winner", true);
		}


		//Debug.Log ("Money is currently " + TempMoney);
		GameControl.control.Money = TempMoney;
		GameControl.control.UpdateMoney(false);
		ResetBoard ();
	}

	void ResetBoard ()
	{
		GameControl.control.PlaySound(Sounds[0],false);
		BetAmount = 0;
		PlayerTurn = false;
		BetMultiplier = 1;
		Playerscore = 0;
		Housescore = 0;
		CurrentRoll = 0;
		HideorUnhideButton(BTNVariables[13],TextVariables[14],true);

		SetBetText ();
	}

	public void PlayerRoll(Button BTN)
	{
		if (PlayerTurn && CurrentRoll < 3 && Playerscore == 0 && !RollingDice) 
		{
			//Debug.Log("Chargetime was " + Chargetimeroll);
			Announce("",false);
			StartCoroutine(RollPhysicalDice(true, Chargetimeroll));
			Chargeroll.recording = false;
			Chargetimeroll = 0;
			//RollDice (true);
		} 
	}

	public void ExittoMainGame()
	{
		GameControl.control.PlaySound(Sounds[0],false);
		if (BetAmount == 0)
			SceneManager.LoadScene (0);
		else
			Announce("It's wrong to leave mid bet.", false);
	}

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

	public void Pausescreenopen()
	{
		GameControl.control.PlaySound(Sounds[0],false);
		if (Pausescreen.enabled == true)
			Pausescreen.enabled = false;
		else
			Pausescreen.enabled = true;
	}
}

public class Dice
{
	public GameObject Model;
	public int value;
	public Rigidbody body;
	public Vector3 pos;
	public Quaternion rot;
	public bool Inbowl = false;
}