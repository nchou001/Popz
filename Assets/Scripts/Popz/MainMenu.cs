﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Button playButton;
	public Button settingsButton;

	// Use this for initialization
	void Start () {
		playButton = playButton.GetComponent<Button> ();
		settingsButton = settingsButton.GetComponent<Button> ();
	}

	public void Play()
	{
		Application.LoadLevel (1);
	}

	public void OpenSettings()
	{
		Application.LoadLevel (2);
	}
}
