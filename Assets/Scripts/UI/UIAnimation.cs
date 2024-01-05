using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class UIAnimation<T> : MonoBehaviour
{
    [FormerlySerializedAs("image")]
    [Header("Components")]
    [SerializeField] protected T component;

    [Header("Attributes")]
    [SerializeField] protected float inLength = 0.1f;
    [SerializeField] protected float outLength = 0.25f;
    public float InLength => inLength;
    public float OutLength =>outLength;

    [Header("Easing")] 
    [SerializeField] private Easing.EaseType inEaseType =  Easing.EaseType.OutQuad;
    [SerializeField] private Easing.EaseType outEaseType = Easing.EaseType.OutQuad;

    [Header("Events")] 
    public UnityEvent OnInAnimationEnd;
    public UnityEvent OnOutAnimationEnd;

    private bool isPaused = false;

    private Coroutine currentCoroutine;
    public static Coroutine currentStaticCoroutine;
    
    private void Awake()
    {
        // Store original value as reference
        SetupAnimation();
    }

    // Cancels the current animation
    private void StopCurrentCoroutine()
    {
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);
    }

    public void TriggerAnimation()
    {
        // Overwrite any previous animation
        StopCoroutine(nameof(TriggerAnimationCoroutine));
        CancelAnimation();

        StartCoroutine(nameof(TriggerAnimationCoroutine));
    }

    private IEnumerator TriggerAnimationCoroutine()
    {
        PlayInAnimation();
        
        // Waits for in animation to end before running the out animation
        while (currentCoroutine != null) yield return null;
        
        PlayOutAnimation();

    }

    public void PlayInAnimation()
    {
        StopCurrentCoroutine();
        currentCoroutine = StartCoroutine(PlayAnimation(CalculateInTarget, inLength, Easing.FindEaseType(inEaseType), OnInAnimationEnd));
    }

    public void PlayOutAnimation()
    {
        StopCurrentCoroutine();
        currentCoroutine = StartCoroutine(PlayAnimation(CalculateOutTarget, outLength, Easing.FindEaseType(outEaseType), OnOutAnimationEnd));
    }


    private IEnumerator PlayAnimation(Action calculateTarget, float length, Easing.EaseTypeDelegate easeFunction, UnityEvent endEvent)
    {

        // By comparing the current scale with the original it ensures the final scale will always be the same and the animation can start from any scale
        calculateTarget();
        
        float animationDuration = 0.0f;

        while (animationDuration < length)
        {
            while(isPaused) yield return null;
            
            ApplyAnimation(easeFunction(animationDuration / length));
            animationDuration += Time.deltaTime;
            yield return null;
        }
        ApplyAnimation(animationDuration / length);
        
        currentCoroutine = null;
        
        endEvent?.Invoke();
    }

    public void PauseAnimation(bool value)
    {
        isPaused = value; 
    }

    public void CancelAnimation()
    {
        CalculateOutTarget();
        ApplyAnimation(1);
    }

    public void JumpToStart()
    {
        StopCurrentCoroutine();
        CalculateOutTarget();
        ApplyAnimation(1);
    }

    public void JumpToEnd()
    {
        StopCurrentCoroutine();
        CalculateInTarget();
        ApplyAnimation(1);
    }

    public void ReSetupAnimation()
    {
        SetupAnimation();
    }

    protected abstract void SetupAnimation();
    protected abstract void CalculateInTarget();
    protected abstract void CalculateOutTarget();
    protected abstract void ApplyAnimation(float t);
}
