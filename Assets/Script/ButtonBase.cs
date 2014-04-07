using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {
	[SerializeField]
	float m_CDtime = 0f;
	float m_currTime = 0f;

	float m_scaleTime = 0f;

	bool m_isPressable = false;

	public bool IsPressable()
	{
		return m_isPressable;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(m_currTime < m_CDtime)
		{
			m_currTime += Time.deltaTime;
		}

		if(m_currTime >= m_CDtime)
		{
			m_scaleTime += Time.deltaTime;

			float scale = 1f+0.1f * Mathf.Sin(m_scaleTime * Mathf.PI *2f);
			transform.localScale = Vector3.one * scale;
			m_isPressable = true;
		}

	}

	public void StopUsableNotice()
	{
		transform.localScale = Vector3.one;
		m_currTime = 0;
		m_isPressable = false;
	}


}
