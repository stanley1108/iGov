using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
	static Level m_instance = null;
	static public Level Instance(){return m_instance;}

	[SerializeField]
	GameObject m_sheepPrefab = null;

	[SerializeField]
	List<GameObject> m_onScene = new List<GameObject>();
	[SerializeField]
	List<GameObject> m_useAble = new List<GameObject>();
	[SerializeField]
	List<SheepBase> m_attacking = new List<SheepBase>();
	int m_maxSheepNum = 200;

	
	[SerializeField]
	Transform m_hpBar = null;
	float m_maxHP = 1000;
	[SerializeField]
	float m_currHP = 1000;
	Vector3 m_barDefaultScale = new Vector3(7.5f, 1, 1);

	[SerializeField]
	float m_idontCare = 0.7f;		//the % which dont care
	public float IdontCare
	{
		get{return m_idontCare;}
		set{
			m_idontCare = Mathf.Clamp(value, 0f, 1f);
		}
	}

	float m_defaltVote = 0f;
	public float DefaultVote
	{
		get{return m_defaltVote;}
		set{m_defaltVote = Mathf.Clamp(value, -1f, 1f);}
	}

	[SerializeField]
	Rect m_pigForm = new Rect(-320, -72, 640, 640);

	[SerializeField]
	float m_levelTime = 0;
	[SerializeField]
	GUIText m_timeText;

	float m_generatePeriod = 0.5f;
	float m_tempTime = 0;



//======================
	void Awake()
	{
		m_instance = this;
	}


	// Use this for initialization
	void Start () {
		GameObject sheepNode = new GameObject("Sheeps");

		for(int i=0; i<m_maxSheepNum; i++)
		{
			GameObject newSheep = Instantiate(m_sheepPrefab) as GameObject;
			newSheep.transform.parent = sheepNode.transform;


			m_useAble.Add(newSheep);
			newSheep.gameObject.SetActive(false);
		}

		m_barDefaultScale = m_hpBar.localScale;
	}

	public void ResetAll()
	{
		m_levelTime = 0f;

		for(int i=0; i<m_onScene.Count; i++)
		{
			m_useAble.Add(m_onScene[i]);
			m_onScene[i].SetActive(false);
		}
		m_onScene.Clear();

		for(int i=0; i<m_attacking.Count; i++)
		{
			m_useAble.Add(m_attacking[i].gameObject);
			m_attacking[i].gameObject.SetActive(false);
		}
		m_attacking.Clear();

		m_currHP = m_maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if(UIManager.Instance().CurrState != UIManager.State.Gaming)
			return;

		if(m_currHP >0)
		{
			m_levelTime+= Time.deltaTime;
			int timeToShow = (int)m_levelTime;
			m_timeText.text = timeToShow +"小時";
		}

		m_tempTime += Time.deltaTime;
		if(m_tempTime >= m_generatePeriod)
		{
			m_tempTime -= m_generatePeriod;
			GeneratePigRandom();
		
			CalHP();
		}

		if(m_currHP <=0)
		{
			//show end game
			UIManager.Instance().CurrState = UIManager.State.TurnBlack;

			return;
		}

		//update HP bar
		m_hpBar.localScale = Vector3.Scale( m_barDefaultScale, new Vector3(m_currHP/m_maxHP, 1, 1));

	}

	void GeneratePigRandom()
	{
		if(m_useAble.Count <=0)
			return;

		GameObject newSheepOnScene = m_useAble[0];
		m_onScene.Add(newSheepOnScene);
		m_useAble.Remove(newSheepOnScene);

		float newX = Random.Range(m_pigForm.xMin, m_pigForm.xMax);
		float newY = Random.Range(m_pigForm.yMin, m_pigForm.yMax);
		newSheepOnScene.transform.position = new Vector3(newX, newY, 0);

		newSheepOnScene.gameObject.SetActive(true);

		SheepBase sheep = newSheepOnScene.GetComponent<SheepBase>()as SheepBase;
		sheep.OnGenerate();
	}

	public void AddSheepToAttacking(GameObject obj)
	{
		SheepBase sheep = obj.GetComponent<SheepBase>() as SheepBase;
		if(sheep == null)
			return;
		
		sheep.StartAttack();
		m_attacking.Add (sheep);
		m_onScene.Remove(obj);
	}

	public void GetBackSheep(GameObject obj)
	{
		m_useAble.Add(obj);
		if(m_onScene.Contains(obj))
			m_onScene.Remove(obj);
		else
		{
			SheepBase sheep = obj.GetComponent<SheepBase>() as SheepBase;
			if(sheep != null)
			{
				if(m_attacking.Contains(sheep))
					m_attacking.Remove(sheep);
			}
		}
		obj.SetActive(false);
	}

	[SerializeField]
	float deltaHP = 0;

	void CalHP()
	{
		if(SkillManager.Instance().IsUsingtank() == true)
			return;

		deltaHP = 0;
		for(int i=0; i< m_attacking.Count; i++)
		{
			if(m_attacking[i].VoteChoice > 0)
				deltaHP += 1;
		}

		if(deltaHP <= 0)
			return;

		m_currHP -= deltaHP * 1f;
	}
}
