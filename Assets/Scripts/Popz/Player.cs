﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool jumpEnabled = true;
	public float jumpingSpeed = 5f;
	public float runningSpeed = 1.7f;

	public bool canRun = true;
	public bool canJump = true;

	// For testing purposes
	public bool canDoubleJump = true;
	private float[] platformPositions;
	private int currentPlatform;

	private bool platforms = true;

	private float screenBottom;
	private PatternLevelManager levelManager;
	// Use this for initialization
	void Start() {
//
//		if (Settings.isSet) {
//			platforms = Settings.togglePlatformsNback;
//		}
//
//		if (platforms) {
//			//runningSpeed = 7.0f;
//		}
//
//		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
//		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<PatternLevelManager> ();
//		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
//		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
//		bottomLeft.y = Camera.main.GetComponent<FixedHeight> ().height - (topRight.y - bottomLeft.y)/2f;
//		Vector3 pos = bottomLeft + grid.GridToWorld (1, 2);
//		transform.position = pos;
//		screenBottom = bottomLeft.y - 10f;
//
//		platformPositions = new float[3];
//		Debug.Log (grid.numCellsY);
//		platformPositions [2] = grid.GridToWorld (0, 6).y - (grid.cellSizeY / 2.0f); // + (grid.cellSizeY / 2.0f);
//		platformPositions [1] = grid.GridToWorld (0, 3).y - (grid.cellSizeY / 2.0f); // + (grid.cellSizeY / 2.0f);
//		platformPositions [0] = grid.GridToWorld (0, 0).y; // + (grid.cellSizeY / 2.0f);
//
//		Debug.Log (grid.GridToWorld (0, 0));
//		Debug.Log ("Cell Size Y: " + grid.cellSizeY);
//		currentPlatform = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (jumpEnabled && canJump && Input.GetKeyDown ("space")){
			Jump ();
		}
		if (canRun) {
			transform.Translate(new Vector3(runningSpeed * Time.deltaTime, 0f, 0f));
		}
		UpdateTouch ();
		NbackPlatformsInput ();
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.y == 1) {
					canJump = true;
				}
			}
		}
		else if (col.gameObject.tag.Equals("Hill")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.x == -1) {
					canRun = false;
				}
				else if (cp.normal.y == 1) {
					canJump = true;
				}
			}
		}
	}
	
	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Ground")) {
			if (!canDoubleJump) {
				canJump = false;
			}
		}
		else if (col.gameObject.tag.Equals("Hill")) {
			foreach (ContactPoint2D cp in col.contacts) {
				if (cp.normal.x == -1) {
					canRun = true;
				}
				else if (cp.normal.y == 1) {
					canJump = false;
				}
			}
		}
	}

	public bool IsRunning { get { return canRun; } } 

	void UpdateTouch () {
		PopzGameManager gameMngr = FindObjectOfType (typeof(PopzGameManager)) as PopzGameManager;
		if (!gameMngr.Modes().Contains (GameModes.Nback)) {
			return;
		}

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Jump ();
			}
		}
	}

	void Jump () {
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpingSpeed);
	}

	void OnSwipeUp () {
		Jump ();
	}

	private void SetPositionByPlatform(int platform) {
		Debug.Log ("Current: " + transform.position);
		Debug.Log ("Attempting to switch to platform: " + platform);
		Vector3 current = transform.position;
		current.y = platformPositions [platform];
		transform.position = current;
		Debug.Log ("Moving to: " + transform.position);
		Camera.main.GetComponent<FixedHeight> ().FixPosition ();
	}

	private void NbackPlatformsInput () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (currentPlatform < platformPositions.Length - 1) {
				++currentPlatform;
				SetPositionByPlatform(currentPlatform);
			}
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (currentPlatform > 0) {
				--currentPlatform;
				SetPositionByPlatform(currentPlatform);
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			//transform.Translate(new Vector3(-5.0f * Time.deltaTime, 0, 0));
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			//transform.Translate(new Vector3(5.0f * Time.deltaTime, 0, 0));
		}
	}
}
