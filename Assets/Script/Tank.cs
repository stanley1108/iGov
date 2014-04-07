using UnityEngine;
using System.Collections;

public class Tank : SkillBase {
	[SerializeField]
	float m_speed = 100;

	[SerializeField]
	float m_totalEffectTime = 10f;
	
	float m_currEffectTime = 0f;

	void OnDisable()
	{
		CircleCollider2D circle = (CircleCollider2D)collider2D;
		
		Collider2D[] collidedObjs = Physics2D.OverlapCircleAll(transform.position, circle.radius);
		
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

	public void MoveTo(Vector3 newPos)
	{
		newPos.z = transform.position.z;

		transform.position = newPos;

		gameObject.SetActive(true);
	}

	override public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3)
	{
		return1 = m_speed;
		return2 = transform.position;
		return SheepBase.EffectType.Attact;
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
		
		sheep.BecomeYellow(0.05f);
	}
}
