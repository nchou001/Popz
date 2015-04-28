using UnityEngine;
using System.Collections;

public class PatternLevelManager : MonoBehaviour {

	public int numLives = 3; // Number of mistakes player can make before pattern length is decremented
	public int numHints = 3;
	public int numRounds = 5; // Number of rounds player must complete before pattern length is incremented
	public int patternLength = 2; // Number of collectibles in the pattern
	public bool collectReverse;
	private int livesPerCollection;
	private int roundsPerCollection;

	public int maxPatternLength;
	private Pattern pattern;
	private Player player;
	private bool displayCollectionStatus = false;
	public GameObject displayCollectionGUI;

	private GameObject nextTerrainChunk;

	private GameObject tempClosest = null;
	private bool justOnceReset = false;

	// Use this for initialization
	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		maxPatternLength = grid.numCellsX - 1;
		pattern = GameObject.FindGameObjectWithTag ("Pattern").GetComponent<Pattern> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		collectReverse = false;
		roundsPerCollection = numRounds;
		livesPerCollection = numLives;
		pattern.GeneratePattern(patternLength);
		//displayCollectionGUI.GetComponent<Animation>().Play();

	}

	IEnumerator RevealCollectionStatus(float displayTime) {
		displayCollectionStatus = true;
		//displayCollectionGUI.GetComponentInChildren<Animation>().Play("MessageFlash");
		yield return new WaitForSeconds(displayTime);  
		displayCollectionStatus = false;

	}
	// Update is called once per frame
	void Update () {
		//TEsting width
		Vector3 topRight = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
		float width = topRight.x - bottomLeft.x;
		Debug.Log ("WIDTH: "+width);

		//Task Display
		if(displayCollectionStatus)
		{
			displayCollectionGUI.SetActive(true);
		}
		else
		{
			displayCollectionGUI.SetActive(false);
		}
		//Get Next Cloud Chunk.
		GameObject[] cloudChunks = GameObject.FindGameObjectsWithTag("TerrainChunk");
		float temp_dif;

		//Loop Through the cloudChunks
		for(int i = 0; i <cloudChunks.Length; i++)
		{
			temp_dif = cloudChunks[i].transform.position.x - player.transform.position.x;
			if(temp_dif > 0)
			{
				justOnceReset = false;
				tempClosest= cloudChunks[i];
			}
		}

		if(tempClosest != null)
		{
			Debug.Log ("PlayerPos: "+player.transform.position+ " Closest: "+tempClosest.transform.position);
			if(player.transform.position.x >= tempClosest.transform.position.x )
			{
				Debug.Log ("Whoaa!!!");
				if(!justOnceReset)
				{
					int rand = Random.Range (0, 2);
					
					if(rand == 0)
					{
						collectReverse = true;
						
						StartCoroutine("RevealCollectionStatus",2f);
					}
					else
					{
						collectReverse = false;
					}
					pattern.GeneratePattern(patternLength);
					numLives = livesPerCollection;

					justOnceReset = true;
				}

			}
		}



		if (pattern.patternCount == 0) {
			if (numRounds > 1) {
				numRounds--;
			}
			else {
				numLives = livesPerCollection;
				numRounds = roundsPerCollection;
				if (patternLength < maxPatternLength) {
					patternLength++;
				}
			}
			//Randomly Choose to introduce Reverse
			int rand = Random.Range (0, 2);

			if(rand == 0)
			{
				collectReverse = true;
				//Play Animation
				StartCoroutine("RevealCollectionStatus",2f);


			}
			else
			{
				collectReverse = false;
			}


			pattern.GeneratePattern(patternLength);


		}

		// Skips and generates new pattern is "s" is pressed
		if (Input.GetKeyDown ("s")) {
			int rand = Random.Range (0, 2);
			
			if(rand == 0)
			{
				collectReverse = true;

				StartCoroutine("RevealCollectionStatus",2f);
			}
			else
			{
				collectReverse = false;
			}
			pattern.GeneratePattern(patternLength);
			numLives = livesPerCollection;
		}
		//Press h
		if (Input.GetKeyDown ("h") && numHints > 0 && !pattern.display) {
			pattern.RevealPattern();
			numHints--;
		}
	}

	public void FailedPattern () {
		if (numLives > 1) {
			numLives--;
			pattern.RevealPattern ();
		}
		else {
			numRounds = roundsPerCollection;
			numLives = livesPerCollection;
			if (patternLength > 2) {
				patternLength--;
			}
			int rand = Random.Range (0, 2);
			
			if(rand == 0)
			{
				collectReverse = true;
				
				StartCoroutine("RevealCollectionStatus",2f);
			}
			else
			{
				collectReverse = false;
			}
			pattern.GeneratePattern(patternLength);
		}
	}
}
