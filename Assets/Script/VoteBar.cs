using UnityEngine;
using System.Collections;

public class VoteBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnTriggerEnter2D(Collider2D coll) {
	//	Debug.Log("Vote bar collide");
		if(coll.gameObject.name.Contains("Sheep") == false)
			return;

		Level.Instance().AddSheepToAttacking(coll.gameObject);
		//Debug.Log("Get Back Sheep");

	}
}
