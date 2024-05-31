/****************************************************
    文件：MoveUtils.cs
    作者：#CREATEAUTHOR#
    邮箱:  gaocanjun@baidu.com
    日期：#CREATETIME#
    功能：Todo
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtils
{
    //http://answers.unity3d.com/questions/248788/calculating-ball-trajectory-in-full-3d-world.html
    /// <summary>计算射击力度
    /// </summary>
    /// <param name="origin">箭头发射位置，通常为射手的位置</param>
    /// <param name="target">将要射击的目标</param>
    /// <param name="timeToTarget">从发射到射中目标需要的时间</param>
    /// <returns></returns>
    public static Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        float t = timeToTarget;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }
}
