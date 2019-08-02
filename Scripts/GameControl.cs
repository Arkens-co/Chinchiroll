using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl control;
	public uint Money;
	public Rules rules = new Rules();
	public Themes themes = new Themes ();
	public DateTime lastlogin = new DateTime ();
	public int loginarow;

	public AudioSource Music;
	public AudioSource Sound;
	public AudioSource Sound2;
	public AudioSource Sound3;

	void Awake () 
	{
		if (control == null) 
		{
			DontDestroyOnLoad (gameObject);
			control = this;
			Load ();
		} 
		else if (control != null) 
		{
			Destroy (this);
		}
	}

	public void PlaySound(AudioClip sound, bool Musictrack)
	{
		if(Musictrack)
		{
			Music.clip = sound;
			Music.volume = control.rules.Musicvolume;
			Music.Play();
		}
		else
		{
			if (!Sound.isPlaying)
			{
				Sound.clip = sound;
				Sound.volume = control.rules.Soundvolume;
				Sound.Play();
			}
			else if (!Sound2.isPlaying)
			{
				Sound2.clip = sound;
				Sound2.volume = control.rules.Soundvolume;
				Sound2.Play();
			}
			else if (!Sound3.isPlaying)
			{
				Sound3.clip = sound;
				Sound3.volume = control.rules.Soundvolume;
				Sound3.Play();
			}
		}
	}

	public void UpdateMoney(bool Timeaswell)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData ();
		data.Money = Money;
		data.rules = rules;
		data.themes = themes;
		data.LastLogin = lastlogin;
		data.LoginaRow = loginarow;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			Money = data.Money;
			rules = data.rules;
			themes = data.themes;
			lastlogin = data.LastLogin;
			loginarow = data.LoginaRow;
		}
	}
}

[Serializable]
class PlayerData
{
	public uint Money;
	public Rules rules;
	public Themes themes;
	public DateTime LastLogin;
	public int LoginaRow;
}

[Serializable]
public class Rules
{
	public bool ShakeToRoll = false; //Physically must roll the dice to roll... to be implemented when taking this to device
	public bool HouseRollsFirst = true; //House will roll first always after betting
	public bool PairIsScore = false; //When you roll a pair, the pair's value is your score instead of the third number
	public int StraightsWin = 1; //1,2,3 2,3,4 and 3,4,5 are also auto wins. 0 is they all auto win, 1 is 123 and 456 are auto loss and win respectively, 2 is all are auto lose.
	public int Multiplier = 3; // Multiplier for when one gets a super win or loss that warrents more money to be given than normal.
	public float DiceSpeed = 2; // How fast the dice are rolled.  The number is the multiplier to time for how quickly it should roll.
	public float Musicvolume = 1; //How loud the music is
	public float Soundvolume = 1; //How loud the sound effects are
	public bool Annoucements = true; //Have the Announcement text constantly giving updates
	public bool Vibrate = true; //Phone vibrates on certain moments
	public bool PlayAnimations = true; //Animations play on particular wins and losses
}

[Serializable]
public class Themes
{
	public Unlocked Dices;
	public Unlocked Backgrounds;
	public Unlocked Cups;
	public Unlocked Musics;
}

[Serializable]
public class Unlocked
{
	public bool isunlocked;
	public int index;
}

