using System;
using UnityEngine;

public class NoteBoxModel : MonoBehaviour
{

    private Transform t;
    private MeshRenderer r;

    private float speed;
    private float halfLength;
    private bool hasHit = false;
    private float hitOffset = 0.5f;
    public MidiNote curNote;
    double tempo = 0;

    public void SetNoteNumber(MidiNote _note,double _tempo)
    {
        curNote = _note;
        tempo = _tempo;
    }

    //void Start()
    //{
    //    t = GetComponent<Transform>();
    //    halfLength = (t.localScale[1] / 2f);
    //    t.position += new Vector3(0f, t.localScale[1] / 2f, 0f);
    //}

    //bool isPlaying = false;
    //public void Play()
    //{
    //    isPlaying = true;
    //}

    //public void Pause()
    //{
    //    isPlaying = false;
    //}

    //void Update()
    //{
    //    //if (isPlaying)
    //    {

    //        //speed = tempo / 4f;
    //        speed = tempo;
    //        t.position += speed * Vector3.down * Time.deltaTime;

    //        if (t.position[1] - halfLength + hitOffset < 0.9864573f && !hasHit)
    //        {
    //            hasHit = true;
    //            //PianoKeyCtrller.PianoNotes[curNote.Note].Play
    //            //    (Color.white,curNote.Velocity,curNote.Length,speed);
    //        }

    //        if (t.position[1] + halfLength + hitOffset < 0)
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}
