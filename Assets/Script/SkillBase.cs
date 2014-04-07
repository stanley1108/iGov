using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SkillBase : MonoBehaviour {
	protected List<GameObject> m_collitionObj = null;
	// Use this for initialization
	protected virtual void Start () {
		m_collitionObj = new List<GameObject>();
	}
	


	protected virtual void OnTriggerEnter2D(Collider2D coll) {
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
	}
	
	protected void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject == gameObject)
			return;
		
		RemoveCollideObj(coll.gameObject);
	}
	
	
	protected void RemoveCollideObj(GameObject obj)
	{
		if(obj == null)
			return;
		if(obj.name.Contains("Sheep") == false)
			return;
		
		SheepBase sheep = obj.GetComponent<SheepBase>() as SheepBase;
		if(sheep == null)
			return;
		
		sheep.RemoveEffectingSkill(this);
		
		if(m_collitionObj.Contains(obj))
			m_collitionObj.Remove(obj);
	}

	abstract public SheepBase.EffectType CalEffect(ref float return1, ref Vector3 return2, ref bool return3);

}
