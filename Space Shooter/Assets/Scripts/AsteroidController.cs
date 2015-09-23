using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

	public const int ASTEROID_SCORE = 1; 

	public GameObject explosion;	
	public GameObject playerExplosion;

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
			float tumble = Random.Range(1,20);
			rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "Bolt" || other.tag == "Shield") {
			Instantiate(this.explosion, this.transform.position, this.transform.rotation);
			if(other.tag == "Player"){
				Instantiate(this.playerExplosion, other.transform.position, other.transform.rotation);
				this.gameControllerScript.GameOver();
			}
			Destroy (other.gameObject);
			Destroy(this.gameObject);
			if(this.gameControllerScript != null){
				this.gameControllerScript.AddScore(ASTEROID_SCORE);
			}
		}
	}
}
