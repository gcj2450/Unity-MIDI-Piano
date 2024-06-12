using System;
using UnityEngine;

public class NoteBoxModel : MonoBehaviour
{
    public MidiNote curNote;

    //private Transform t;
    //private MeshRenderer r;
    private float speed;
    private float halfLength;
    private bool hasHit = false;
    private float hitOffset = 0.5f;
    double tempo = 0;
    /// <summary>
    /// 在当前音乐中的索引值
    /// </summary>
    public int id = 0;
    public Action<NoteBoxModel> onTriggerEnter;
    public void SetNoteNumber(MidiNote _note,double _tempo,int _id,Action<NoteBoxModel> _onTriggerEnter)
    {
        curNote = _note;
        tempo = _tempo;
        id = _id;
        gameObject.name = $"{curNote.NoteNumber}_{curNote.NoteName}";
        onTriggerEnter = _onTriggerEnter;
    }

    /// <summary>
    /// Fire Trigger Enter Event
    /// </summary>
    public void FireTriggerEnterEvent()
    {
        if (onTriggerEnter!=null)
        {
            onTriggerEnter(this);
        }
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
