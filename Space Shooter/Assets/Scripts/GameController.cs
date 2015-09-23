using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	
	public Vector3 spawnPosition;

	public GameObject hazard1;
	public GameObject hazard2;
	public GameObject hazard3;
	public int hazardCount;

	public GameObject enemy;
	public int enemyCount;

	public GameObject gunItem;
	public GameObject shieldItem;
	public float itemWait;
	public float maxItemWait;

	public Text scoreText;
	public Text waveText;
	public Text gameOverText;
	public Text resetText;

	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	private int level = 1;
	private int score = 0;
	private bool isGameOver = false; 	 	

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Input.gyro.enabled = true;
		this.gameOverText.text = "";
		this.resetText.text = "";
//		StartCoroutine(this.SpawnHazardWaves ());
//		StartCoroutine (this.SpawnEnemys ());
//		StartCoroutine (this.SpawnItem ());
	}

	void Update(){
		if (isGameOver) {
			if(Input.GetKeyDown(KeyCode.R) || Input.touchCount > 0){
				Application.LoadLevel(Application.loadedLevel);
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public int GetLevel(){
		return this.level;
	}

	public int GetScore(){
		return this.score;
	}

	public void AddScore(int newScore){
		this.score += newScore;
		this.UpdateScore ();
	}

	public void GameOver(){
		this.isGameOver = true;
		this.gameOverText.text = "GAME OVER!";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer) {
			this.resetText.text = "Touch Screen for Restart";
		} else {
			this.resetText.text = "Press 'R' for Restart";
		}
	}

	private IEnumerator SpawnHazardWaves(){
		yield return new WaitForSeconds (startWait);
		Vector3 hazardSpawnPos;
		int randomHazard = 1;
		GameObject hazard;
		while (!isGameOver) {			
			hazardCount = hazardCount * level > 100 ? 100 : hazardCount * level;
			for (int i = 0; i< hazardCount; i++) {
				randomHazard = Random.Range(1,4);
				hazardSpawnPos = new Vector3 (Random.Range (-spawnPosition.x, spawnPosition.x), spawnPosition.y, spawnPosition.z);
				switch (randomHazard){
				case 1: 
					hazard = hazard1;
					break;
				case 2: 
					hazard = hazard2;
					break;
				case 3: 
					hazard = hazard3;
					break;
				default:
					hazard = hazard1;
					break;
				}
				Instantiate (hazard, hazardSpawnPos, Quaternion.identity);
				yield return new WaitForSeconds (spawnWait);
			}

			this.waveText.text = "Wave: " + ++level;
			yield return new WaitForSeconds (waveWait);
		}
	}

	private IEnumerator SpawnEnemys(){
		yield return new WaitForSeconds (startWait);
		Vector3 enemySpawnPos;
		while (!isGameOver) {	
			for(int i = 1; i <= this.level * this.enemyCount; i++){
				enemySpawnPos = new Vector3 (Random.Range (-spawnPosition.x, spawnPosition.x), spawnPosition.y, spawnPosition.z);
				Instantiate (this.enemy, enemySpawnPos, Quaternion.Euler(new Vector3(0, 180, 0)));
				yield return new WaitForSeconds (spawnWait);
			}

			yield return new WaitForSeconds (waveWait);
		}
	}

	private IEnumerator SpawnItem(){
		yield return new WaitForSeconds (startWait);
		Vector3 itemSpawnPos;
		int randomItem = 1;
		while (!isGameOver) {	
			itemSpawnPos = new Vector3 (Random.Range (-spawnPosition.x, spawnPosition.x), spawnPosition.y, spawnPosition.z);
			randomItem = Random.Range(1,3);
			switch(randomItem){
			case 1:
				Instantiate (this.gunItem, itemSpawnPos, Quaternion.identity);
				break;
			case 2:				
				Instantiate (this.shieldItem, itemSpawnPos, Quaternion.identity);
				break;
			default:
				break;
			}
			yield return new WaitForSeconds (itemWait);
			if(this.itemWait < this.maxItemWait){
				this.itemWait += Time.deltaTime * 100;
			}
		}
	}

	private void UpdateScore(){
		if (this.scoreText != null) {
			this.scoreText.text = "Score: " + score;
		}
	}
}
