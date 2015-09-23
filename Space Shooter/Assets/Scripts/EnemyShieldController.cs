using UnityEngine;
using System.Collections;

public class EnemyShieldController : MonoBehaviour
{

	public int shieldStrength = 2;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Bolt") {
			if (this.shieldStrength == 0) {
				Destroy (this.gameObject);
			} else {
				this.shieldStrength--;
			}
		} else if (other.gameObject.tag == "Player") {
		}
	}
}
