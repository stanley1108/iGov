using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SheepBase : MonoBehaviour {
	[SerializeField]
	float m_voteChoice = 0;
	public float VoteChoice
	{
		get{return m_voteChoice;}
	}

	[SerializeField]
	float m_maxMoveSpeed = 100;

	float m_effectAcceptRatio = 0.3f;

	SpriteRenderer m_renderer = null;

	bool m_isAttacking = false;

	float m_lifeTime = 30f;

	List<SkillBase>	m_effectingSkill = new List<SkillBase>();

	public enum EffectType
	{
		None,
		SlowDown,
		Attact,
		Frezze,
		Kill

	}
//==================================================

	// Use this for initialization
	void Start () {
		m_renderer = GetComponent<SpriteRenderer>();

		OnGenerate();
	}
	
	public void OnGenerate()
	{
		//random set vote choice
		m_voteChoice = 0.2f * (float)Random.Range(-5, 5);
		m_voteChoice += Level.Instance().DefaultVote;

		m_voteChoice = Mathf.Clamp(m_voteChoice, -1f, 1f);
		
		float isDontCare = Random.Range(0f, 1f);
		if(isDontCare < Level.Instance().IdontCare)
			m_voteChoice = 0;

		m_lifeTime = 30f;

		m_isAttacking = false;

		m_effectingSkill.Clear();
	}
	
	// Update is called once per frame
	void Update () {
		if(UIManager.Instance().CurrState != UIManager.State.Gaming)
			return;

		m_lifeTime -= Time.deltaTime;

		if(m_lifeTime <= 0f)
		{
			Level.Instance().GetBackSheep(gameObject);
			return;
		}

		//update color by vote choice
		Color newColor = Color.white;
		if(m_voteChoice < 0f)
		{
			newColor.r = 0.5f*(1f+m_voteChoice);
			newColor.g = 0.5f*(1f+m_voteChoice);
		}
		else if(m_voteChoice >0f)
		{
			newColor.b = 0.5f*(1f-m_voteChoice);
		}
		m_renderer.color = newColor;


		if(m_voteChoice == 0)
		   return;
//		if(m_isAttacking == true)
//		   return;

		float speed = m_maxMoveSpeed * Mathf.Abs(m_voteChoice);
		if(m_isAttacking == true)
			speed = 0f;
		
		Vector3 finalResult = CalEffect(new Vector3(0,-speed * Time.deltaTime, 0));
		transform.Translate(finalResult);

	}


	public void PusVoteEffect(SheepBase otherSheep)
	{
		otherSheep.AcccptVoteEffect(m_voteChoice);
	}

	public void AcccptVoteEffect(float effectValue)
	{
		if(m_isAttacking)
			return;

		m_voteChoice += m_effectAcceptRatio * effectValue;
		m_voteChoice = Mathf.Clamp(m_voteChoice, -1f, 1f);
	}

	public void StartAttack()
	{
		m_isAttacking = true;
		m_lifeTime+=30f;
	}

	public void AddEffectingSkill(SkillBase skill)
	{
		if(m_effectingSkill.Contains(skill) == true)
			return;

		m_effectingSkill.Add(skill);

	}

	public void RemoveEffectingSkill(SkillBase skill)
	{
		if(m_effectingSkill.Contains(skill) == false)
			return;
		
		m_effectingSkill.Remove(skill);
	}

	Vector3 CalEffect(Vector3 currTranslate)
	{
		float slowRatio = 1f;
		Vector3 attactPos = Vector3.zero;
		bool frezze = false;

		for(int i=0; i<m_effectingSkill.Count; i++)
		{
			EffectType type = m_effectingSkill[i].CalEffect(ref slowRatio, ref attactPos, ref frezze);

			if(type == EffectType.Frezze)
			{
				if(frezze == true)
					currTranslate = Vector3.zero;
			}

			if(type == EffectType.SlowDown)
			{
				currTranslate *= slowRatio;
			}

			if(type == EffectType.Attact)
			{
				if(m_isAttacking)
					continue;

				Vector3 attactVector = attactPos - transform.position;

				Vector3 realattactVector = attactVector.normalized * slowRatio * Time.deltaTime;
				if(attactVector.sqrMagnitude > realattactVector.sqrMagnitude)
					currTranslate += realattactVector;
				else
					currTranslate += attactVector;
			}

			if(type == EffectType.Kill)
			{
				Level.Instance().GetBackSheep(gameObject);
			}
		}


		return currTranslate;
	}

	public void BecomeWhite(float value)
	{
		if(m_voteChoice >0f)
		{
			m_voteChoice -= value;
			if(m_voteChoice <0f)
				m_voteChoice = 0f;
		}
		else if(m_voteChoice <0f)
		{
			m_voteChoice += value;
			if(m_voteChoice > 0f)
				m_voteChoice = 0f;
		}
	}

	public void BecomeYellow(float value)
	{
		m_voteChoice += value;
		m_voteChoice = Mathf.Clamp(m_voteChoice, 0f, 1f);
	}
}
