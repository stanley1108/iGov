using UnityEngine;
using System.Collections;

public class NoTalkBar : SkillBase {

	// Use this for initialization
	override protected void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveTo(Vector3 newPos)
	{
		newPos.z = transform.position.z;
		BoxCollider2D box = (BoxCollider2D)collider2D;
		Vector2 boxCenter = transform.position;
		Collider2D[] collidedObjs = Physics2D.OverlapAreaAll(boxCenter-box.size, boxCenter+box.size);
		for(int i=0; i<collidedObjs.Length; i++)
		{
			RemoveCollideObj(collidedObjs[i].gameObject);
		}
		
		
		transform.position = newPos;
	}
	
	
	
	override public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3)
	{
		return3 = true;
		return SheepBase.EffectType.Frezze;
	}
}
