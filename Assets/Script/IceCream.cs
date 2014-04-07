using UnityEngine;
using System.Collections;

public class IceCream : SkillBase {
	[SerializeField]
	float m_totalEffectTime = 10f;

	float m_currEffectTime = 0f;

	void OnDisable()
	{
		BoxCollider2D box = (BoxCollider2D)collider2D;
		Vector2 boxCenter = transform.position;
		Collider2D[] collidedObjs = Physics2D.OverlapAreaAll(boxCenter-box.size, boxCenter+box.size);
		for(int i=0; i<collidedObjs.Length; i++)
		{
			RemoveCollideObj(collidedObjs[i].gameObject);
		}
	}

	void Update()
	{
		if(m_currEffectTime >= m_totalEffectTime)
		{
			m_currEffectTime = 0f;
			gameObject.SetActive(false);
		}
		m_currEffectTime += Time.deltaTime;
	}

	override public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3)
	{
		return1 = 0.5f;
		return SheepBase.EffectType.SlowDown;
	}

	protected override void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject == gameObject)
			return;
		
		if(coll.gameObject.name.Contains("Sheep") == false)
			return;
		
		SheepBase sheep = coll.gameObject.GetComponent<SheepBase>() as SheepBase;
		if(sheep == null)
			return;
		
		sheep.AddEffectingSkill(this);
		
		if(m_collitionObj.Contains(coll.gameObject) == false)
			m_collitionObj.Add(coll.gameObject);

		sheep.BecomeWhite(0.1f);
	}
}
