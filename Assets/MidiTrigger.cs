using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTrigger : MonoBehaviour
{
    public PianoKeyController PianoKeyCtrller;
    bool isMove = false;
    [Range(0.2f,10)]
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            isMove = true;
        }
        if (isMove)
        {
            transform.position += new Vector3(1, -1, 0) * Time.deltaTime * speed;
        }   
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("OnCollisionEnter"+collision.gameObject.name);
    //    MidiNote curNote = collision.gameObject.GetComponent<NoteBoxModel>().curNote;
    //    PianoKeyCtrller.PianoNotes[curNote.NoteName].Play
    //        (Color.white, curNote.Velocity, curNote.Length, 1);
    //}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:"+other.gameObject.name);
        MidiNote curNote = other.gameObject.GetComponent<NoteBoxModel>().curNote;
        PianoKeyCtrller.PianoNotes[curNote.NoteName].Play
            (Color.white, curNote.Velocity, curNote.Length, 1);
    }
}
