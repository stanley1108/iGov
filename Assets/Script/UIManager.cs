using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	static UIManager m_instance = null;
	static public UIManager Instance(){return m_instance;}

	[SerializeField]
	SpriteRenderer m_blackCover = null;

	public enum State
	{
		Gaming,
		TurnBlack,
		WaitToReplay
	}

	State m_currState = State.Gaming;
	public State CurrState
	{
		get{return m_currState;}
		set{m_currState = value;}
	}

	float m_currTime = 0f;
	// Use this for initialization
	void Start () {
		m_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_currState)
		{
		case State.Gaming:
			break;
		case State.TurnBlack:
			m_currTime += Time.deltaTime;
			if(m_currTime >= 1f)
			{
				m_currTime = 1f;
				m_currState = State.WaitToReplay;
			}
			Color newColor = Color.black;

			newColor.a = m_currTime;

			m_blackCover.color = newColor;
			break;
		case State.WaitToReplay:
#if !UNITY_EDITOR
			if(Input.touchCount == 0)
				return;
			Touch currTouch = Input.GetTouch(0);
			if(currTouch.phase != TouchPhase.Began)
				return;
#else
			if(Input.GetMouseButtonDown(0) == false)
				return;			
#endif

			m_currState = State.Gaming;
			Level.Instance().ResetAll();
			m_blackCover.color = Color.clear;
			m_currTime = 0f;
			break;
		}
	}
}
