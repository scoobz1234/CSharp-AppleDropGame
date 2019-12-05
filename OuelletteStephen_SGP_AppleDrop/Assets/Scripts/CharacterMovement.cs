using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {


	float Speed = 4f;
	public GameObject Player,ScorePopUp;
	public Transform PlayerPos;

	void Start (){



	}

	void Update () {
		
		//Gets the Mouse Position on screen
		Vector3 screenPos = Input.mousePosition;
		//Translates Mouse/Screen Position to a world point position
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (screenPos);
		//the following code clamps (locks) my X axis to the left most and right most positions of the screen
		mousePos.x = Mathf.Clamp (mousePos.x, -9.5f, 9.5f);
		//the following code clamps (locks) my Y axis to the bottom of the screen
		mousePos.y = Mathf.Clamp (mousePos.y, -4.53f, -4.53f);
		//the following code clamps (locks) my Z axis to 0 which is the same level the apples drop
		mousePos.z = Mathf.Clamp (mousePos.z, -2f, -2f);
		//makes the player follow the mouse position and multiplies its movement speed by time and the speed variable
		PlayerPos.position = Vector3.Lerp (PlayerPos.position, mousePos, Time.deltaTime * Speed);

	}
			
	void ScorePopUpText(string text){
		//this sets a temporary game object and instantiates it from prefab
		GameObject temp = Instantiate (ScorePopUp) as GameObject;
		//this sets a temporary transform for the object and gets the component
		RectTransform tempRect = temp.GetComponent <RectTransform> ();
		//sets parent of instantiated object to the canvas, so it shows up in the right place
		temp.transform.SetParent (transform.FindChild ("PlayerCanvas"));
		//stores local position/scale/rotation of the object.
		tempRect.transform.localPosition = ScorePopUp.transform.localPosition;
		tempRect.transform.localScale = ScorePopUp.transform.localScale;
		tempRect.transform.rotation = ScorePopUp.transform.localRotation;
		//gets the text component of the object
		temp.GetComponent<Text> ().text = text;
		//activates the animator trigger applescore
		temp.GetComponent<Animator> ().SetTrigger ("AppleScore");
		//destroys the object after 2 seoncds
		Destroy (temp, 2);
	}

	void OnTriggerStay (Collider other){
		if (other.gameObject.tag == "Apple"){
			//locates gamemaster script and score
			GameObject Score = GameObject.Find ("GameMaster");
			GameMasterScript GameMaster = Score.GetComponent<GameMasterScript> ();
			//puts the score into the text of the gameobject
			ScorePopUpText(GameMaster.GameScore.ToString());
		}
	}
}
