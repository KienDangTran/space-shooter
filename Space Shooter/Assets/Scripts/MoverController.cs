using UnityEngine;
using System.Collections;

public class MoverController : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;

	private GameController gameControllerScript; 

	// Use this for initialization
	void Start () {
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObj != null) {
			this.gameControllerScript = gameControllerObj.GetComponent<GameController>();
			if(this.gameControllerScript == null){
				Debug.LogError("Cannot find 'GameController' Script!");
			}
		}

		Rigidbody rigidbody = this.GetComponent<Rigidbody> ();
		if (rigidbody != null) {
			int level = this.gameControllerScript != null ? this.gameControllerScript.GetLevel() : 1;
			if(this.gameObject.tag == "Asteroid"){
				float asteroidSpeed = Random.Range(this.minSpeed, this.maxSpeed + level);
				rigidbody.velocity = this.transform.forward * -asteroidSpeed;
			} else {
				rigidbody.velocity = this.transform.forward * this.minSpeed;
			}
		}
	}
}
