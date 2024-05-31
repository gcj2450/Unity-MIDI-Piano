#region Declare
/******************************************************************************************************************
 * File:        Info.cs
 * Author：     Haobel.com 2015 
 * CreateDate： 2015-12-30
 * Description：弓箭手射击脚本
 *****************************************************************************************************************/
#endregion

using UnityEngine;
using System.Collections;
public class TargetProjectileCtrl : MonoBehaviour
{
    /// <summary>射击的目标
    /// </summary>
    public Transform target;

    /// <summary>箭头预制
    /// </summary>
    public GameObject grenadePrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            Vector3 throwForce = calculateBestThrowSpeed(transform.position, target.transform.position, 2);
            grenade.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
        }
    }

    //http://answers.unity3d.com/questions/248788/calculating-ball-trajectory-in-full-3d-world.html
    /// <summary>计算射击力度
    /// </summary>
    /// <param name="origin">箭头发射位置，通常为射手的位置</param>
    /// <param name="target">将要射击的目标</param>
    /// <param name="timeToTarget">从发射到射中目标需要的时间</param>
    /// <returns></returns>
    private Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
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

    //==================以下代码是另外一个计算的方法，不是很好用，需要限制射击的目标不能太近并且高度也需要和目标高度一致
    void shoot()
    {
        var ang = ElevationAngle(target);
        var shootAng = ang + 15; // shoot 15 degree higher
        // limit the shoot angle to a convenient range:
        shootAng = Mathf.Clamp(shootAng, 15, 85);
        // and shoot:
        GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity) as GameObject;
        grenade.GetComponent<Rigidbody>().velocity = BallisticVel(target, shootAng);
    }

    /// <summary>计算射击速度。
    /// </summary>
    /// <param name="target"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - transform.position; // get target direction 
        float h = dir.y; // get height difference 
        //dir.y = 0; // retain only the horizontal direction 
        var dist = dir.magnitude; // get horizontal distance 
        var a = angle * Mathf.Deg2Rad; // convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle 
        dist += h / Mathf.Tan(a); // correct for small height differences 
        // calculate the velocity magnitude 
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)); return vel * dir.normalized;
    }

    /// <summary>计算和目标的相对角度
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    float ElevationAngle(Transform target)
    {
        // find the cannon->target vector:
        var dir = target.position - transform.position;
        // create a horizontal version of it:
        var dirH = new Vector3(dir.x, 0, dir.y);
        // measure the unsigned angle between them:
        var ang = Vector3.Angle(dir, dirH);
        // add the signal (negative is below the cannon):
        if (dir.y < 0) ang = -ang;
        return ang;
    }

}