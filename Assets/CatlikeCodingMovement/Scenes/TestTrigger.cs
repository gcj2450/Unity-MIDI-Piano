/****************************************************
    文件：TestTrigger.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    public Transform nextTarget;
    public bool isRoll = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    collision.rigidbody.velocity = Vector3.Reflect(collision.rigidbody.velocity, collision.contacts[0].normal);
    //    Debug.Log(collision.contacts[0].normal);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (isRoll)
        {
            //other.gameObject.GetComponent<Rigidbody>().velocity
        }
        else
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (nextTarget != null)
            {
                Vector3 throwForce = MoveUtils.CalculateBestThrowSpeed(other.gameObject.transform.position, nextTarget.transform.position, 2);
                other.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
            }
        }
    }

    /// <summary>
    /// OnTriggerEnter时计算碰撞点法线方向
    /// </summary>
    /// <param name="other"></param>
    private void CalculateNormalDirection(Collider other)
    {
        // 获取触发器的碰撞体和被触发的碰撞体
        Collider triggerCollider = GetComponent<Collider>();
        Collider otherCollider = other;

        // 获取两个碰撞体的边界盒
        Bounds triggerBounds = triggerCollider.bounds;
        Bounds otherBounds = otherCollider.bounds;

        // 计算两个边界盒的中心点
        Vector3 triggerCenter = triggerBounds.center;
        Vector3 otherCenter = otherBounds.center;

        // 计算射线起点和方向
        Vector3 direction = (otherCenter - triggerCenter).normalized;
        Ray ray = new Ray(triggerCenter, direction);

        // 射线检测获取碰撞点信息
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // 获取碰撞点和法线方向
            Vector3 contactPoint = hitInfo.point;
            Vector3 contactNormal = hitInfo.normal;

            // 在Scene视图中绘制一个小的法线方向线，方便调试
            Debug.DrawLine(contactPoint, contactPoint + contactNormal * 1.0f, Color.red, 2.0f);

            // 打印碰撞点和法线方向
            Debug.Log("碰撞点: " + contactPoint + ", 法线方向: " + contactNormal);

            other.GetComponent<Rigidbody>().velocity = Vector3.Reflect(other.GetComponent<Rigidbody>().velocity, contactNormal);
        }
    }
}
