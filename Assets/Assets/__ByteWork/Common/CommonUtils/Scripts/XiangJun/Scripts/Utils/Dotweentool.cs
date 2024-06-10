using System;
using System.Text;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;


public class Dotweentool
{
    public static Tweener CurTweener;
    public static Action OnMoveComplete;
    public static Action OnScaleComplete;
    public static Action OnRotateComplete;
    public static void Move(Transform tweenTransform, bool isworld, Vector3 from, Vector3 to, float duration,float delay,Ease ease=Ease.InOutSine)
    {
        if (isworld)
        {
        
      CurTweener=  DOTween.To(() => from, x => tweenTransform.position = x, to, duration)
            .SetEase(ease)
            .SetDelay(delay)
            .OnComplete(MoveComplete).SetAutoKill();
        }
        else 
        {

            CurTweener = DOTween.To(() => from, x => tweenTransform.localPosition = x, to, duration)
                .SetEase(ease)
                    .SetDelay(delay)
                .OnComplete(MoveComplete).SetAutoKill();
        }
    }

    private static  void MoveComplete()
    {
        if (OnMoveComplete != null)
        {
            OnMoveComplete();
            OnMoveComplete = null;
        }
    }

    public static void Scale(Transform tweenTransform, Vector3 from, Vector3 to, float duration, float delay)
    {

        CurTweener = DOTween.To(() => from, x => tweenTransform.localScale = x, to, duration)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnComplete(ScaleComplete).SetAutoKill();
    }

    private static void ScaleComplete()
    {
        if (OnScaleComplete != null)
        {
            OnScaleComplete();
           OnScaleComplete = null;
        }
    }


    public static void Rotate(Transform tweenTransform, bool isworld, Vector3 from, Vector3 to, float duration, float delay,bool loop=false)
    {
        var fixAngles = FixRotateAngles(from, to);
        from = fixAngles.Item1;
        to = fixAngles.Item2;
        if (isworld)
        {

            Tweener tw1 = DOTween.To(() => from, x => tweenTransform.eulerAngles = x, to, duration)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnComplete(RotateComplete).SetAutoKill();
            if (loop)
            {
                tw1.SetLoops(-1, LoopType.Incremental);
            }
            CurTweener = tw1;
        }
        else
        {

            Tweener tw2 = DOTween.To(() => from, x => tweenTransform.localEulerAngles = x, to, duration)
                .SetEase(Ease.Linear)
                .SetDelay(delay);
                
            if (loop)
            {
                tw2.SetLoops(-1, LoopType.Incremental);
            }
            else
            {
                tw2.OnComplete(RotateComplete).SetAutoKill();
            }
            CurTweener = tw2;
        }
    }

    public static Tuple<Vector3, Vector3> FixRotateAngles(Vector3 from, Vector3 to)
    {
        from = new Vector3(NormalizeAngle(from.x), NormalizeAngle(from.y), NormalizeAngle(from.z));
        to = new Vector3(FindNeartestToAngle(from.x, NormalizeAngle(to.x)), FindNeartestToAngle(from.y, NormalizeAngle(to.y)), FindNeartestToAngle(from.z, NormalizeAngle(to.z)));
        return new Tuple<Vector3, Vector3>(from, to);
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle < 0)
        {
            angle += 360;
        }
        return angle % 360;
    }

    private static float FindNeartestToAngle(float from, float to)
    {
        if (Mathf.Abs(from - to) > 180)
        {
            if (from < to)
            {
                to -= 360;
            }
            else
            {
                to += 360;
            }
        }
        return to;
    }

    private static void RotateComplete()
    {
        if (OnRotateComplete != null)
        {
            OnRotateComplete();
            OnRotateComplete = null;
        }
    }
}
