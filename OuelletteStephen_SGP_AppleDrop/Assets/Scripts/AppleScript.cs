using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour {

	float DeSpawn = 3f, SpinVelocity = 50f;
	Vector3 Rotation, Torque;
	public AudioClip Thud;
	public Rigidbody rb;



	void Start(){
		//sets rotation to randomize
		Rotation = Random.onUnitSphere;
		// gets the rigid body of the apple
		rb = GetComponent<Rigidbody> ();
	}


	void FixedUpdate () {

		//Slowly rotates the Apples when they drop on a random Axis.
		transform.Rotate (Rotation, SpinVelocity * Time.smoothDeltaTime);
		//this adds a random force to the object just to make it harder :-)
		rb.AddForce (transform.right * Random.Range (-.005, -.001));
		// if the timer is less than 1 it destroys the object
		if (DeSpawn < 1f) {

			Destroy (gameObject);
			DeSpawn = 3f;
			SubtractScore ();
		}
		// if the timer is greater than 1 it subtracts 1 per tick (frame)
		else {

			DeSpawn = DeSpawn - Time.deltaTime;

		}
	}
	// destroys object if it collides with the player
	void OnTriggerStay (Collider other){

		if(other.gameObject.tag == "Player"){
			//plays sound when apple hits basket
			AudioSource.PlayClipAtPoint(Thud, new Vector3 (0,-6,0));
			Destroy (gameObject);
			//locates the game score from the gamemaster script
			GameObject PlayerScore = GameObject.Find ("GameMaster");
			GameMasterScript GameMaster = PlayerScore.GetComponent<GameMasterScript> ();
			//increments score by 1
			GameMaster.GameScore++;

		}
	}

	public void SubtractScore (){
		//locates objects from gamemaster script
		GameObject Apple3 = GameObject.Find ("GameMaster");
		GameMasterScript GameMaster = Apple3.GetComponent<GameMasterScript> ();


		//if there is 3 lives make it 2
		if (GameMaster.TillDeath == 3){
			GameMaster.TillDeath--;
			GameMaster.Apple3.SetActive (false);
		}
		//if there are 2 lives make it 1
		else if (GameMaster.TillDeath == 2){
			GameMaster.TillDeath--;
			GameMaster.Apple2.SetActive (false);
		}
		else {
			//if there are 1 lives make it 0
			GameMaster.TillDeath--;
			GameMaster.Apple1.SetActive (false);
		}
	}

}
