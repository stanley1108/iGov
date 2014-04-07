using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {
	static SkillManager m_instance = null;
	static public SkillManager Instance(){return m_instance;}

	public enum SkillType
	{
		IceCream,
		KillSheep,
		MovePig,
		NoTalk,
		Tank,
		KillAll
	}

	[SerializeField]
	GameObject m_PlayArea = null;
	BoxCollider2D m_playAreaCollider = null;
//items
	[SerializeField]
	Pig m_pig = null;
	CircleCollider2D m_pigCollider = null;

	[SerializeField]
	NoTalkBar m_noTalkBar = null;
	BoxCollider2D m_notalkBarCollider = null;

	[SerializeField]
	IceCream  m_iceCream = null;

	[SerializeField]
	KillSheep m_killSheep = null;

	[SerializeField]
	Tank m_tank = null;

	[SerializeField]
	KillAll m_KillAll = null;

	SkillType m_currSkillType;

	[SerializeField]
	GameObject[] m_buttons;
	List<ButtonBase> m_buttonScripts = new List<ButtonBase>();
	List<BoxCollider2D> m_buttonBoxColliders = new List<BoxCollider2D>();

	[SerializeField]
	GameObject m_selectingObj;

	public bool IsUsingtank()
	{
		return m_tank.gameObject.activeSelf;
	}

	//=====================================================

	// Use this for initialization
	void Start () {
		m_instance = this;

		m_playAreaCollider = (BoxCollider2D)m_PlayArea.collider2D;
		m_pigCollider = (CircleCollider2D)m_pig.gameObject.collider2D;
		m_notalkBarCollider = (BoxCollider2D)m_noTalkBar.gameObject.collider2D;

		m_currSkillType = SkillType.MovePig;
		OnSelectButton((int)SkillType.MovePig);

		for(int i=0; i<m_buttons.Length; i++)
		{
			if(m_buttons[i] == null)
				continue;
			m_buttonBoxColliders.Add((BoxCollider2D)m_buttons[i].collider2D);

			m_buttonScripts.Add(m_buttons[i].GetComponent<ButtonBase>());
		}


	}
	
	// Update is called once per frame
	void Update () {
		if(UIManager.Instance().CurrState != UIManager.State.Gaming)
			return;

		Vector3 newPos = Vector3.zero;
#if !UNITY_EDITOR
		if(Input.touchCount == 0)
			return;
		Touch currTouch = Input.GetTouch(0);
		if(currTouch.phase != TouchPhase.Began)
			return;
		
		newPos = Camera.main.ScreenToWorldPoint(currTouch.position);

#else
		if(Input.GetMouseButtonDown(0) == false)
			return;
		
		newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

#endif

		//check buttons
		for(int i=0; i<m_buttonBoxColliders.Count; i++)
		{
			if(m_buttonBoxColliders[i] == null)
				continue;

			if(m_buttonBoxColliders[i].OverlapPoint(newPos) == true)
			{
				if(m_buttonScripts[i] != null)
				{
					if(m_buttonScripts[i].IsPressable())
					{
						OnSelectButton(i);
						m_currSkillType = (SkillType)i;
					}
				}
				else
				{
					OnSelectButton(i);
					m_currSkillType = (SkillType)i;
				}
			}
		}

		//check pig and notalkBar
		if(m_currSkillType != SkillType.KillSheep)
		{
			if(m_pigCollider.OverlapPoint(newPos) == true)
			{
				OnSelectButton(2);
				m_currSkillType = SkillType.MovePig;
			}

			if(m_notalkBarCollider.OverlapPoint(newPos) == true)
			{
				OnSelectButton(3);
				m_currSkillType = SkillType.NoTalk;
			}
		}

		//check play area
		if(m_currSkillType == SkillType.IceCream)
		{
			if(m_iceCream.gameObject.activeSelf == false)
			{
				m_iceCream.gameObject.SetActive(true);
				m_currSkillType = SkillType.MovePig;
				OnSelectButton((int)SkillType.MovePig);
				m_buttonScripts[(int)SkillType.IceCream].StopUsableNotice();
			}
		}
		else if(m_currSkillType == SkillType.KillAll)
		{
			if(m_KillAll.gameObject.activeSelf == false)
			{
				m_KillAll.gameObject.SetActive(true);
				m_currSkillType = SkillType.MovePig;
				OnSelectButton((int)SkillType.MovePig);
				m_buttonScripts[(int)SkillType.KillAll].StopUsableNotice();

				Level.Instance().DefaultVote = Level.Instance().DefaultVote + 0.1f;

				Level.Instance().IdontCare = Level.Instance().IdontCare -0.1f;
			}
		}
		else if(m_currSkillType == SkillType.KillSheep)
		{

			UpdateKillSheep(newPos);
		}
		else if(m_currSkillType == SkillType.MovePig)
		{
			UpdateMovePig(newPos);
		}
		else if(m_currSkillType == SkillType.NoTalk)
		{
			UpdateNoTalkBar(newPos);
		}
		else if(m_currSkillType == SkillType.Tank)
		{
			UpdateTank(newPos);
		}
	}

	void UpdateMovePig(Vector3 newPos)
	{
		if(m_playAreaCollider.OverlapPoint(newPos) == true)
		{
			m_pig.MoveTo(newPos);
		}
	}

	void UpdateNoTalkBar(Vector3 newPos)
	{
		if(m_playAreaCollider.OverlapPoint(newPos) == true)
		{
			m_noTalkBar.MoveTo(newPos);
		}
	}

	void UpdateKillSheep(Vector3 newPos)
	{
		if(m_playAreaCollider.OverlapPoint(newPos) == true)
		{
			m_killSheep.MoveTo(newPos);


			m_currSkillType = SkillType.MovePig;
			OnSelectButton((int)SkillType.MovePig);
			m_buttonScripts[(int)SkillType.KillSheep].StopUsableNotice();
		}
	}

	void UpdateTank(Vector3 newPos)
	{
		if(m_playAreaCollider.OverlapPoint(newPos) == true)
		{
			m_tank.MoveTo(newPos);
			
			
			m_currSkillType = SkillType.MovePig;
			OnSelectButton((int)SkillType.MovePig);
			m_buttonScripts[(int)SkillType.Tank].StopUsableNotice();
		}
	}

	void OnSelectButton(int index)
	{
		Transform buttonTransform = m_buttons[index].transform;

		m_selectingObj.transform.position = buttonTransform.position;
	}






}
