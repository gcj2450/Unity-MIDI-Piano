using UnityEngine;
using System.Collections;

public class ObjectUtil  {

	
	public static void UpdateActive2Object(GameObject obj , bool active)
	{
		if (obj != null && obj.activeSelf != active)
		{
			obj.SetActive(active);
		}
	}

	public static GameObject GetSceneObjectByName(string objectName)
	{
		GameObject obj = GameObject.Find (objectName);
		if ( obj == null )
		{
			obj = new GameObject(objectName);
		}
		return obj;
	}

	public static T GetSceneScrpitByObject<T>(GameObject obj) where T : Component
	{
		if(obj.GetComponent<T>())
		{
			return obj.GetComponent<T>();
		}
		T t = obj.AddComponent<T> ();
		return t;
	}
}
