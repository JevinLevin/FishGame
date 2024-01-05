using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackgroundTint : MonoBehaviour
{
    private static CanvasGroup canvasGroup;
    private static IAnimation backgroundTintAnimation;

    [SerializeField] private float defaultFadeInLength;
    [SerializeField] private float defaultFadeOutLength;
    private static float defaultFadeInLengthStatic;
    private static float defaultFadeOutLengthStatic;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        Activate(false);
        
        defaultFadeInLengthStatic = defaultFadeInLength;
        defaultFadeOutLengthStatic = defaultFadeOutLength;
    }

    private static void Activate(bool value)
    {
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }

    public static void FadeInBackgroundTint(float fadeLength = 0.0f, Easing.EaseType easeType = Easing.EaseType.OutQuad)
    {
        Activate(true);
        if (fadeLength == 0.0f)
            fadeLength = defaultFadeInLengthStatic;
        PlayFadeAnimation(fadeLength,easeType,1.0f);
    }
    public static void FadeOutBackgroundTint(float fadeLength = 0.0f, Easing.EaseType easeType = Easing.EaseType.OutQuad)
    {
        Activate(false);
        if (fadeLength == 0.0f)
            fadeLength = defaultFadeOutLengthStatic;
        PlayFadeAnimation(fadeLength,easeType,0.0f);
    }

    private static void PlayFadeAnimation(float fadeLength, Easing.EaseType easeType, float final)
    {
        Animations.Instance.StopAnimation(backgroundTintAnimation);
        backgroundTintAnimation = Animations.Instance.PlayAnimation(canvasGroup, newValue => canvasGroup.alpha = newValue, fadeLength, canvasGroup.alpha, final, Easing.FindEaseType(easeType), Animations.LoopType.Single);

    }
}
