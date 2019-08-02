using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour 
{

	private bool loadScene = false;

	[SerializeField]
	private int scene;

	[SerializeField]
	private Text loadingText;

	public void Startloading ()
	{
		if (loadScene)
			return;
		loadScene = true;

		loadingText.text = "Loading...";

		StartCoroutine(LoadNewScene());
	}

	IEnumerator LoadNewScene()
	{
		yield return new WaitForSeconds(1);

		AsyncOperation async = SceneManager.LoadSceneAsync(scene);

		while (!async.isDone)
		{
			yield return null;
		}
	}
}
