using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoGroup : MonoBehaviour 
{
    /// <summary>
    /// 该组键起始ID;
    /// </summary>
    public int StartID;
    public List<PianoKeyBoard> GroupKeys = new List<PianoKeyBoard>();

    void Awake()
    {
        SetID();
    }
    /// <summary>
    /// 从起始ID对该组的键进行赋值
    /// </summary>
    void SetID()
    {
        for (int i = 0; i < GroupKeys.Count; i++)
        {
            GroupKeys[i].ID = StartID + i;
            GroupKeys[i].gameObject.name = (StartID + i).ToString();
        }
    }
}
