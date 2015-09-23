using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

	public const int ENEMY_SCORE = 2;
	public int enemySpeed;
	public int enemyTitl;
	public float fireRate = 0.5f;
	public GameObject shot;
	public GameObject explosion;
	public GameObject playerExplosion;
	public GameObject enemyShield;
	public Transform shotSpawnTransform;
	public Boundary boundary;
	private float nextFire;
	private float randomX;
	private GameController gameControllerScript;
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start ()
	{
		while (randomX == 0) {
			randomX = Random.Range (-0.3f, 0.3f);
		}
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObj != null) {
			this.gameControllerScript = gameControllerObj.GetComponent<GameController> ();
			if (this.gameControllerScript == null) {
				Debug.LogError ("Cannot find 'GameController' Script!");
			}
		}

		this.rigidbody = this.GetComponent<Rigidbody> ();
		if (this.rigidbody != null) {
			Vector3 movement = new Vector3 (randomX, 0, 0.5f);
			this.rigidbody.velocity = -movement * this.enemySpeed;
			
			if (this.gameObject.tag == "Boss") {
				this.rigidbody.position = new Vector3 (
					Mathf.Clamp (rigidbody.position.x, this.boundary.minX, this.boundary.maxX),
					0,
					Mathf.Clamp (rigidbody.position.z, this.boundary.minZ, this.boundary.maxZ)
				);
			}
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > this.nextFire) {
			this.nextFire = Time.time + this.fireRate;
			Instantiate (this.shot, this.shotSpawnTransform.position, this.shotSpawnTransform.rotation);
			AudioSource audio = this.GetComponent<AudioSource> ();
			if (audio != null) {
				audio.Play ();
			}
			if (this.gameObject.tag == "Boss") {
				Quaternion quaternion = Quaternion.AngleAxis (30f, Vector3.down);
				Instantiate (this.shot, this.shotSpawnTransform.position, quaternion);
				quaternion = Quaternion.AngleAxis (-30f, Vector3.down);
				Instantiate (this.shot, this.shotSpawnTransform.position, quaternion);
			}
		}

		if (this.gameObject.tag == "Boss") {
			if (this.transform.position.x >= this.boundary.maxX || this.transform.position.x <= this.boundary.minX) {
				this.rigidbody.velocity = - rigidbody.velocity;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" || other.tag == "Bolt" || other.tag == "Shield") {
			Instantiate (this.explosion, this.transform.position, this.transform.rotation);
			if (other.tag == "Player") {
				Instantiate (this.playerExplosion, other.transform.position, other.transform.rotation);
				this.gameControllerScript.GameOver ();
			}
			if (other.tag != "Shield") {
				Destroy (other.gameObject);
				Destroy (this.gameObject);
				if (this.gameControllerScript != null) {
					this.gameControllerScript.AddScore (ENEMY_SCORE);
				}
			}
		}
	}
}
