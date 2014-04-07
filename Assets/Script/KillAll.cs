using UnityEngine;
using System.Collections;

public class KillAll : SkillBase {
	[SerializeField]
	float m_totalEffectTime = 3f;
	
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
		return1 = 1f;
		return SheepBase.EffectType.Kill;
	}
	
	protected override void OnTriggerEnter2D(Collider2D coll) {
		if(m_currEffectTime >m_totalEffectTime *0.5f)
			return;
		
		base.OnTriggerEnter2D(coll);
	}
}
