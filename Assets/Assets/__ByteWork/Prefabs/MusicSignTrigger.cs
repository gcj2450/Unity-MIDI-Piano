using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSignTrigger : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        ChangeUIEventArgs args = new ChangeUIEventArgs(
        EventNames.OnMusicSignCheck.ToString(), collider.GetComponent<MusicSign>().KeyId);
        App.Instance.EventManager.SendEvent(args);
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        ChangeUIEventArgs args = new ChangeUIEventArgs(
        EventNames.OnMusicSignExit.ToString(), collider.GetComponent<MusicSign>().KeyId);
        App.Instance.EventManager.SendEvent(args);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.other.gameObject.name);
    }
}
