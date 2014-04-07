using UnityEngine;
using System.Collections;

public class KillSheep : SkillBase {
	[SerializeField]
	float m_totalEffectTime = 1f;
	
	float m_currEffectTime = 0f;
	
	// Update is called once per frame
	void Update () {
		if(m_currEffectTime >= m_totalEffectTime)
		{

			m_currEffectTime = 0f;
			gameObject.SetActive(false);
			transform.localScale = Vector3.one;
			return;
		}
		m_currEffectTime += Time.deltaTime;

		transform.localScale = Vector3.one * (0.5f+m_currEffectTime/m_totalEffectTime);

		CircleCollider2D circle = (CircleCollider2D)collider2D;
		
		Collider2D[] collidedObjs = Physics2D.OverlapCircleAll(transform.position, circle.radius*2f);
		for(int i=0; i<collidedObjs.Length; i++)
		{
			SheepBase sheep = collidedObjs[i].gameObject.GetComponent<SheepBase>();
			if(sheep != null)
			{
				sheep.BecomeYellow(0.5f);
			}
		}
	}

	public void MoveTo(Vector3 newPos)
	{
		newPos.z = transform.position.z;

		transform.position = newPos;
		transform.localScale = Vector3.one;

		gameObject.SetActive(true);
	}

	override public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3)
	{
		return SheepBase.EffectType.Kill;
	}

	protected override void OnTriggerEnter2D(Collider2D coll) {
		if(m_currEffectTime >m_totalEffectTime *0.5f)
			return;

		base.OnTriggerEnter2D(coll);
	}
}
