using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public int playerSpeed;
	public int tilt;
	public float fireRate = 0.5f;
	public float itemDuration = 1500;
	public GameObject shot;
	public GameObject shield;
	public Transform shotSpawnTransform;
	public Boundary boundary;
	private Rigidbody rigidbody ;
	private Quaternion calibrationQuaternion;
	private float nextFire = 0.0f;
	private bool isWeaponUpgraded = false;
	private bool isGotShield = false;
	private float weaponUpgradedDuration = 0;
	private float shieldDuration = 0;

	void Start ()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer) {
			this.CalibrateAccelerometer ();
		}
	}
		
	// Update is called once per frame
	void Update ()
	{
		this.SpawnShots (this.isWeaponUpgraded);
		this.DecreaseItemDuration ();
	}

	void FixedUpdate ()
	{
		rigidbody = this.GetComponent<Rigidbody> ();
		if (rigidbody != null) {
			Vector3 movement;
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXPlayer) {
				Vector3 acceleration = this.FixAcceleration (Input.acceleration);
				movement = new Vector3 (acceleration.x, 0, acceleration.y);
			} else {
				movement = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			}
			rigidbody.velocity = movement * this.playerSpeed;
			rigidbody.position = new Vector3 (
				Mathf.Clamp (rigidbody.position.x, this.boundary.minX, this.boundary.maxX),
				0,
				Mathf.Clamp (rigidbody.position.z, this.boundary.minZ, this.boundary.maxZ)
			);
			rigidbody.rotation = Quaternion.Euler (new Vector3 (0, 0, rigidbody.velocity.x * -this.tilt));
		}
		
		this.ActiveShield ();
	}

	public bool IsWeaponUpgraded ()
	{
		return this.isWeaponUpgraded;
	}

	public void SetWeaponUpgraded (bool isWeaponUpgraded)
	{
		this.weaponUpgradedDuration = itemDuration;
		this.isWeaponUpgraded = isWeaponUpgraded;
	}

	public bool IsGotShield ()
	{
		return this.isGotShield;
	}

	public void SetGotShield (bool isGotShield)
	{
		this.shieldDuration = itemDuration;
		this.isGotShield = isGotShield;
	}

	private void SpawnShots (bool isWeaponUpgraded)
	{
		if (Time.time > this.nextFire) {
			this.nextFire = Time.time + this.fireRate;
			Instantiate (this.shot, this.shotSpawnTransform.position, this.shotSpawnTransform.rotation);
			if (isWeaponUpgraded && this.weaponUpgradedDuration > 0) {
				Quaternion quaternion = Quaternion.AngleAxis (15f, Vector3.up);
				Instantiate (this.shot, this.shotSpawnTransform.position, quaternion);
				quaternion = Quaternion.AngleAxis (-15f, Vector3.up);
				Instantiate (this.shot, this.shotSpawnTransform.position, quaternion);
			}
			AudioSource audio = this.GetComponent<AudioSource> ();
			if (audio != null) {
				audio.Play ();
			}
		}
	}

	private void ActiveShield ()
	{
		shield.gameObject.SetActive (this.isGotShield);
//		shield.gameObject.SetActive (true);
	}

	private void DecreaseItemDuration ()
	{
		if (this.weaponUpgradedDuration > 0) {
			this.weaponUpgradedDuration--;
		} else {
			this.isWeaponUpgraded = false;
		}

		if (this.shieldDuration > 0) {
			this.shieldDuration--;
		} else {
			this.isGotShield = false;
		}
	}
	
	//Used to calibrate the Iput.acceleration input
	private void CalibrateAccelerometer ()
	{
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.5f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}
	
	//Get the 'calibrated' value from the Input
	private Vector3 FixAcceleration (Vector3 acceleration)
	{
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}
}