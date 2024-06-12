/****************************************************
    文件：NoteBoxTrigger.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoteBoxTrigger : MonoBehaviour
{
    ///// <summary>
    ///// 是否在该Note上滚动
    ///// </summary>
    //public bool isRoll = false;
    //public Transform nextTarget;
    public NoteBoxModel ParentModel;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ParentModel != null && ParentModel.onTriggerEnter != null)
        {
            ParentModel.onTriggerEnter(ParentModel);
        }

        //if (other.gameObject.GetComponent<Rigidbody>()==null)
        //{
        //    return;
        //}

        //Debug.Log("OnTriggerEnter" + transform.parent.name+"_"+ other.gameObject.name);

        //if (isRoll)
        //{
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
        //else
        //{
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    if (nextTarget != null)
        //    {
        //        Vector3 throwForce = MoveUtils.CalculateBestThrowSpeed(other.gameObject.transform.position, nextTarget.transform.position, timeDuration);
        //        other.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
        //    }
        //}
    }
}
