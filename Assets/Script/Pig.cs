using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pig : SkillBase {

	// Use this for initialization
	protected override void Start () {
		base.Start();	
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void MoveTo(Vector3 newPos)
	{
		newPos.z = transform.position.z;
		CircleCollider2D circle = (CircleCollider2D)collider2D;
		
		Collider2D[] collidedObjs = Physics2D.OverlapCircleAll(transform.position, circle.radius);
		
		for(int i=0; i<collidedObjs.Length; i++)
		{
			RemoveCollideObj(collidedObjs[i].gameObject);
		}
		
		
		transform.position = newPos;
	}



	override public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3)
	{
		return1 = 0.5f;
		return SheepBase.EffectType.SlowDown;
	}
}
