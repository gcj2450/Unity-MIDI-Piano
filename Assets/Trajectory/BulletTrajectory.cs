/****************************************************
    文件：BulletTrajectory.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrajectory : MonoBehaviour
{
    public GameObject bullet;
    public Transform target;
    public float launchAngle; // 发射角度（度数）

    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 确保bulletRigidbody和target已设置
            if (bullet == null || target == null)
            {
                Debug.LogError("bulletRigidbody or target not set.");
                return;
            }

            // 计算初始速度矢量
            GameObject grenade = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

            Vector3 velocity = CalculateLaunchVelocity(grenade.transform.position,target.position);


            // 将计算出的力应用到子弹的Rigidbody上
            grenade.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    Vector3 CalculateLaunchVelocity(Vector3 startPosition, Vector3 targetPosition)
    {
        // 计算子弹起始位置到目标位置的水平距离和高度差
        Vector3 delta = targetPosition - startPosition;
        float horizontalDistance = new Vector3(delta.x, 0, delta.z).magnitude;
        float heightDifference = delta.y;

        // 将发射角度从度数转为弧度
        float angleInRadians = launchAngle * Mathf.Deg2Rad;

        // 计算初始速度的平方
        float initialSpeedSquared = (Physics.gravity.magnitude * horizontalDistance * horizontalDistance) /
                                    (2 * (heightDifference - Mathf.Tan(angleInRadians) * horizontalDistance) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));

        // 若初始速度的平方小于零，则无法计算出合适的速度
        if (initialSpeedSquared < 0)
        {
            Debug.LogError("无法计算出合适的初始速度。请调整发射角度或目标位置。");
            return Vector3.zero;
        }

        // 计算初始速度
        float initialSpeed = Mathf.Sqrt(initialSpeedSquared);

        // 计算初始速度矢量
        Vector3 velocity = new Vector3(delta.x, horizontalDistance * Mathf.Tan(angleInRadians), delta.z).normalized * initialSpeed;

        return velocity;
    }
}
