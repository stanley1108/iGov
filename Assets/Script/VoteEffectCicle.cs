using UnityEngine;
using System.Collections;

public class VoteEffectCicle : MonoBehaviour {
	[SerializeField]
	SheepBase m_ownerSheep = null;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject == gameObject)
			return;

		if(coll.gameObject == transform.parent.gameObject)
			return;
		if(coll.gameObject.name.Contains("Sheep") == false)
			return;

//		Debug.Log("effect other sheep");

		SheepBase sheep = coll.gameObject.GetComponent<SheepBase>() as SheepBase;
		if(sheep == null)
			return;

		m_ownerSheep.PusVoteEffect(sheep);
	}
}
