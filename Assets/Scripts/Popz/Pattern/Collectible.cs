﻿/*
 * 
 * 
 * OLD
 * 
 * 
 * using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Species { Plant1, Plant2, Plant3, Plant4, Sphere, Cube, Capsule }; // Possible plant species

public class Collectible : MonoBehaviour {

	public Color color; 
	public Species species; 
	public bool selectable = true;
	public int type;

	public AudioClip success;
	public AudioClip fail;
	
	private Pattern pattern;
	private PatternLevelManager levelManager;

	void Start () {
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		levelManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
	}

	void OnMouseDown () {
		if (!selectable || pattern.display) { return; }
		if(pattern.patternCount != 0)
		{
			if (pattern.current.color == color && pattern.current.species == species) 
			{
				Destroy (gameObject);
				pattern.foundCollectible();
				AudioSource.PlayClipAtPoint(success, transform.position);
			}	 
			else 
			{
				levelManager.FailedPattern();
				AudioSource.PlayClipAtPoint(fail, transform.position);
			}
		}
	}

	// If collectible is off screen, delete it
	void OnBecameInvisible () {
		if (selectable) {
			//Destroy (gameObject);
		}
	}

}***/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Species { Plant1, Plant2, Plant3, Plant4, Sphere, Cube, Capsule }; // Possible plant species

public class Collectible : MonoBehaviour {
	
	public Color color; 
	public Species species; 
	public bool selectable = true;
	public int type;
	
	public AudioClip success;
	public AudioClip fail;
	
	private Pattern pattern;
	private PatternLevelManager levelManager;
	
	void Start () {
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		levelManager = GameObject.FindGameObjectWithTag ("PatternLevelManager").GetComponent<PatternLevelManager> ();
	}
	
	void OnMouseDown () {
		if (!selectable || pattern.display) { return; }
		if(pattern.patternCount != 0)
		{
			if (pattern.current.color == color && pattern.current.species == species) 
			{
				Destroy (gameObject);
				pattern.foundCollectible();
				AudioSource.PlayClipAtPoint(success, transform.position);
			}	 
			else 
			{
				levelManager.FailedPattern();
				AudioSource.PlayClipAtPoint(fail, transform.position);
			}
		}
	}
	
	// If collectible is off screen, delete it
	void OnBecameInvisible () {
		if (selectable) {
			//Destroy (gameObject);
		}
	}
	
}

