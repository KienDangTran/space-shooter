using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			this.playerController = player.GetComponent<PlayerController>();
			if(this.playerController == null){
				Debug.LogError("Cannot find 'PlayerController' Script!");
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(this.gameObject.tag == "GunItem"){
				this.playerController.SetWeaponUpgraded(true);
			}
			if(this.gameObject.tag == "ShieldItem"){
				this.playerController.SetGotShield(true);
			}
			Destroy(this.gameObject);
		}
	}
}
