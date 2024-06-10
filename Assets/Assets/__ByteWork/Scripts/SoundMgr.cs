using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour 
{
	public static SoundMgr Instance;
    public List<AudioClip> KeySounds = new List<AudioClip>();
     AudioSource soudAS;
    AudioSource chrosAs;
	void Awake()
	{
		Instance=this;
        soudAS=gameObject.AddComponent<AudioSource>();
        chrosAs= gameObject.AddComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
    public bool IsMouseDown = false;
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            IsMouseDown = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    bool soundPlay = false;
    public void PlaySound(AudioClip _audioClip)
    {
        if(soundPlay)
        {
            if (soudAS.isPlaying)
                soudAS.Stop();
            soudAS.PlayOneShot(_audioClip);
            Debug.Log("AAA");
        }
        else
        {
            if (chrosAs.isPlaying)
                chrosAs.Stop();
            chrosAs.PlayOneShot(_audioClip);
            Debug.Log("BBB");
        }
        soundPlay = !soundPlay;
    }
}
