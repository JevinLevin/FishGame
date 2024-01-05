using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animations : MonoBehaviour
{
    #region Singleton
    public static Animations Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return;
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public enum LoopType
    {
        Single,
        SingleYoYo,
        Loop
    }
    private Dictionary<IAnimation, Action> animations = new();
    
    public IAnimation PlayAnimation(Component target, Action<float> getTarget, float length, float current, float final, Easing.EaseTypeDelegate ease, LoopType loopType, Action onEnd = null)
    {
        IAnimation newAnimation = new Animation<float>(target, getTarget, length, current, final, ease, loopType, Mathf.Lerp);
        animations.Add(newAnimation, onEnd);
        return newAnimation;
    }

    public IAnimation PlayAnimation(Component target, Action<Vector3> getTarget, float length, Vector3 current, Vector3 final, Easing.EaseTypeDelegate ease, LoopType loopType, Action onEnd = null)
    {
        IAnimation newAnimation =new Animation<Vector3>(target, getTarget, length, current, final, ease, loopType, Vector3.Lerp);
        animations.Add(newAnimation, onEnd);
        return newAnimation;
    }

    public void StopAnimation(IAnimation stopAnimation)
    {
        foreach (KeyValuePair<IAnimation, Action> currentAnimation in animations.ToList())
        {
            if (currentAnimation.Key != stopAnimation) continue;
            
            EndAnimation(currentAnimation.Key,currentAnimation.Value);
        }
            
    }

    private void EndAnimation(IAnimation endAnimation, Action endAction)
    {
        endAction?.Invoke();
        KillAnimation(endAnimation);
    }

    private void Update()
    {
        foreach (KeyValuePair<IAnimation, Action> currentAnimation in animations.ToList())
        {
            if (currentAnimation.Key.GetActive() == false)
            {
                WarnAnimation(currentAnimation.Key, "The object running this animation was destroyed");
                continue;
            }

            bool endAnimation = currentAnimation.Key.PlayAnimation();

            if (!endAnimation) continue;
            EndAnimation(currentAnimation.Key,currentAnimation.Value);
        }
    }

    private void WarnAnimation(IAnimation currentAnimation, string warnMsg)
    {
        Debug.LogWarning(warnMsg);
        KillAnimation(currentAnimation);
    }

    private void KillAnimation(IAnimation currentAnimation)
    {
        animations.Remove(currentAnimation);
    }
}
public interface IAnimation
{
    bool PlayAnimation();
    bool GetActive();
}

public class Animation<T> : IAnimation
{
    private readonly WeakReference targetReference;
    
    private readonly Action<T> getTarget;
    
    private readonly float length;
    private readonly T current;
    private readonly T final;
    private readonly Easing.EaseTypeDelegate inEase;
    private readonly Animations.LoopType loopType;
    private readonly Func<T,T,float,T> lerpFunction;

    public Animation(Component target, Action<T> getTarget, float length, T current, T final, Easing.EaseTypeDelegate inEase, Animations.LoopType loopType, Func<T,T,float,T> lerpFunction)
    {
        targetReference = new WeakReference(target);
        
        this.getTarget = getTarget;
        
        this.length = length;
        this.current = current;
        this.final = final;
        this.inEase = inEase;
        this.loopType = loopType;

        this.lerpFunction = lerpFunction;
    }

    private float time = 0.0f;

    public bool PlayAnimation()
    {
        time += Time.deltaTime;
        float t = time / length;

        switch (t)
        {
            case < 1.0f:
                getTarget.Invoke(lerpFunction(current, final, inEase(t)));
                break;
            case < 2.0f when loopType == Animations.LoopType.SingleYoYo:
                getTarget.Invoke(lerpFunction(current, final, inEase(1-(1-t))));
                break;
            default:
                return true;
        }

        return false;
    }

    public bool GetActive()
    {
        if (targetReference != null)
        {
            Component component = targetReference.Target as Component;
            //expensive but i just dont care anymore
            return component != null && component.gameObject != null;
        }

        return false;
    }
    
}