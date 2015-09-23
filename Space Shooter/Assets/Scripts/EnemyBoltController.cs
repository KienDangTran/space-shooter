using UnityEngine;
using System.Collections;

public class EnemyBoltController : MonoBehaviour {

	public GameObject playerExplosion;
	private GameController gameControllerScript; 

	// Use this for initialization
	void Start () {
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObj != null) {
			this.gameControllerScript = gameControllerObj.GetComponent<GameController> ();
			if (this.gameControllerScript == null) {
				Debug.LogError ("Cannot find 'GameController' Script!");
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Instantiate (this.playerExplosion, other.transform.position, other.transform.rotation);
			this.gameControllerScript.GameOver ();
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
		if (other.tag == "Shield") {
			Destroy (this.gameObject);
		}
	}
}
