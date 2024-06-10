using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 移动的过程中，碰到带有Note的物体，播放Note的音符
/// </summary>
public class MidiTrigger : MonoBehaviour
{
    public PianoKeyController PianoKeyCtrller;
    bool isMove = false;
    /// <summary>
    /// 移动速度
    /// </summary>
    float speed = 1;

    public MidiPlayerBoxMgr boxMgr;

    /// <summary>
    /// 移动方向
    /// </summary>
    Vector3 moveDir=new Vector3(1,0,0);
    // Start is called before the first frame update
    void Start()
    {
        speed = boxMgr.MoveSpeed;
        switch (boxMgr.NoteMoveDir)
        {
            case NoteMoveDirection.Vertical:
                moveDir = new Vector3(0, -1, 0);
                break;
            case NoteMoveDirection.Horizontal:
                moveDir = new Vector3(1, 0, 0);
                break;
            default:
                break;
        }
        transform.localPosition = boxMgr.startPos;
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
            transform.position += moveDir * Time.deltaTime * speed;
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
        //Debug.Log("OnTriggerEnter:"+other.gameObject.name);
        if (other.transform.parent==null||other.transform.parent.GetComponent<NoteBoxModel>()==null)
        {
            return;
        }
        MidiNote curNote = other.transform.parent.GetComponent<NoteBoxModel>().curNote;
        PianoKeyCtrller.PianoNotes[curNote.NoteName].Play
            (Color.white, curNote.Velocity, curNote.Length, 1);
    }
}
