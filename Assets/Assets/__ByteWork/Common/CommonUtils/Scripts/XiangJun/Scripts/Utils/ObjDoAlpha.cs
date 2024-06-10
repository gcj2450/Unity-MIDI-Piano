using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ObjDoAlpha : MonoBehaviour
{
    [SerializeField]  private Material  _material;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private Color From;
    [SerializeField] private Color To;
    private Color _curColor ;
    [SerializeField] private bool StartOnEnable = true;
    public Action OnTweenComplete;

    private bool _isTweening;
    private void OnEnable()
    {
        _isTweening = false;
        if (StartOnEnable)
        {
            StartTween();
        }

    }

    void Update()
    {
        if (_isTweening)
        {
            _material.SetColor("_Color", _curColor);
        }
    }
    public void StartTween()
    {
        _isTweening = true;
        DOTween.To(() => From, x => _curColor = x, To, duration)
         .SetEase(Ease.Linear)
         .OnComplete(Complete).SetAutoKill();
    }
    private void Complete()
    {
        _isTweening = false;
        if (OnTweenComplete != null)
        {
            OnTweenComplete();
        }
    }
}